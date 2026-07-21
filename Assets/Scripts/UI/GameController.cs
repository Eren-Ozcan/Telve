using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;
using Telve.Gameplay;
using Telve.Meta;
using UnityEngine;

namespace Telve.UI
{
    /// <summary>
    /// Faz 1 dijital prototip orkestratörü (ROADMAP.md: "amaç güzellik
    /// değil, eğlencenin kodda da çalıştığını kanıtlamak"). Resources'tan
    /// veri kütüphanesini yükler, günü (DaySession) yönetir, fincan çevirir,
    /// oyuncunun sıraya dizme kararını (tıkla-seç — tam sürükle-bırak Faz 2
    /// "hissiyat" cilası, bkz. ROADMAP.md Faz 2) reading order'a çevirir ve
    /// turu CustomerEncounter ile çözer. Görsel güncelleme GameView'da.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [SerializeField] int startingGold = 20;

        List<SymbolData> _allSymbols;
        List<CharmData> _allCharmDefinitions;
        List<ComboData> _allCombos;

        /// <summary>Oyuncunun elindeki deste — fincan bundan çekilir. Başlangıçta Common alt kümesi.</summary>
        List<SymbolData> _ownedSymbols;

        /// <summary>Satın alınmış, aktif tılsımlar.</summary>
        List<CharmData> _activeCharms;

        System.Random _rng;
        DaySession _day;
        ComboJournal _journal;
        int _totalWisdom;
        int _newCombosThisRun;

        public IReadOnlyList<SymbolData> CurrentCup { get; private set; } = Array.Empty<SymbolData>();

        /// <summary>CurrentCup içindeki indeksler, oyuncunun tıklama (okuma) sırasıyla.</summary>
        public List<int> ReadingOrderCupIndices { get; } = new();

        /// <summary>Mevcut fincan zaten okundu mu — yeni fincan çekilene kadar tekrar gönderilemez.</summary>
        public bool CurrentCupResolved { get; private set; }

        public bool IsMarketOpen { get; private set; }
        public IReadOnlyList<MarketOffer> CurrentOffers { get; private set; } = Array.Empty<MarketOffer>();

        public int Gold => _day.Gold;
        public bool DayLost => _day.DayLost;
        public bool DayComplete => _day.DayComplete;
        public bool IsMuhtarTurn => _day.IsMuhtarTurn;
        public int CustomerIndex => _day.CurrentCustomerIndex;

        /// <summary>Mevcut sıradaki müşterinin arketipi (gün/koşu bitmişse Regular döner).</summary>
        public CustomerArchetype CurrentArchetype =>
            (_day.DayLost || _day.DayComplete) ? CustomerArchetype.Regular : _day.CurrentProfile().Archetype;

        public IReadOnlyList<ComboData> AllCombos => _allCombos;
        public IReadOnlyCollection<string> DiscoveredComboIds => _journal.DiscoveredComboIds;

        /// <summary>ROADMAP.md Faz 3 "Bilgelik puanı": koşular arası kalıcı toplam.</summary>
        public int TotalWisdom => _totalWisdom;

        public event Action OnStateChanged;
        public event Action<EncounterResult> OnEncounterResolved;
        public event Action<IReadOnlyList<string>> OnNewCombosDiscovered;

        /// <summary>ROADMAP.md Faz 3: koşu (gün) bittiğinde kazanılan bilgelik puanıyla tetiklenir.</summary>
        public event Action<int> OnRunEnded;

        /// <summary>ROADMAP.md Faz 2 sunum katmanı: yeni fincan çekildiğinde (animasyon/ses tetikleyicisi).</summary>
        public event Action OnCupDrawn;

        /// <summary>ROADMAP.md Faz 2 sunum katmanı: pazardan satın alma başarılı olduğunda (ses tetikleyicisi).</summary>
        public event Action<MarketOffer> OnOfferPurchased;

        void Awake()
        {
            _allSymbols = Resources.LoadAll<SymbolData>("Data/Symbols").ToList();
            _allCharmDefinitions = Resources.LoadAll<CharmData>("Data/Charms").ToList();
            _allCombos = Resources.LoadAll<ComboData>("Data/Combos").ToList();

            // docs/design/04-economy.md: "Başlangıç destesi: en yaygın Common alt kümesi."
            _ownedSymbols = _allSymbols.Where(s => s.rarity == SymbolRarity.Common).ToList();
            _activeCharms = new List<CharmData>();
            _journal = new ComboJournal(MetaProgressStore.LoadDiscoveredComboIds());
            _totalWisdom = MetaProgressStore.LoadTotalWisdom();

            _rng = new System.Random();
            _day = new DaySession(startingGold, _rng);
        }

        void Start()
        {
            DrawCup();
        }

        public void DrawCup()
        {
            if (_day.DayLost || _day.DayComplete) return;
            if (IsMarketOpen) return;

            ReadingOrderCupIndices.Clear();
            CurrentCupResolved = false;
            CurrentCup = CupDraw.Draw(_ownedSymbols, _rng, _activeCharms);
            OnStateChanged?.Invoke();
            OnCupDrawn?.Invoke();
        }

        /// <summary>Fincan sırası tıklandığında: okuma sırasındaysa çıkar, değilse sona ekler.</summary>
        public void ToggleCupSlot(int cupIndex)
        {
            if (CurrentCupResolved) return;
            if (cupIndex < 0 || cupIndex >= CurrentCup.Count) return;

            if (!ReadingOrderCupIndices.Remove(cupIndex))
            {
                ReadingOrderCupIndices.Add(cupIndex);
            }

            OnStateChanged?.Invoke();
        }

        public IEnumerable<SymbolData> ReadingOrderSymbols =>
            ReadingOrderCupIndices.Select(i => CurrentCup[i]);

        /// <summary>Sürükle-bırak: okuma sırasındaki iki pozisyonu yer değiştirir (ROADMAP.md Faz 1).</summary>
        public void ReorderReadingOrder(int fromPosition, int toPosition)
        {
            if (CurrentCupResolved) return;
            if (fromPosition < 0 || fromPosition >= ReadingOrderCupIndices.Count) return;
            if (toPosition < 0 || toPosition >= ReadingOrderCupIndices.Count) return;
            if (fromPosition == toPosition) return;

            (ReadingOrderCupIndices[fromPosition], ReadingOrderCupIndices[toPosition]) =
                (ReadingOrderCupIndices[toPosition], ReadingOrderCupIndices[fromPosition]);

            OnStateChanged?.Invoke();
        }

        public void SubmitReading()
        {
            if (CurrentCupResolved) return;
            if (ReadingOrderCupIndices.Count == 0) return;
            if (_day.DayLost || _day.DayComplete) return;

            var readingOrder = ReadingOrderSymbols.ToList();
            var profile = _day.CurrentProfile();
            var result = CustomerEncounter.Resolve(profile, readingOrder, _allCombos, _activeCharms);
            _day.SubmitEncounter(result);
            CurrentCupResolved = true;

            var newlyDiscovered = _journal.RecordEncounter(result.Score.TriggeredCombos);
            if (newlyDiscovered.Count > 0)
            {
                _newCombosThisRun += newlyDiscovered.Count;
                MetaProgressStore.SaveDiscoveredComboIds(_journal.DiscoveredComboIds);
            }

            OnEncounterResolved?.Invoke(result);
            if (newlyDiscovered.Count > 0) OnNewCombosDiscovered?.Invoke(newlyDiscovered);

            if (_day.DayLost || _day.DayComplete)
            {
                int reward = WisdomReward.CalculateRunReward(_day.Gold, _day.DayComplete, _newCombosThisRun);
                _totalWisdom += reward;
                MetaProgressStore.SaveTotalWisdom(_totalWisdom);
                OnRunEnded?.Invoke(reward);
            }

            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// ROADMAP.md Faz 3 çıkış kriteri: "Bir koşu kaybedince 'bir daha'
        /// isteği doğuyor". Bilgelik puanı ve falcı defteri (meta) kalıcı
        /// kalır; deste/tılsımlar ve gün durumu koşuya özgü olduğundan
        /// sıfırlanır.
        /// </summary>
        public void StartNewRun()
        {
            if (!_day.DayLost && !_day.DayComplete) return;

            _ownedSymbols = _allSymbols.Where(s => s.rarity == SymbolRarity.Common).ToList();
            _activeCharms = new List<CharmData>();
            _newCombosThisRun = 0;
            _day = new DaySession(startingGold, _rng);

            DrawCup();
        }

        /// <summary>docs/design/04-economy.md: "Müşteriler arası pazara uğranabilir." Sadece mevcut fincan okunduktan sonra (müşteriler arası) açılabilir.</summary>
        public void OpenMarket()
        {
            if (!CurrentCupResolved) return;
            if (_day.DayLost || _day.DayComplete) return;
            if (IsMarketOpen) return;

            CurrentOffers = MarketSystem.GenerateOffers(
                _allSymbols, _ownedSymbols, _allCharmDefinitions, _activeCharms, _rng, _activeCharms);
            IsMarketOpen = true;
            OnStateChanged?.Invoke();
        }

        public void CloseMarket()
        {
            if (!IsMarketOpen) return;

            IsMarketOpen = false;
            CurrentOffers = Array.Empty<MarketOffer>();
            OnStateChanged?.Invoke();
        }

        /// <summary>Teklifi satın alır ve pazarı kapatır (bir ziyarette bir alım — MVP basitliği).</summary>
        public bool TryBuyOffer(int offerIndex)
        {
            if (!IsMarketOpen) return false;
            if (offerIndex < 0 || offerIndex >= CurrentOffers.Count) return false;

            var offer = CurrentOffers[offerIndex];
            if (!_day.TrySpendGold(offer.Price)) return false;

            if (offer.IsSymbol) _ownedSymbols.Add(offer.Symbol);
            else _activeCharms.Add(offer.Charm);

            OnOfferPurchased?.Invoke(offer);
            CloseMarket();
            return true;
        }
    }
}

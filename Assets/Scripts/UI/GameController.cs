using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;
using Telve.Gameplay;
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
        List<ComboData> _allCombos;
        List<CharmData> _activeCharms; // MVP v0: boş, pazar/tılsım satın alma sonraki iterasyon

        System.Random _rng;
        DaySession _day;

        public IReadOnlyList<SymbolData> CurrentCup { get; private set; } = Array.Empty<SymbolData>();

        /// <summary>CurrentCup içindeki indeksler, oyuncunun tıklama (okuma) sırasıyla.</summary>
        public List<int> ReadingOrderCupIndices { get; } = new();

        public int Gold => _day.Gold;
        public bool DayLost => _day.DayLost;
        public bool DayComplete => _day.DayComplete;
        public bool IsMuhtarTurn => _day.IsMuhtarTurn;
        public int CustomerIndex => _day.CurrentCustomerIndex;

        public event Action OnStateChanged;
        public event Action<EncounterResult> OnEncounterResolved;

        void Awake()
        {
            _allSymbols = Resources.LoadAll<SymbolData>("Data/Symbols").ToList();
            _allCombos = Resources.LoadAll<ComboData>("Data/Combos").ToList();
            _activeCharms = new List<CharmData>();

            _rng = new System.Random();
            _day = new DaySession(startingGold);
        }

        void Start()
        {
            DrawCup();
        }

        public void DrawCup()
        {
            if (_day.DayLost || _day.DayComplete) return;

            ReadingOrderCupIndices.Clear();
            CurrentCup = CupDraw.Draw(_allSymbols, _rng, _activeCharms);
            OnStateChanged?.Invoke();
        }

        /// <summary>Fincan sırası tıklandığında: okuma sırasındaysa çıkar, değilse sona ekler.</summary>
        public void ToggleCupSlot(int cupIndex)
        {
            if (cupIndex < 0 || cupIndex >= CurrentCup.Count) return;

            if (!ReadingOrderCupIndices.Remove(cupIndex))
            {
                ReadingOrderCupIndices.Add(cupIndex);
            }

            OnStateChanged?.Invoke();
        }

        public IEnumerable<SymbolData> ReadingOrderSymbols =>
            ReadingOrderCupIndices.Select(i => CurrentCup[i]);

        public void SubmitReading()
        {
            if (ReadingOrderCupIndices.Count == 0) return;
            if (_day.DayLost || _day.DayComplete) return;

            var readingOrder = ReadingOrderSymbols.ToList();
            var profile = _day.CurrentProfile();
            var result = CustomerEncounter.Resolve(profile, readingOrder, _allCombos, _activeCharms);
            _day.SubmitEncounter(result);

            OnEncounterResolved?.Invoke(result);
            OnStateChanged?.Invoke();
        }
    }
}

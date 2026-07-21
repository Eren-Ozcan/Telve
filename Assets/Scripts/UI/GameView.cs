using System.Collections;
using System.Text;
using Telve.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// GameController'daki durumu placeholder UI'ya yansıtır (ROADMAP.md
    /// Faz 1: "Placeholder görseller (düz renkli daireler + sembol adı
    /// yazısı) yeterli"). Sabit sayıda (MaxCupSlots) önceden sahnede
    /// oluşturulmuş slot GameObject'i aktif/pasif yapılarak değişken
    /// fincan boyutu (5-8 sembol) desteklenir.
    /// </summary>
    public class GameView : MonoBehaviour
    {
        public const int MaxCupSlots = 8;

        [SerializeField] GameController controller;
        [SerializeField] GameObject[] cupSlotRoots = new GameObject[MaxCupSlots];
        [SerializeField] Text[] cupSlotLabels = new Text[MaxCupSlots];
        [SerializeField] Image[] cupSlotBackgrounds = new Image[MaxCupSlots];
        [SerializeField] Image[] cupSlotIcons = new Image[MaxCupSlots];
        public const int MaxMarketOffers = 3;

        [SerializeField] Text readingOrderText;
        [SerializeField] GameObject[] readingOrderChipRoots = new GameObject[MaxCupSlots];
        [SerializeField] Text[] readingOrderChipLabels = new Text[MaxCupSlots];
        [SerializeField] Text statusText;
        [SerializeField] Text resultText;
        [SerializeField] Button submitButton;
        [SerializeField] Button nextCustomerButton;
        [SerializeField] Button newRunButton;
        [SerializeField] Text runSummaryText;
        [SerializeField] Button marketButton;
        [SerializeField] GameObject marketPanel;
        [SerializeField] Button[] marketOfferButtons = new Button[MaxMarketOffers];
        [SerializeField] Text[] marketOfferLabels = new Text[MaxMarketOffers];
        [SerializeField] Image[] marketOfferIcons = new Image[MaxMarketOffers];
        [SerializeField] Button closeMarketButton;
        [SerializeField] RectTransform cupPanelRoot;

        static readonly Color UnselectedColor = new(0.85f, 0.85f, 0.85f);
        static readonly Color SelectedColor = new(1f, 0.85f, 0.3f);

        const float CupFlipPunchScale = 1.15f;
        const float CupFlipDuration = 0.22f;
        const float SymbolRevealDuration = 0.25f;

        void OnEnable()
        {
            controller.OnStateChanged += Refresh;
            controller.OnEncounterResolved += ShowResult;
            controller.OnCupDrawn += PlayCupDrawFeedback;
        }

        void OnDisable()
        {
            controller.OnStateChanged -= Refresh;
            controller.OnEncounterResolved -= ShowResult;
            controller.OnCupDrawn -= PlayCupDrawFeedback;
        }

        void Start()
        {
            for (int i = 0; i < MaxCupSlots; i++)
            {
                var button = cupSlotRoots[i].GetComponent<CupSlotButton>();
                if (button == null) continue;
                button.SlotIndex = i;
                button.Controller = controller;
            }

            for (int i = 0; i < MaxMarketOffers; i++)
            {
                int offerIndex = i; // closure capture
                marketOfferButtons[i].onClick.AddListener(() => controller.TryBuyOffer(offerIndex));
            }

            for (int i = 0; i < MaxCupSlots; i++)
            {
                var chip = readingOrderChipRoots[i].GetComponent<ReadingOrderChip>();
                if (chip == null) continue;
                chip.Position = i;
                chip.Controller = controller;
            }

            Refresh();
        }

        void Refresh()
        {
            for (int i = 0; i < MaxCupSlots; i++)
            {
                bool active = i < controller.CurrentCup.Count;
                cupSlotRoots[i].SetActive(active);
                if (!active) continue;

                var symbol = controller.CurrentCup[i];
                cupSlotLabels[i].text = $"{symbol.displayName}\n{symbol.baseValue}";

                bool selected = controller.ReadingOrderCupIndices.Contains(i);
                cupSlotBackgrounds[i].color = selected ? SelectedColor : UnselectedColor;

                if (i < cupSlotIcons.Length && cupSlotIcons[i] != null)
                {
                    cupSlotIcons[i].sprite = symbol.sprite;
                    cupSlotIcons[i].enabled = symbol.sprite != null;
                }
            }

            if (controller.ReadingOrderCupIndices.Count == 0)
            {
                readingOrderText.text = "Sıra: (henüz seçim yok)";
            }
            else
            {
                var order = new StringBuilder("Sıra: ");
                int n = 1;
                foreach (var symbol in controller.ReadingOrderSymbols)
                {
                    order.Append($"{n}.{symbol.displayName} ");
                    n++;
                }
                readingOrderText.text = order.ToString();
            }

            for (int i = 0; i < MaxCupSlots; i++)
            {
                bool active = i < controller.ReadingOrderCupIndices.Count;
                readingOrderChipRoots[i].SetActive(active);
                if (!active) continue;

                var symbol = controller.CurrentCup[controller.ReadingOrderCupIndices[i]];
                readingOrderChipLabels[i].text = $"{i + 1}. {symbol.displayName}";
            }

            string archetypeSuffix = controller.CurrentArchetype == CustomerArchetype.Regular
                ? string.Empty : $" ({controller.CurrentArchetype})";
            string dayStatus = controller.DayLost ? "GÜN KAYBEDİLDİ"
                : controller.DayComplete ? "GÜN TAMAMLANDI"
                : controller.IsMuhtarTurn ? "Muhtar geldi!"
                : $"Müşteri {controller.CustomerIndex}/{CustomerEconomy.RegularCustomerCount}{archetypeSuffix}";
            statusText.text = $"Altın: {controller.Gold}   {dayStatus}   Bilgelik: {controller.TotalWisdom}";

            bool dayOver = controller.DayLost || controller.DayComplete;
            submitButton.interactable = controller.ReadingOrderCupIndices.Count > 0 && !controller.CurrentCupResolved && !dayOver && !controller.IsMarketOpen;
            nextCustomerButton.interactable = !dayOver && !controller.IsMarketOpen;
            marketButton.interactable = controller.CurrentCupResolved && !dayOver && !controller.IsMarketOpen;

            if (newRunButton != null)
            {
                newRunButton.gameObject.SetActive(dayOver);
                newRunButton.interactable = dayOver;
            }

            if (runSummaryText != null)
            {
                runSummaryText.gameObject.SetActive(dayOver);
                if (dayOver)
                {
                    string bestCombo = controller.BestComboThisRun != null
                        ? controller.BestComboThisRun.displayName
                        : "yok";
                    runSummaryText.text =
                        $"Koşu Özeti — En iyi kombo: {bestCombo}   " +
                        $"Toplam kazanç: {controller.TotalGoldEarnedThisRun} altın   " +
                        $"Defter: {controller.DiscoveredCombosCount}/{controller.TotalCombosCount}";
                }
            }

            marketPanel.SetActive(controller.IsMarketOpen);
            if (controller.IsMarketOpen)
            {
                for (int i = 0; i < MaxMarketOffers; i++)
                {
                    bool hasOffer = i < controller.CurrentOffers.Count;
                    marketOfferButtons[i].gameObject.SetActive(hasOffer);
                    if (!hasOffer) continue;

                    var offer = controller.CurrentOffers[i];
                    string kind = offer.IsSymbol ? "Sembol" : "Tılsım";
                    marketOfferLabels[i].text = $"{kind}: {offer.DisplayName}\n{offer.Price} altın";
                    marketOfferButtons[i].interactable = controller.Gold >= offer.Price;

                    if (i < marketOfferIcons.Length && marketOfferIcons[i] != null)
                    {
                        var offerSprite = offer.IsSymbol ? offer.Symbol.sprite : offer.Charm.icon;
                        marketOfferIcons[i].sprite = offerSprite;
                        marketOfferIcons[i].enabled = offerSprite != null;
                    }
                }
            }
        }

        void ShowResult(EncounterResult result)
        {
            resultText.text =
                $"Taban: {result.Score.BaseScore:0}  Skor: {result.Score.FinalScore:0.#}\n" +
                $"Eşik {(result.Payment.ThresholdMet ? "AŞILDI" : "AŞILAMADI")} — Ödeme: {result.Payment.Payment} altın";
        }

        /// <summary>
        /// ROADMAP.md Faz 2 "Fincan çevirme animasyonu" + "Sembol belirme
        /// efekti": Refresh() (OnStateChanged, önce çalışır) slotları zaten
        /// aktif/etiketli hale getirmiş olur; burada sadece görsel punch/
        /// reveal tween'i tetiklenir. Harici animasyon paketi yok — düz
        /// coroutine tabanlı basit tween (Faz 1 placeholder kalitesine uygun).
        /// </summary>
        void PlayCupDrawFeedback()
        {
            if (cupPanelRoot != null) StartCoroutine(PunchScale(cupPanelRoot));

            for (int i = 0; i < MaxCupSlots; i++)
            {
                if (i < controller.CurrentCup.Count) StartCoroutine(RevealSlot(cupSlotRoots[i].transform));
            }
        }

        IEnumerator PunchScale(RectTransform target)
        {
            float elapsed = 0f;
            while (elapsed < CupFlipDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / CupFlipDuration;
                float scale = Mathf.Lerp(CupFlipPunchScale, 1f, t);
                target.localScale = Vector3.one * scale;
                yield return null;
            }

            target.localScale = Vector3.one;
        }

        IEnumerator RevealSlot(Transform slot)
        {
            float elapsed = 0f;
            while (elapsed < SymbolRevealDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / SymbolRevealDuration;
                slot.localScale = Vector3.one * Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }

            slot.localScale = Vector3.one;
        }

        public void OnSubmitButtonPressed() => controller.SubmitReading();
        public void OnNextCustomerButtonPressed() => controller.DrawCup();
        public void OnNewRunButtonPressed() => controller.StartNewRun();
        public void OnMarketButtonPressed() => controller.OpenMarket();
        public void OnCloseMarketButtonPressed() => controller.CloseMarket();
    }
}

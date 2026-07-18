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
        public const int MaxMarketOffers = 3;

        [SerializeField] Text readingOrderText;
        [SerializeField] Text statusText;
        [SerializeField] Text resultText;
        [SerializeField] Button submitButton;
        [SerializeField] Button nextCustomerButton;
        [SerializeField] Button marketButton;
        [SerializeField] GameObject marketPanel;
        [SerializeField] Button[] marketOfferButtons = new Button[MaxMarketOffers];
        [SerializeField] Text[] marketOfferLabels = new Text[MaxMarketOffers];
        [SerializeField] Button closeMarketButton;

        static readonly Color UnselectedColor = new(0.85f, 0.85f, 0.85f);
        static readonly Color SelectedColor = new(1f, 0.85f, 0.3f);

        void OnEnable()
        {
            controller.OnStateChanged += Refresh;
            controller.OnEncounterResolved += ShowResult;
        }

        void OnDisable()
        {
            controller.OnStateChanged -= Refresh;
            controller.OnEncounterResolved -= ShowResult;
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

            string dayStatus = controller.DayLost ? "GÜN KAYBEDİLDİ"
                : controller.DayComplete ? "GÜN TAMAMLANDI"
                : controller.IsMuhtarTurn ? "Muhtar geldi!"
                : $"Müşteri {controller.CustomerIndex}/{CustomerEconomy.RegularCustomerCount}";
            statusText.text = $"Altın: {controller.Gold}   {dayStatus}";

            bool dayOver = controller.DayLost || controller.DayComplete;
            submitButton.interactable = controller.ReadingOrderCupIndices.Count > 0 && !controller.CurrentCupResolved && !dayOver && !controller.IsMarketOpen;
            nextCustomerButton.interactable = !dayOver && !controller.IsMarketOpen;
            marketButton.interactable = controller.CurrentCupResolved && !dayOver && !controller.IsMarketOpen;

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
                }
            }
        }

        void ShowResult(EncounterResult result)
        {
            resultText.text =
                $"Taban: {result.Score.BaseScore:0}  Skor: {result.Score.FinalScore:0.#}\n" +
                $"Eşik {(result.Payment.ThresholdMet ? "AŞILDI" : "AŞILAMADI")} — Ödeme: {result.Payment.Payment} altın";
        }

        public void OnSubmitButtonPressed() => controller.SubmitReading();
        public void OnNextCustomerButtonPressed() => controller.DrawCup();
        public void OnMarketButtonPressed() => controller.OpenMarket();
        public void OnCloseMarketButtonPressed() => controller.CloseMarket();
    }
}

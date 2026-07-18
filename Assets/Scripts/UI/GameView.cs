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
        [SerializeField] Text readingOrderText;
        [SerializeField] Text statusText;
        [SerializeField] Text resultText;
        [SerializeField] Button submitButton;
        [SerializeField] Button nextCustomerButton;

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
            submitButton.interactable = controller.ReadingOrderCupIndices.Count > 0 && !dayOver;
            nextCustomerButton.interactable = !dayOver;
        }

        void ShowResult(EncounterResult result)
        {
            resultText.text =
                $"Taban: {result.Score.BaseScore:0}  Skor: {result.Score.FinalScore:0.#}\n" +
                $"Eşik {(result.Payment.ThresholdMet ? "AŞILDI" : "AŞILAMADI")} — Ödeme: {result.Payment.Payment} altın";
        }

        public void OnSubmitButtonPressed() => controller.SubmitReading();
        public void OnNextCustomerButtonPressed() => controller.DrawCup();
    }
}

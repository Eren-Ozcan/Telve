using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 3 "2-3 falcı karakteri" + "Bilgelik puanı: kalıcı
    /// açılımlar ağacı" + "Açılabilir sembol desteleri". JournalView'daki
    /// gibi bağımsız bir aç/kapa panel — sabit sayıda (MaxCharacters)
    /// önceden sahnede oluşturulmuş buton, GameController.AllCharacters
    /// üzerinden doldurulur. Karakter seçimi geçerli koşuyu etkilemez,
    /// sadece bir sonraki StartNewRun çağrısında uygulanır.
    /// </summary>
    public class CharacterSelectView : MonoBehaviour
    {
        public const int MaxCharacters = 3;

        [SerializeField] GameController controller;
        [SerializeField] GameObject panel;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;
        [SerializeField] Button[] characterButtons = new Button[MaxCharacters];
        [SerializeField] Text[] characterLabels = new Text[MaxCharacters];

        void Awake()
        {
            // Sabit karakter butonlarının tıklama bağlantısı nesne ömrü
            // boyunca bir kez kurulur — OnEnable'da kurulsaydı ve OnDisable'da
            // kaldırılmasaydı, tekrarlayan enable/disable döngülerinde
            // dinleyiciler birikip her tıklamayı birden çok kez tetiklerdi.
            for (int i = 0; i < MaxCharacters; i++)
            {
                int index = i; // closure capture
                characterButtons[i].onClick.AddListener(() => OnCharacterButtonPressed(index));
            }
        }

        void OnEnable()
        {
            openButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);

            controller.OnStateChanged += RefreshIfOpen;
            panel.SetActive(false);
        }

        void OnDisable()
        {
            openButton.onClick.RemoveListener(Open);
            closeButton.onClick.RemoveListener(Close);
            controller.OnStateChanged -= RefreshIfOpen;
        }

        void Open()
        {
            panel.SetActive(true);
            Refresh();
        }

        void Close() => panel.SetActive(false);

        void RefreshIfOpen()
        {
            if (panel.activeSelf) Refresh();
        }

        void Refresh()
        {
            var characters = controller.AllCharacters;
            for (int i = 0; i < MaxCharacters; i++)
            {
                bool hasCharacter = characters != null && i < characters.Count;
                characterButtons[i].gameObject.SetActive(hasCharacter);
                if (!hasCharacter) continue;

                var character = characters[i];
                bool unlocked = controller.IsCharacterUnlocked(character.characterId);
                bool selected = character.characterId == controller.SelectedCharacterId;

                string status = selected ? "SEÇİLİ" : unlocked ? "Seç" : $"Aç ({character.wisdomCost} bilgelik)";
                characterLabels[i].text = $"{character.displayName}\n{character.description}\n{status}";
                characterButtons[i].interactable = !selected && (unlocked || controller.TotalWisdom >= character.wisdomCost);
            }
        }

        void OnCharacterButtonPressed(int index)
        {
            var characters = controller.AllCharacters;
            if (characters == null || index >= characters.Count) return;

            var character = characters[index];
            if (controller.IsCharacterUnlocked(character.characterId)) controller.SelectCharacter(character.characterId);
            else controller.UnlockCharacter(character.characterId);
        }
    }
}

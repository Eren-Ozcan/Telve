using System.Collections.Generic;
using System.Linq;
using Telve.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Telve.UI
{
    /// <summary>
    /// ROADMAP.md Faz 2 "Falcı defteri v1": GameController.AllCombos
    /// üzerinde sabit sırayla listeler. Keşfedilmemiş kombolar "???" ve
    /// soluk renkte, keşfedilenler adlarıyla ve altın renkte gösterilir.
    /// Satırlar rowPrefab'den runtime'da instantiate edilir (~47 kombo,
    /// sabit slot havuzu diğer view'lardaki gibi pratik değil).
    /// </summary>
    public class JournalView : MonoBehaviour
    {
        [SerializeField] GameController controller;
        [SerializeField] GameObject panel;
        [SerializeField] Text rowPrefab;
        [SerializeField] Transform contentParent;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;

        static readonly Color DiscoveredColor = new(1f, 0.85f, 0.3f);
        static readonly Color UndiscoveredColor = new(0.5f, 0.5f, 0.5f);

        void OnEnable()
        {
            openButton.onClick.AddListener(Open);
            closeButton.onClick.AddListener(Close);
            controller.OnNewCombosDiscovered += OnNewCombosDiscovered;
            panel.SetActive(false);
        }

        void OnDisable()
        {
            openButton.onClick.RemoveListener(Open);
            closeButton.onClick.RemoveListener(Close);
            controller.OnNewCombosDiscovered -= OnNewCombosDiscovered;
        }

        void Open()
        {
            panel.SetActive(true);
            RefreshRows();
        }

        void Close() => panel.SetActive(false);

        void OnNewCombosDiscovered(IReadOnlyList<string> _)
        {
            if (panel.activeSelf) RefreshRows();
        }

        void RefreshRows()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }

            foreach (var combo in controller.AllCombos)
            {
                bool discovered = controller.DiscoveredComboIds.Contains(combo.comboId);
                var row = Instantiate(rowPrefab, contentParent);
                row.text = discovered ? combo.displayName : "???";
                row.color = discovered ? DiscoveredColor : UndiscoveredColor;
                row.gameObject.SetActive(true);
            }
        }
    }
}

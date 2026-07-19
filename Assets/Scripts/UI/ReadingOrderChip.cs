using UnityEngine;
using UnityEngine.EventSystems;

namespace Telve.UI
{
    /// <summary>
    /// GameView'ın önceden sahneye koyduğu sabit sayıdaki (bkz. GameView
    /// ReadingOrderChip havuzu) okuma-sırası chip'lerinden biri. Havuzdaki
    /// slot index'i (Position) hep aynı okuma-sırası pozisyonunu temsil eder
    /// — CupSlotButton'daki sabit-slot deseniyle aynı mantık. Sürükleyip
    /// başka bir chip'in üstüne bırakınca GameController.ReorderReadingOrder
    /// ile iki pozisyon yer değiştirir; GameView.Refresh() metinleri yeni
    /// sıraya göre günceller.
    /// </summary>
    public class ReadingOrderChip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public int Position;
        public GameController Controller;

        [SerializeField] CanvasGroup canvasGroup;

        RectTransform _rect;
        Canvas _rootCanvas;
        Vector2 _dragStartAnchoredPos;

        void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _rootCanvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartAnchoredPos = _rect.anchoredPosition;
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0.6f;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            float scale = _rootCanvas != null ? _rootCanvas.scaleFactor : 1f;
            _rect.anchoredPosition += eventData.delta / scale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
            }

            // Chip havuzdaki sabit slotunda kalır; görünen sırayı
            // GameView.Refresh() metinle günceller. Sürükleme sırasında
            // taşınan görsel her zaman kendi sabit konumuna geri döner.
            _rect.anchoredPosition = _dragStartAnchoredPos;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Controller == null || eventData.pointerDrag == null) return;

            var draggedChip = eventData.pointerDrag.GetComponent<ReadingOrderChip>();
            if (draggedChip == null || draggedChip == this) return;

            Controller.ReorderReadingOrder(draggedChip.Position, Position);
        }
    }
}

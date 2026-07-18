using UnityEngine;

namespace Telve.UI
{
    /// <summary>
    /// GameView'ın önceden sahneye koyduğu sabit sayıdaki (bkz. GameView
    /// CupSlots) fincan sırası butonlarından biri. Tıklanınca kendi
    /// SlotIndex'ini GameController.ToggleCupSlot'a iletir.
    /// </summary>
    public class CupSlotButton : MonoBehaviour
    {
        public int SlotIndex;
        public GameController Controller;

        public void OnTapped()
        {
            if (Controller != null) Controller.ToggleCupSlot(SlotIndex);
        }
    }
}

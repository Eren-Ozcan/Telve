using System;
using System.Collections.Generic;

namespace Telve.Gameplay
{
    /// <summary>
    /// docs/design/00-core-loop.md "Gün Döngüsü": 8 sıradan müşteri +
    /// muhtar, altın takibi. Sıradan müşteri eşiği kaçırılırsa gün
    /// devam eder (sadece düşük ödeme); muhtar kaçırılırsa gün kaybedilir
    /// (roguelike ölüm koşulu).
    /// </summary>
    public class DaySession
    {
        readonly List<CustomerResult> _history = new();

        public int Gold { get; private set; }
        public int CurrentCustomerIndex { get; private set; } = 1; // 1..8, sonra muhtar
        public bool DayLost { get; private set; }
        public bool DayComplete { get; private set; }
        public IReadOnlyList<CustomerResult> History => _history;

        public bool IsMuhtarTurn => CurrentCustomerIndex > CustomerEconomy.RegularCustomerCount;

        public DaySession(int startingGold)
        {
            Gold = startingGold;
        }

        public CustomerProfile CurrentProfile()
        {
            ThrowIfDayOver();
            return IsMuhtarTurn ? CustomerProfile.Muhtar() : CustomerProfile.Regular(CurrentCustomerIndex);
        }

        public CustomerResult SubmitEncounter(EncounterResult encounter)
        {
            ThrowIfDayOver();

            _history.Add(encounter.Payment);
            Gold += encounter.Payment.Payment;

            if (IsMuhtarTurn)
            {
                if (encounter.Payment.ThresholdMet) DayComplete = true;
                else DayLost = true;
            }
            else
            {
                CurrentCustomerIndex++;
            }

            return encounter.Payment;
        }

        void ThrowIfDayOver()
        {
            if (DayLost || DayComplete)
                throw new InvalidOperationException("Gün zaten bitti (kazanıldı ya da kaybedildi).");
        }
    }
}

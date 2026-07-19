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
        static readonly CustomerArchetype[] ArchetypePool =
        {
            CustomerArchetype.Regular, CustomerArchetype.Aceleci, CustomerArchetype.Kuskucu,
            CustomerArchetype.Dertli, CustomerArchetype.Comert,
        };

        readonly List<CustomerResult> _history = new();
        readonly CustomerArchetype[] _archetypes;

        public int Gold { get; private set; }
        public int CurrentCustomerIndex { get; private set; } = 1; // 1..8, sonra muhtar
        public bool DayLost { get; private set; }
        public bool DayComplete { get; private set; }
        public IReadOnlyList<CustomerResult> History => _history;

        public bool IsMuhtarTurn => CurrentCustomerIndex > CustomerEconomy.RegularCustomerCount;

        public DaySession(int startingGold) : this(startingGold, null) { }

        /// <summary>
        /// ROADMAP.md Faz 2: rng verilirse her sıradan müşteriye rastgele
        /// bir CustomerArchetype atanır (bkz. ArchetypePool). rng null ise
        /// (geriye dönük uyumluluk) tüm müşteriler Regular kalır — eski
        /// davranışla birebir aynı sayılar.
        /// </summary>
        public DaySession(int startingGold, System.Random rng)
        {
            Gold = startingGold;

            _archetypes = new CustomerArchetype[CustomerEconomy.RegularCustomerCount];
            for (int i = 0; i < _archetypes.Length; i++)
            {
                _archetypes[i] = rng != null ? ArchetypePool[rng.Next(ArchetypePool.Length)] : CustomerArchetype.Regular;
            }
        }

        public CustomerProfile CurrentProfile()
        {
            ThrowIfDayOver();
            return IsMuhtarTurn
                ? CustomerProfile.Muhtar()
                : CustomerProfile.Regular(CurrentCustomerIndex, _archetypes[CurrentCustomerIndex - 1]);
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

        /// <summary>Pazar alışverişi için. Yetersiz altında false döner, hiçbir şey değişmez.</summary>
        public bool TrySpendGold(int amount)
        {
            if (amount < 0 || amount > Gold) return false;
            Gold -= amount;
            return true;
        }

        void ThrowIfDayOver()
        {
            if (DayLost || DayComplete)
                throw new InvalidOperationException("Gün zaten bitti (kazanıldı ya da kaybedildi).");
        }
    }
}

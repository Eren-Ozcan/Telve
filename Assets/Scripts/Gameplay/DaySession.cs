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

        /// <summary>ROADMAP.md Faz 3 "Kayıt sistemi": koşu ortası kayıt için sıradan müşteri arketiplerinin (1..8) dışa açılan hâli.</summary>
        public IReadOnlyList<CustomerArchetype> Archetypes => _archetypes;

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

        /// <summary>
        /// ROADMAP.md Faz 3 "Kayıt sistemi: koşu ortası kayıt/devam".
        /// Kaydedilmiş bir koşuyu, orijinal arketip atamalarını ve
        /// geçmişini koruyarak birebir geri kurar (yeni bir DaySession
        /// oluşturmak arketipleri rastgele yeniden atardı — bu yüzden ayrı
        /// bir yol). RNG durumu kasıtlı olarak kurtarılmıyor: System.Random
        /// iç durumunu dışa açmıyor, bundan sonraki çekilişler yeni bir
        /// rastgelelikle devam eder — zaten çekilmiş/geçmişe işlenmiş hiçbir
        /// şeyi etkilemez.
        /// </summary>
        public static DaySession Restore(
            int gold, int currentCustomerIndex, bool dayLost, bool dayComplete,
            List<CustomerResult> history, CustomerArchetype[] archetypes)
        {
            var session = new DaySession(gold, null)
            {
                CurrentCustomerIndex = currentCustomerIndex,
                DayLost = dayLost,
                DayComplete = dayComplete,
            };

            for (int i = 0; i < archetypes.Length && i < session._archetypes.Length; i++)
            {
                session._archetypes[i] = archetypes[i];
            }

            session._history.AddRange(history);
            return session;
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

        /// <summary>
        /// ROADMAP.md Faz 4 "Rewarded ad: koşu sonu ikinci şans". Sadece
        /// muhtar'a kaybedildiğinde geçerli (sıradan müşteri kaybı yok —
        /// bkz. SubmitEncounter); muhtar turu tekrar denenebilir hale gelir.
        /// Kaç kez kullanılabileceği (roadmap: "2 nokta, fazlası deneyimi
        /// yer") bu sınıfın değil, çağıranın (GameController) sorumluluğu.
        /// </summary>
        public bool TryGrantSecondChance()
        {
            if (!DayLost || !IsMuhtarTurn) return false;

            DayLost = false;
            return true;
        }

        void ThrowIfDayOver()
        {
            if (DayLost || DayComplete)
                throw new InvalidOperationException("Gün zaten bitti (kazanıldı ya da kaybedildi).");
        }
    }
}

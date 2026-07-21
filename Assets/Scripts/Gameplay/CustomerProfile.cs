namespace Telve.Gameplay
{
    /// <summary>
    /// docs/design/04-economy.md müşteri eşik/ödeme tablosu. Regular(n)
    /// üretir 1-8 sıradaki sıradan müşterileri; Muhtar() gün sonu boss'u
    /// (tabloda formülden bağımsız sabit 35 taban ödeme ile listelenir).
    /// ROADMAP.md Faz 2: arketipler (bkz. CustomerArchetype) eşik/ödemeyi
    /// hafifçe çarpar; Muhtar "kötü fala inanmaz" (PunishesNegativeCombos)
    /// ile negatif kombo cezasını ScoreCalculator'da şiddetlendirir.
    /// </summary>
    public readonly struct CustomerProfile
    {
        public readonly int Threshold;
        public readonly int BasePayment;
        public readonly bool IsMuhtar;
        public readonly CustomerArchetype Archetype;
        public readonly bool PunishesNegativeCombos;

        CustomerProfile(int threshold, int basePayment, bool isMuhtar, CustomerArchetype archetype, bool punishesNegativeCombos)
        {
            Threshold = threshold;
            BasePayment = basePayment;
            IsMuhtar = isMuhtar;
            Archetype = archetype;
            PunishesNegativeCombos = punishesNegativeCombos;
        }

        /// <param name="customerIndex">Gün içi sıra, 1-8.</param>
        /// <param name="archetype">Varsayılan Regular — DaySession sıradan müşteriler için rastgele atar.</param>
        public static CustomerProfile Regular(int customerIndex, CustomerArchetype archetype = CustomerArchetype.Regular)
        {
            if (customerIndex < 1 || customerIndex > CustomerEconomy.RegularCustomerCount)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(customerIndex),
                    $"customerIndex 1-{CustomerEconomy.RegularCustomerCount} aralığında olmalı.");
            }

            int threshold = 12 + customerIndex * 4;
            int basePayment = 6 + customerIndex * 2;

            var (thresholdMult, paymentMult) = ArchetypeMultipliers(archetype);
            threshold = (int)System.MathF.Round(threshold * thresholdMult);
            basePayment = (int)System.MathF.Round(basePayment * paymentMult);

            return new CustomerProfile(threshold, basePayment, isMuhtar: false, archetype, punishesNegativeCombos: false);
        }

        /// <summary>
        /// Eşik çarpanı ROADMAP.md Faz 3 "Denge turu" simülasyonuyla (20 koşu,
        /// BalanceSimulator) 1.5×'ten 1.2×'e düşürüldü: 1.5×'te (eşik 66)
        /// açgözlü pazar alışverişi + optimal dizilimle bile muhtar geçme
        /// oranı %25, ortalama ulaşılan skor eşiğin ~19 puan altındaydı.
        /// 1.2× (eşik ~53) bu farkı kapatırken muhtar'ı hâlâ 8. müşteriden
        /// zorlu tutuyor. Gerçek oyuncu verisiyle yeniden ayarlanabilir.
        /// </summary>
        const float MuhtarThresholdMultiplier = 1.2f;

        public static CustomerProfile Muhtar()
        {
            var lastRegular = Regular(CustomerEconomy.RegularCustomerCount);
            int threshold = (int)System.MathF.Round(lastRegular.Threshold * MuhtarThresholdMultiplier);
            return new CustomerProfile(threshold, basePayment: 35, isMuhtar: true, CustomerArchetype.Regular, punishesNegativeCombos: true);
        }

        /// <summary>
        /// Taslak çarpanlar (docs/design/04-economy.md Faz 3 denge turunda
        /// kesinleşecek): Aceleci kolay ikna olur ama az öder; Kuşkucu ikna
        /// etmesi zor ama iyi öder; Dertli hem daha zor hem daha cömert;
        /// Cömert eşiği aynı ama ödemesi yüksek.
        /// </summary>
        static (float thresholdMult, float paymentMult) ArchetypeMultipliers(CustomerArchetype archetype) => archetype switch
        {
            CustomerArchetype.Aceleci => (0.85f, 0.85f),
            CustomerArchetype.Kuskucu => (1.15f, 1.15f),
            CustomerArchetype.Dertli => (1.1f, 1.2f),
            CustomerArchetype.Comert => (1f, 1.3f),
            _ => (1f, 1f),
        };
    }
}

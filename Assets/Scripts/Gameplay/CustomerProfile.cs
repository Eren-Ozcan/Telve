namespace Telve.Gameplay
{
    /// <summary>
    /// docs/design/04-economy.md müşteri eşik/ödeme tablosu. Regular(n)
    /// üretir 1-8 sıradaki sıradan müşterileri; Muhtar() gün sonu boss'u
    /// (tabloda formülden bağımsız sabit 35 taban ödeme ile listelenir).
    /// </summary>
    public readonly struct CustomerProfile
    {
        public readonly int Threshold;
        public readonly int BasePayment;
        public readonly bool IsMuhtar;

        CustomerProfile(int threshold, int basePayment, bool isMuhtar)
        {
            Threshold = threshold;
            BasePayment = basePayment;
            IsMuhtar = isMuhtar;
        }

        /// <param name="customerIndex">Gün içi sıra, 1-8.</param>
        public static CustomerProfile Regular(int customerIndex)
        {
            if (customerIndex < 1 || customerIndex > CustomerEconomy.RegularCustomerCount)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(customerIndex),
                    $"customerIndex 1-{CustomerEconomy.RegularCustomerCount} aralığında olmalı.");
            }

            int threshold = 12 + customerIndex * 4;
            int basePayment = 6 + customerIndex * 2;
            return new CustomerProfile(threshold, basePayment, isMuhtar: false);
        }

        public static CustomerProfile Muhtar()
        {
            var lastRegular = Regular(CustomerEconomy.RegularCustomerCount);
            int threshold = (int)System.MathF.Round(lastRegular.Threshold * 1.5f);
            return new CustomerProfile(threshold, basePayment: 35, isMuhtar: true);
        }
    }
}

namespace Telve.Gameplay
{
    public readonly struct CustomerResult
    {
        public readonly bool ThresholdMet;
        public readonly int Payment;

        public CustomerResult(bool thresholdMet, int payment)
        {
            ThresholdMet = thresholdMet;
            Payment = payment;
        }
    }

    /// <summary>
    /// docs/design/04-economy.md ödeme kuralı: eşik aşılırsa taban ödeme
    /// + aşım başına %30 bonus; aşılamazsa taban ödemenin %40'ı. Taslak
    /// sayılar — Faz 3'te 20+ koşu verisiyle yeniden dengelenecek.
    /// </summary>
    public static class CustomerEconomy
    {
        public const int RegularCustomerCount = 8;

        const float BonusPerExcessPoint = 0.3f;
        const float BelowThresholdPaymentRatio = 0.4f;

        public static CustomerResult Evaluate(CustomerProfile profile, float satisfaction)
        {
            if (satisfaction >= profile.Threshold)
            {
                float excess = satisfaction - profile.Threshold;
                int payment = profile.BasePayment + (int)System.MathF.Round(excess * BonusPerExcessPoint);
                return new CustomerResult(thresholdMet: true, payment);
            }

            int reducedPayment = (int)System.MathF.Round(profile.BasePayment * BelowThresholdPaymentRatio);
            return new CustomerResult(thresholdMet: false, reducedPayment);
        }
    }
}

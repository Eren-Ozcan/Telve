using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;

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

        public static CustomerResult Evaluate(
            CustomerProfile profile,
            float satisfaction,
            IReadOnlyList<CharmData> activeCharms = null)
        {
            int payment;
            bool thresholdMet;

            if (satisfaction >= profile.Threshold)
            {
                float excess = satisfaction - profile.Threshold;
                payment = profile.BasePayment + (int)MathF.Round(excess * BonusPerExcessPoint);
                thresholdMet = true;
            }
            else
            {
                payment = (int)MathF.Round(profile.BasePayment * BelowThresholdPaymentRatio);
                thresholdMet = false;
            }

            // Muhtarın Gözdesi: "Muhtar müşterisinde ödeme ×1.5".
            if (profile.IsMuhtar && activeCharms != null && activeCharms.Any(c => c.charmId == "muhtarin_gozdesi"))
            {
                payment = (int)MathF.Round(payment * 1.5f);
            }

            return new CustomerResult(thresholdMet, payment);
        }
    }
}

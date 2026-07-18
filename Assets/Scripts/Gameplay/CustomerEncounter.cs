using System.Collections.Generic;
using Telve.Data;

namespace Telve.Gameplay
{
    public readonly struct EncounterResult
    {
        public readonly ScoreResult Score;
        public readonly CustomerResult Payment;

        public EncounterResult(ScoreResult score, CustomerResult payment)
        {
            Score = score;
            Payment = payment;
        }
    }

    /// <summary>
    /// docs/design/00-core-loop.md "Bir Müşteri Turu": okuma sırası
    /// belirlendikten sonraki adımları (puanla → öde/tepki) birleştirir.
    /// Fincan çevirme (CupDraw) ve okuma sırası kararı (oyuncu girdisi)
    /// bu sınıfın dışında, önceden yapılmış olmalı.
    /// </summary>
    public static class CustomerEncounter
    {
        public static EncounterResult Resolve(
            CustomerProfile profile,
            IReadOnlyList<SymbolData> readingOrder,
            IReadOnlyList<ComboData> comboLibrary,
            IReadOnlyList<CharmData> activeCharms = null)
        {
            var score = ScoreCalculator.Calculate(readingOrder, comboLibrary, activeCharms);
            var payment = CustomerEconomy.Evaluate(profile, score.FinalScore, activeCharms);
            return new EncounterResult(score, payment);
        }
    }
}

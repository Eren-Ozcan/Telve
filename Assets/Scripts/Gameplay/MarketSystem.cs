using System.Collections.Generic;
using System.Linq;
using Telve.Data;

namespace Telve.Gameplay
{
    /// <summary>
    /// docs/design/04-economy.md: "Her pazar ziyaretinde 3 seçenek sunulur."
    /// Seçenekler henüz sahip olunmayan sembol/tılsım havuzundan nadirlik
    /// ağırlıklı (CupDraw ile aynı mantık) rastgele seçilir.
    /// </summary>
    public static class MarketSystem
    {
        public static List<MarketOffer> GenerateOffers(
            IReadOnlyList<SymbolData> allSymbols,
            IReadOnlyList<SymbolData> ownedSymbols,
            IReadOnlyList<CharmData> allCharms,
            IReadOnlyList<CharmData> ownedCharms,
            System.Random rng,
            IReadOnlyList<CharmData> activeCharmsForPricing,
            int count = 3)
        {
            var symbolPool = allSymbols.Where(s => !ownedSymbols.Contains(s)).ToList();
            var charmPool = allCharms.Where(c => !ownedCharms.Contains(c)).ToList();

            var offers = new List<MarketOffer>();
            for (int i = 0; i < count; i++)
            {
                if (symbolPool.Count == 0 && charmPool.Count == 0) break;

                bool offerSymbol = charmPool.Count == 0 || (symbolPool.Count > 0 && rng.NextDouble() < 0.6);

                if (offerSymbol)
                {
                    var symbol = WeightedPickSymbol(symbolPool, rng);
                    int price = MarketPricing.SymbolPrice(symbol.rarity, activeCharmsForPricing);
                    offers.Add(new MarketOffer(symbol, price));
                }
                else
                {
                    var charm = charmPool[rng.Next(charmPool.Count)];
                    int price = MarketPricing.CharmPrice(charm, activeCharmsForPricing);
                    offers.Add(new MarketOffer(charm, price));
                }
            }

            return offers;
        }

        static SymbolData WeightedPickSymbol(IReadOnlyList<SymbolData> pool, System.Random rng)
        {
            int total = pool.Sum(s => s.drawWeight);
            if (total <= 0) return pool[rng.Next(pool.Count)];

            int roll = rng.Next(total);
            int cumulative = 0;
            foreach (var symbol in pool)
            {
                cumulative += symbol.drawWeight;
                if (roll < cumulative) return symbol;
            }

            return pool[^1];
        }
    }
}

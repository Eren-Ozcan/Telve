using System;
using System.Collections.Generic;
using System.Linq;
using Telve.Data;

namespace Telve.Gameplay
{
    /// <summary>
    /// docs/design/04-economy.md pazar fiyat tablosu. Sembol fiyatı
    /// SymbolData'da tutulmaz — baseValue'dan bağımsız, sadece
    /// nadirliğe bağlı bir pazar özelliğidir.
    /// </summary>
    public static class MarketPricing
    {
        public static int SymbolPrice(SymbolRarity rarity, IReadOnlyList<CharmData> activeCharms = null)
        {
            int basePrice = rarity switch
            {
                SymbolRarity.Common => 8,
                SymbolRarity.Uncommon => 15,
                SymbolRarity.Rare => 28,
                SymbolRarity.Epic => 50,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity)),
            };

            return ApplyEkonomikFal(basePrice, activeCharms);
        }

        public static int CharmPrice(CharmData charm, IReadOnlyList<CharmData> activeCharms = null)
        {
            return ApplyEkonomikFal(charm.price, activeCharms);
        }

        // Ekonomik Fal: "Pazar fiyatları %15 düşer".
        static int ApplyEkonomikFal(int basePrice, IReadOnlyList<CharmData> activeCharms)
        {
            if (activeCharms == null || !activeCharms.Any(c => c.charmId == "ekonomik_fal"))
                return basePrice;

            return (int)MathF.Round(basePrice * 0.85f);
        }
    }
}

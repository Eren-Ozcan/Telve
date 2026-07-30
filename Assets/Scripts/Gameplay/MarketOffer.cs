using Telve.Data;

namespace Telve.Gameplay
{
    /// <summary>One pazar seçeneği: ya bir sembol ya da bir tılsım, asla ikisi birden.</summary>
    public readonly struct MarketOffer
    {
        public readonly SymbolData Symbol;
        public readonly CharmData Charm;
        public readonly int Price;

        public MarketOffer(SymbolData symbol, int price)
        {
            Symbol = symbol;
            Charm = null;
            Price = price;
        }

        public MarketOffer(CharmData charm, int price)
        {
            Symbol = null;
            Charm = charm;
            Price = price;
        }

        public bool IsSymbol => Symbol != null;
    }
}

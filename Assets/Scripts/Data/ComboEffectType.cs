namespace Telve.Data
{
    /// <summary>
    /// See docs/design/03-scoring.md step 3-4: flat bonuses are summed into
    /// the base score before any multiplier is applied.
    /// </summary>
    public enum ComboEffectType
    {
        Multiplier,
        Flat,
    }
}

namespace Telve.Data
{
    /// <summary>
    /// Which stage of the docs/design/03-scoring.md pipeline (or which
    /// out-of-band system) a charm's effectValue applies to. See the
    /// classification table in docs/design/05-charms.md.
    /// </summary>
    public enum CharmEffectTarget
    {
        /// <summary>Adds to a matching symbol's baseValue (e.g. Kuş Tüyü).</summary>
        SymbolValue,

        /// <summary>Flat addition to the base score (e.g. Sadık Dost).</summary>
        FlatBonus,

        /// <summary>Multiplies a triggered combo's multiplier (e.g. İlk Kombo Çarpanı).</summary>
        ComboMultiplier,

        /// <summary>Suppresses or dampens negative combo penalties (e.g. Şanslı Nazar, Kara Kedi Tılsımı).</summary>
        NegativeComboSuppression,

        /// <summary>Affects fincan çevirme draw pool/weights, before scoring (e.g. Şans Tekerleği, Kader Anahtarı).</summary>
        DrawRng,

        /// <summary>Affects market prices or customer payment, after scoring (e.g. Ekonomik Fal, Muhtarın Gözdesi).</summary>
        Economy,
    }
}

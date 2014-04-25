using Flai;

namespace Assets.Misc
{
    public static class BaratieConstants
    {
        public static readonly LayerMaskF IgnorePlayerLayerMask = LayerMaskF.FromNames("Player", "PlayerHoldingCrate", "Funnel").Inverse;
    }
}

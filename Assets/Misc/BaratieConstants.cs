using Flai;

namespace Assets.Misc
{
    public static class BaratieConstants
    {
        public static LayerMaskF IgnorePlayerLayerMask
        {
            get { return LayerMaskF.FromNames("Player", "PlayerHoldingCrate", "Funnel").Inverse; }
        }
    }
}

using Assets.Scripts.General;
using Assets.Scripts.Objects.Button;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using Flai.Scene;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    // ...................................................................................
    // Make a generic "ActionManager" or somethings.... god damnit......

    public class ToggleButtonPresser : FlaiScript
    {
        private static LayerMaskF IgnoreMask
        {
            get { return LayerMaskF.FromNames("Crates", "Player", "PlayerHoldingCrate", "Funnel", "Keys").Inverse; }
        }

        private PlayerController Controller
        {
            get { return this.Get<PlayerController>(); }
        }

        protected override void Update()
        {
            if (FlaiInput.IsNewKeyPress(KeyCode.F))
            {
                this.TryPressButton();
            }
        }

        private void TryPressButton()
        {
            RectangleF target = this.GetComponent<Collider2D>().GetBoundsHack().AsInflated(0.3f, Tile.Size * 0.4f);
            target.Center += this.Controller.FacingDirection.ToUnitVector() * target.Width * 0.5f;
            var toggleButtons = Scene.FindAllOfType<ToggleButton>().ToSet();
            var toggleButton =
                toggleButtons.FirstOrDefault(c =>
                {
                    var rc = Physics2D.Linecast(this.Position2D, c.Position2D, ToggleButtonPresser.IgnoreMask);
                    return c.Get<Collider2D>().GetBoundsHack().Intersects(target) && (!rc || rc.transform.Has<ToggleButton>());
                });

            if (toggleButton != null)
            {
                toggleButton.Toggle();
            }

            // draw the pick area
            FlaiDebug.DrawRectangleOutlines(target, ColorF.Red, 0.5f);
        }

    }
}

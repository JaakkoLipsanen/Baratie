using System.CodeDom;
using System.Linq;
using Assets.Scripts.General;
using Assets.Scripts.Objects;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using Flai.Scene;

namespace Assets.Scripts.Player
{
    public class CratePicker : FlaiScript
    {
        private Crate _currentlyPickingCrate;
        private PlayerController _controller;

        public bool IsPicking
        {
            get { return _currentlyPickingCrate != null; }
        }

        protected override void Awake()
        {
            _controller = this.Get<PlayerController>();
        }

        protected override void Update()
        {
            this.HandleInput();
            if (this.IsPicking)
            {
                this.UpdateCratePosition();
            }
        }

        private void UpdateCratePosition()
        {
            const float HorizontalDistance = Tile.Size*1.5f;
            const float VerticalDistance = Tile.Size * 1.65f;

            _currentlyPickingCrate.SetPosition2D(this.Position2D + _controller.FacingDirection.ToUnitVector() * HorizontalDistance - _controller.GroundDirection.ToUnitVector() * VerticalDistance);
        }

        private void HandleInput()
        {
            if (FlaiInput.IsNewButtonPress("Pick Crate"))
            {
                if (this.IsPicking)
                {
                    _currentlyPickingCrate.Drop();
                    _currentlyPickingCrate = null;
                }
                else
                {
                    this.TryPickUpCrate();
                }
            }
        }

        private void TryPickUpCrate()
        {
            RectangleF target = this.collider2D.GetBoundsHack();
            target.Center += _controller.FacingDirection.ToUnitVector() * target.Width;
            var crates = Scene.FindAllOfType<Crate>().ToSet();
            var crate = crates.FirstOrDefault(c => c.collider2D.GetBoundsHack().Intersects(target));

            if (crate != null)
            {
                _currentlyPickingCrate = crate;
                crate.Pick(this.GameObject);
            }

            // draw the pick area
            FlaiDebug.DrawRectangleOutlines(target, ColorF.White, 0.5f);
        }
    }
}
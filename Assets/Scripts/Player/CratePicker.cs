using Assets.Scripts.General;
using Assets.Scripts.Objects;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using Flai.Scene;
using System.Linq;

namespace Assets.Scripts.Player
{
    public class CratePicker : FlaiScript
    {
        private Crate _currentlyPickingCrate;
        private PlayerController _controller;
        private PlayerManager _playerManager;

        public bool IsPicking
        {
            get { return _currentlyPickingCrate != null; }
        }

        protected override void Awake()
        {
            _controller = this.Get<PlayerController>();
            _playerManager = Scene.FindOfType<PlayerManager>();
        }

        protected override void Update()
        {
            if (_playerManager.CurrentPlayer.GameObject == this.GameObject)
            {
                this.HandleInput();  
            }

            if (this.IsPicking)
            {
                this.UpdateCratePosition();
            }
        }

        private void UpdateCratePosition()
        {
            const float HorizontalDistance = Tile.Size * 1.5f;
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
            RectangleF target = this.collider2D.GetBoundsHack().AsInflated(0, Tile.Size * 0.25f);
            target.Center += _controller.FacingDirection.ToUnitVector() * target.Width;
            var crates = Scene.FindAllOfType<Crate>().ToSet();
            var crate = crates.FirstOrDefault(c => c.collider2D.GetBoundsHack().Intersects(target));

            if (crate != null)
            {
                if (crate.IsPicked)
                {
                    crate.Picker.Get<CratePicker>().ChangeCrateOwner(this);
                }
                else
                {
                    _currentlyPickingCrate = crate;
                    crate.Pick(this.GameObject);
                }
            }

            // draw the pick area
            FlaiDebug.DrawRectangleOutlines(target, ColorF.White, 0.5f);
        }

        public void ChangeCrateOwner(CratePicker other)
        {
            Ensure.True(this.IsPicking);
            Ensure.True(other != this);

            _currentlyPickingCrate.ChangeOwner(other.GameObject);

            other._currentlyPickingCrate = _currentlyPickingCrate;
            _currentlyPickingCrate = null;
        }
    }
}
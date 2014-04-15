using Assets.Scripts.General;
using Assets.Scripts.Objects;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using Flai.Scene;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CratePicker : FlaiScript
    {
        private Crate _currentlyPickingCrate;
        private PlayerController _controller;
        private PlayerManager _playerManager;

        private Vector2f _currentCrateOffset;

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
                this.UpdateCratePosition();
            }
        }

        protected override void LateUpdate()
        {
            if (this.IsPicking)
            {
                this.UpdateCratePosition();
            }
        }

        protected override void FixedUpdate()
        {
            if (this.IsPicking)
            {
                this.UpdateCratePosition();
            }
        }

        private void ResolveAxis(Axis axis, float targetOffsetMultiplier)
        {
            Vector2f targetOffset = this.CalculateDefaultCrateOffset();
            if (FlaiMath.Sign(targetOffset.X) != FlaiMath.Sign(_currentCrateOffset.X))
            {
                _currentCrateOffset.X *= -1;
            }

            if (FlaiMath.Sign(targetOffset.Y) != FlaiMath.Sign(_currentCrateOffset.Y))
            {
                _currentCrateOffset.Y *= -1;
            }

            _currentlyPickingCrate.SetPosition2D(this.Position2D + _currentCrateOffset);
            RectangleF crateArea = _currentlyPickingCrate.collider2D.GetBoundsHack();

            _currentCrateOffset = this.Resolve(crateArea, _currentCrateOffset, axis, targetOffset.GetAxis(axis) * targetOffsetMultiplier);
            _currentlyPickingCrate.SetPosition2D(this.Position2D + _currentCrateOffset);
        }

        private Vector2f Resolve(RectangleF crateArea, Vector2f currentOffset, Axis axis, float targetAmount)
        {
            const float ResolveStep = 0.001f;
            LayerMaskF IgnoreLayerMask = LayerMaskF.FromNames("Crates", "Player", "PlayerHoldingCrate").Inverse;

            float currentAmount = currentOffset.GetAxis(axis);
            if (currentAmount == targetAmount)
            {
                return currentOffset;
            }

            bool isMovingIn = (targetAmount == 0f);
            bool stopWhen = !isMovingIn;

            float changeAmount = targetAmount - currentAmount;
            for (float fraction = 0; fraction <= 1; fraction += ResolveStep)
            {
                RectangleF newArea = crateArea.AsOffsetted(axis.ToUnitVector() * changeAmount * fraction);
                if (Physics2D.OverlapArea(newArea.TopLeft, newArea.BottomRight, IgnoreLayerMask) == stopWhen)
                {
                    return _currentCrateOffset += axis.ToUnitVector() * changeAmount * fraction;
                }
            }

            currentOffset.SetAxis(axis, targetAmount);
            return currentOffset;
        }

        private void UpdateCratePosition()
        {
            if (_currentlyPickingCrate == null)
            {
                return;
            }

            this.ResolveAxis(Axis.Horizontal, 1);
            this.ResolveAxis(Axis.Horizontal, 0);

            this.ResolveAxis(Axis.Vertical, 1);
            this.ResolveAxis(Axis.Vertical, 0);
        }

        private void HandleInput()
        {
            if (FlaiInput.IsNewButtonPress("Pick Crate"))
            {
                if (this.IsPicking)
                {
                    this.Drop();
                }
                else
                {
                    this.TryPickUpCrate();
                }
            }
        }

        private void TryPickUpCrate()
        {
            RectangleF target = this.collider2D.GetBoundsHack().AsInflated(0.3f, Tile.Size * 0.4f);
            target.Center += _controller.FacingDirection.ToUnitVector() * target.Width * 0.5f;
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
                    this.OnIsPickingChanged();
                }
            }

            _currentCrateOffset = this.CalculateDefaultCrateOffset();

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

            this.OnIsPickingChanged();
            other.OnIsPickingChanged();
        }

        public void Drop()
        {
            if (_currentlyPickingCrate != null)
            {
                _currentlyPickingCrate.Drop();
                _currentlyPickingCrate = null;
                this.OnIsPickingChanged();
            }
        }

        private Vector2f CalculateDefaultCrateOffset()
        {
            const float HorizontalDistance = Tile.Size * 1.5f;
            const float VerticalDistance = Tile.Size * 1.65f;

            return _controller.FacingDirection.ToUnitVector() * HorizontalDistance - _controller.GroundDirection.ToUnitVector() * VerticalDistance;
        }

        private void OnIsPickingChanged()
        {
            this.LayerName = this.IsPicking ? "PlayerHoldingCrate" : "Player";
        }
    }
}
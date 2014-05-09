// ReSharper disable ConvertConditionalTernaryToNullCoalescing
using Assets.Scripts.General;
using Assets.Scripts.Objects.Button;
using Flai;
using Flai.Inspector;
using Flai.Scripts;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [RequireComponent(typeof(OnTopMover))]
    public class ButtonPresser : FlaiScript, IButtonState
    {
        private float _positionOffset = 0f;
        private Vector2f _initialPosition = new Vector2f(0, 0.3f);
        private OnTopMover _onTopMover;

        [ShowInInspector]
        public bool IsPressed
        {
            get { return this.IsPressing && _positionOffset >= this.Height; }
        }

        protected bool IsPressing
        { 
            get { return this.OnTopMover.HasAny; }
        }

        protected OnTopMover OnTopMover
        {
            get { return _onTopMover != null ? _onTopMover : (_onTopMover = this.Get<OnTopMover>()); }
        }

        protected float Height  
        {
            get { return this.BoxCollider2D.size.y * this.Scale2D.Y * Tile.Size; }
        }

        protected override void Awake()
        {
         // _initialPosition = this.LocalPosition2D;
         //   this.OnTopMover.AllowedDirection = DirectionHelper.FromRotation(this.Rotation2D, Direction2D.Up).Opposite();
        }

        protected override void Update()
        {
            this.UpdateButtonPressing();
        }

        private void UpdateButtonPressing()
        {
            float previous = _positionOffset;
            if (this.IsPressing && _positionOffset < this.Height)
            {
                _positionOffset = FlaiMath.Min(_positionOffset + this.Height * Time.deltaTime * 4, this.Height);
            }
            else if (!this.IsPressing && _positionOffset > 0)
            {
                _positionOffset = FlaiMath.Max(_positionOffset - this.Height * Time.deltaTime * 4, 0);
            }

            float change = _positionOffset - previous;
            if (change != 0)
            {
                this.LocalPosition2D = _initialPosition - Vector2f.UnitY * _positionOffset;
            }

            _onTopMover.ForceUpdate();
        }
    }
}
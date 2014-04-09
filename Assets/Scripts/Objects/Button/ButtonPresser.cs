using Flai.Scripts;
// ReSharper disable ConvertConditionalTernaryToNullCoalescing
using Assets.Scripts.General;
using Flai;
using UnityEngine;

namespace Assets.Scripts.Objects.Button
{
    [RequireComponent(typeof(OnTopMover))]
    public class ButtonPresser : FlaiScript
    {
        private float _height;
        private float _positionOffset;
        private Vector2f _initialPosition;
        private OnTopMover _onTopMover;

        public bool IsPressed
        {
            get { return this.IsPressing && _positionOffset >= _height; }
        }

        protected bool IsPressing
        {
            get { return this.OnTopMover.HasAny; }
        }

        protected OnTopMover OnTopMover
        {
            get { return _onTopMover != null ? _onTopMover : (_onTopMover = this.Get<OnTopMover>()); }
        }

        protected override void Awake()
        {
            _height = this.Get<BoxCollider2D>().size.y * this.Scale2D.Y * Tile.Size;
            _initialPosition = this.LocalPosition2D;
        }

        protected override void Update()
        {
            this.UpdateButtonPressing();
        }

        private void UpdateButtonPressing()
        {
            float previous = _positionOffset;
            if (this.IsPressing && _positionOffset < _height)
            {
                _positionOffset = FlaiMath.Min(_positionOffset + _height * Time.deltaTime * 4, _height);
            }
            else if (!this.IsPressing && _positionOffset > 0)
            {
                _positionOffset = FlaiMath.Max(_positionOffset - _height * Time.deltaTime * 4, 0);
            }

            float change = _positionOffset - previous;
            if (change != 0)
            {
                this.LocalPosition2D = _initialPosition - Vector2f.UnitY * _positionOffset;
            }
        }
    }
}
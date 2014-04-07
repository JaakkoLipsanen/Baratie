using Flai;
using Flai.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class ButtonPresser : FlaiScript
    {
        private bool _isPressed = false;
        private float _height;
        private float _positionOffset;
        private Vector2f _initialPosition;
        private GameObject _pressingGameObject;

        public bool IsPressed
        {
            get { return _isPressed && _positionOffset >= _height; }
        }

        protected override void Awake()
        {
            _height = this.Get<BoxCollider2D>().size.y * this.Scale2D.Y;
            _initialPosition = this.LocalPosition2D;
        }

        protected override void Update()
        {
            this.UpdateButtonPressing();
        }

        private void UpdateButtonPressing()
        {
            float previous = _positionOffset;
            if (_isPressed && _positionOffset < _height)
            {
                _positionOffset = FlaiMath.Min(_positionOffset + _height * Time.deltaTime * 4, _height);
            }
            else if (!_isPressed && _positionOffset > 0)
            {
                _positionOffset = FlaiMath.Max(_positionOffset - _height * Time.deltaTime * 4, 0);
            }

            float change = _positionOffset - previous;
            if (change != 0)
            {
                this.LocalPosition2D = _initialPosition - Vector2f.UnitY * _positionOffset;
                if (_pressingGameObject != null)
                {
                    _pressingGameObject.SetPosition2D(_pressingGameObject.GetPosition2D() - Vector2f.UnitY * change);
                }
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Any(contact => contact.normal == Vector2f.Down))
            {
                _isPressed = true;
                _pressingGameObject = collision.gameObject;
                FlaiDebug.LogWithTypeTag<ButtonPresser>("Button Pressed!");
            }
        }

        protected override void OnCollisionExit2D(Collision2D collision)
        {
            if (_isPressed)
            {
                FlaiDebug.LogWithTypeTag<ButtonPresser>("Button Un-pressed!");
                _pressingGameObject = null;
                _isPressed = false;
            }
        }
    }
}
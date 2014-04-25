using Flai;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class GravitySwitchArea : FlaiScript
    {
        private GameObject _currentGameObjectInArea;
        private bool _isOn = false;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOn && _currentGameObjectInArea == null)
            {
           //     _isOn = false;
            }

            if (_currentGameObjectInArea != null)
            {
                return;
            }

            Physics2D.gravity *= - 1;
            _currentGameObjectInArea = other.gameObject;
            _currentGameObjectInArea.rigidbody2D.gravityScale *= -1;
            _isOn = true;
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (_isOn && _currentGameObjectInArea == other.gameObject)
            {
                Physics2D.gravity *= -1;
                _currentGameObjectInArea.rigidbody2D.gravityScale *= -1;
                _currentGameObjectInArea = null;
                _isOn = false;
            }
        }

        protected override void LateUpdate()
        {
            if (_isOn && (_currentGameObjectInArea == null || !PhysicsHelper.Intersects(this.collider2D, _currentGameObjectInArea.collider2D, 0.1f)))
            {
                Physics2D.gravity *= -1;
                if (_currentGameObjectInArea != null)
                {
                    _currentGameObjectInArea.rigidbody2D.gravityScale *= -1;
                }

                _currentGameObjectInArea = null;
                _isOn = false;
            }
        }
    }
}
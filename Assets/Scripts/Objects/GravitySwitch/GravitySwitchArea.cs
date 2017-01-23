using Assets.Scripts.General;
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
            Debug.Log(other);
            if (other == null || other.gameObject.GetLayerName() == "Button")
            {
                return; 
            }

            if (_isOn && _currentGameObjectInArea == null)
            {
           //     _isOn = false;
            }

            if (_currentGameObjectInArea != null)
            {
                return;
            }

            Physics2D.gravity *= -1; _currentGameObjectInArea = other.gameObject;
            var gravityState = _currentGameObjectInArea.Get<GravityState>();
            if (gravityState != null)
            {
                gravityState.FlipGravityDirection();
            }


            _isOn = true;
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (_isOn && _currentGameObjectInArea == other.gameObject)
            {
                Physics2D.gravity *= -1;
                _currentGameObjectInArea.Get<GravityState>().FlipGravityDirection();
                _currentGameObjectInArea = null;
                _isOn = false;
            }
        }

        protected override void LateUpdate()
        {
            if (_isOn && (_currentGameObjectInArea == null || !PhysicsHelper.Intersects(this.GetComponent<Collider2D>(), _currentGameObjectInArea.GetComponent<Collider2D>(), 0.1f)))
            {
                Physics2D.gravity *= -1;
                if (_currentGameObjectInArea != null)
                {
                    _currentGameObjectInArea.Get<GravityState>().FlipGravityDirection();
                }

                _currentGameObjectInArea = null;
                _isOn = false;
            }
        }
    }
}
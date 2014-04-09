using Flai;
using System.Collections.Generic;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Objects.GravitySwitch
{
    public class GravitySwitchArea : FlaiScript
    {
        private readonly HashSet<GameObject> _gameObjectsInArea = new HashSet<GameObject>();
        private readonly HashSet<GameObject> _gameObjectsToRemove = new HashSet<GameObject>(); 
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_gameObjectsInArea.Contains(other.gameObject) || _gameObjectsInArea.Count > 0)
            {
                return;
            }

            Physics2D.gravity *= - 1; // Vector2f.Abs(Physics2D.gravity);
            other.rigidbody2D.gravityScale *= -1; // -FlaiMath.Abs(other.rigidbody2D.gravityScale);

            _gameObjectsInArea.Add(other.gameObject);
            FlaiDebug.Log("E");
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (!_gameObjectsInArea.Contains(other.gameObject))
            {
                return;
            }

            Physics2D.gravity *= -1; // -Vector2f.Abs(Physics2D.gravity);
            other.rigidbody2D.gravityScale *= -1; // FlaiMath.Abs(other.rigidbody2D.gravityScale);

            _gameObjectsInArea.Remove(other.gameObject);
            FlaiDebug.Log("Ex");
        }

        protected override void LateUpdate()
        {
            foreach (GameObject go in _gameObjectsInArea)
            {
                // removed from scene, probably because of a split
                if (go == null)
                {
                    _gameObjectsToRemove.Add(go);
                    Physics2D.gravity *= -1;
                    FlaiDebug.Log("LU");
                }
            }

            _gameObjectsInArea.RemoveWhere(go => _gameObjectsToRemove.Contains(go));
            _gameObjectsToRemove.Clear();
        }
    }
}
using Flai;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class GravitySwitchArea : FlaiScript
    {
        private readonly HashSet<GameObject> _gameObjectsInArea = new HashSet<GameObject>();
        private readonly HashSet<GameObject> _gameObjectsToRemove = new HashSet<GameObject>(); 
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_gameObjectsInArea.Contains(other.gameObject) || _gameObjectsInArea.Count > 0) // allow only one gameobject (these hashsets are useless now :P)
            {
                return;
            }

            Physics2D.gravity *= - 1;
            other.rigidbody2D.gravityScale *= -1;

            _gameObjectsInArea.Add(other.gameObject);
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (!_gameObjectsInArea.Contains(other.gameObject))
            {
                return;
            }

            Physics2D.gravity *= -1;
            other.rigidbody2D.gravityScale *= -1;

            _gameObjectsInArea.Remove(other.gameObject);
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
                }
            }

            _gameObjectsInArea.RemoveWhere(go => _gameObjectsToRemove.Contains(go));
            _gameObjectsToRemove.Clear();
        }
    }
}
using Flai;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
    // todo: awful name, name it better
    public class OnTopMover : FlaiScript
    {
        private HashSet<GameObject> _gameObjectsOnTop = new HashSet<GameObject>();

        private float _previousScaleY;
        private float _previousPositionY;

        private float _scaleMultiplier = 1f;

        public bool HasAny
        {
            get { return _gameObjectsOnTop.Count > 0; }
        }

        protected override void Awake()
        {
            _previousScaleY = this.Scale2D.Y;
            _previousPositionY = this.Position2D.Y;

            var boxCollider = this.Get<BoxCollider2D>();
            if (boxCollider != null)
            {
                _scaleMultiplier = boxCollider.size.y;
            }
        }

        protected override void Update()
        {
            float changeInUnitsScale = (this.Scale2D.Y - _previousScaleY) * _scaleMultiplier;
            float changeInUnitsPosition = (this.Position2D.Y - _previousPositionY);
            float changeInUnits = changeInUnitsScale + changeInUnitsPosition;

            _gameObjectsOnTop.RemoveWhere(go => go == null);
            foreach (GameObject other in _gameObjectsOnTop)
            {
                other.SetPosition2D(other.GetPosition2D() + Vector2f.UnitY * changeInUnits);
            }

            _previousScaleY = this.Scale.y;
            _previousPositionY = this.Position2D.Y;
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Any(contact => contact.normal == Vector2f.Down))
            {
                _gameObjectsOnTop.Add(collision.gameObject);
            }
        }

        protected override void OnCollisionExit2D(Collision2D collision)
        {
            _gameObjectsOnTop.Remove(collision.gameObject);
        }
    }
}
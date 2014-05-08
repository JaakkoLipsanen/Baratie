using Flai;
using UnityEngine;

namespace Assets.Scripts.General
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityState : FlaiScript
    {
        private float _initialGravityScale;
        private VerticalDirection _gravityDirection = VerticalDirection.Down;
        private bool _useGravity;

        protected override void Awake()
        {
            _initialGravityScale = this.rigidbody2D.gravityScale;
        }

        public VerticalDirection GravityDirection
        {
            get { return _gravityDirection; }
            set
            {
                _gravityDirection = value;
                this.CalculateGravityScale();
            }
        }

        public bool UseGravity
        {
            get { return _useGravity; }
            set
            {
                _useGravity = value;
                this.CalculateGravityScale();
            }
        }

        private void CalculateGravityScale()
        {
            if (!_useGravity)
            {
                this.rigidbody2D.gravityScale = 0;
                return;
            }

            this.rigidbody2D.gravityScale = _initialGravityScale * _gravityDirection.ToInt();
        }
    }
}

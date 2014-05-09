using Flai;
using Flai.Inspector;
using UnityEngine;

namespace Assets.Scripts.General
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityState : FlaiScript
    {
        [SerializeField]
        [HideInInspector]
        private VerticalDirection _gravityDirection = VerticalDirection.Down;

        [SerializeField]
        [HideInInspector]
        private bool _useGravity = true;

        [SerializeField]
        [HideInInspector]
        private float _gravityScale = 1;

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public VerticalDirection GravityDirection
        {
            get { return _gravityDirection; }
            set
            {
                _gravityDirection = value;
                this.CalculateGravityScale();
            }
        }

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public bool UseGravity
        {
            get { return _useGravity; }
            set
            {
                _useGravity = value;
                this.CalculateGravityScale();
            }
        }

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public float GravityScale
        {
            get { return _gravityScale; }
            set
            {
                _gravityScale = value;
                this.CalculateGravityScale();
            }
        }

        [ShowInInspector]
        private void CalculateGravityScale()
        {
            if (!_useGravity)
            {
                this.rigidbody2D.gravityScale = 0;
                return;
            }

            this.rigidbody2D.gravityScale = _gravityScale * _gravityDirection.Opposite().ToInt(); // opposite because VerticalDirection.Down is -1
        }

        [ShowInInspector(IsReadOnly = true)]
        public object Tag { get; set; } // meh... 
        public void FlipGravityDirection()
        {
            this.GravityDirection = this.GravityDirection.Opposite();
        }
    }
}

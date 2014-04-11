using Flai;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Crate : FlaiScript
    {
        private GameObject _picker;
        public bool IsPicked
        {
            get { return _picker != null; }
        }

        public GameObject Picker
        {
            get { return _picker; }
        }

        protected override void Awake()
        {
        }

        protected override void Update()
        {
            if (!this.IsPicked)
            {
                // when not picked, reduce the x axis
                this.rigidbody2D.velocity *= Vector2f.UnitY;
            }
        }

        public void Pick(GameObject picker)
        {
            if (_picker != null)
            {
                FlaiDebug.LogErrorWithTypeTag<Crate>("Crate is already picked, can't be picked again");
                return;
            }

            if (picker != null)
            {
                _picker = picker;
                this.rigidbody2D.isKinematic = true;
                FlaiDebug.LogWithTypeTag<Crate>("Crate Picked");
            }
        }

        public void Drop()
        {
            if (_picker == null)
            {
                FlaiDebug.LogErrorWithTypeTag<Crate>("Crate isn't picked, can't be dropped");
                return;
            }

            if (_picker.Has<Rigidbody2D>())
            {
                this.rigidbody2D.velocity = Vector2f.UnitY * _picker.rigidbody2D.velocity;
            }

            _picker = null;
            this.rigidbody2D.isKinematic = false;
            FlaiDebug.LogWithTypeTag<Crate>("Crate Dropped");
        }

        public void ChangeOwner(GameObject newOwner)
        {
            _picker = newOwner;
            FlaiDebug.LogWithTypeTag<Crate>("Crate owner changed");
        }
    }
}
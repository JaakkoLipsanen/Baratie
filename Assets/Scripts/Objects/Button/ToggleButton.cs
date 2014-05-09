using Flai;
using Flai.Diagnostics;
using Flai.Inspector;
using UnityEngine;

namespace Assets.Scripts.Objects.Button
{
    public class ToggleButton : FlaiScript, IButtonState
    {
        private float _initialX;
        private float _amountMovedX = 0;
        public bool IsPressed { get; private set; }

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public HorizontalDirection PushDirection
        {
            get
            {
                if (this.LocalRotation2D == 0)
                {
                    return HorizontalDirection.Right;
                }

                return HorizontalDirection.Left;
            }
            set
            {
                this.LocalRotation2D = (value == HorizontalDirection.Right) ? 0 : 180;
            }
        }

        private float TargetAmountX
        {
            get { return this.Scale2D.X * 0.6f; }
        }

        protected override void Awake()
        {
            _initialX = this.Position2D.X;
        }

        protected override void Update()
        {
            if (this.IsPressed && _amountMovedX < this.TargetAmountX)
            {
                _amountMovedX = FlaiMath.Clamp(_amountMovedX + Time.deltaTime * 2, 0, this.TargetAmountX);
            }
            else if (!this.IsPressed && _amountMovedX > 0)
            {
                _amountMovedX = FlaiMath.Clamp(_amountMovedX - Time.deltaTime * 2, 0, this.TargetAmountX);
            }

            this.Transform.SetPositionX(_initialX + _amountMovedX * this.PushDirection.Opposite().ToInt());
        }

        public void Toggle()
        {
            FlaiDebug.Log("toggle");
            this.IsPressed = !this.IsPressed;
        }
    }
}

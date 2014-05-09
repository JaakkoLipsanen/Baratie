// ReSharper disable ConvertConditionalTernaryToNullCoalescing
using Flai;
using Flai.Diagnostics;
using Flai.Scripts.Responses;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class PillarHandler : Response
    {
        private const float DefaultOffScale = 0.6f;

        private GameObject _pillarGameObject;
        private float _defaultScale;
        private bool _isExecutingOn = false;
        private float _scaleVelocity = 0f;

        public float TargetScale = 5;
        public bool IsOnByDefault = false;

        protected new float Scale
        {
            get { return this.PillarGameObject.GetScale2D().Y; }
            set { this.PillarGameObject.SetScale2D(new Vector2f(this.PillarGameObject.GetScale2D().X, value)); }
        }

        protected GameObject PillarGameObject
        {
            get
            {
                return _pillarGameObject == null ? (_pillarGameObject = this.GetChild("PillarTop")) : _pillarGameObject;
            }
        }

        public bool IsOn
        {
            get { return this.IsOnByDefault ^ _isExecutingOn; }
        }

        protected override void Start()
        {
            _defaultScale = this.IsOnByDefault ? DefaultOffScale : this.PillarGameObject.GetScale2D().Y;
            if (this.IsOnByDefault)
            {
                this.Scale = this.TargetScale;
            }
        }

        protected override void Update()
        {
            if (this.IsOn && this.Scale < this.TargetScale)
            {
                this.Scale = FlaiMath.Min(this.Scale + _scaleVelocity * Time.deltaTime, this.TargetScale);
            }
            else if (!this.IsOn && this.Scale > _defaultScale)
            {
                this.Scale = FlaiMath.Max(this.Scale + _scaleVelocity * Time.deltaTime, _defaultScale);
            }
            else
            {
                _scaleVelocity = 0;
                return;
            }

            int multiplier = (_scaleVelocity < 0 && this.IsOn ? 4 : 1);
            if (_scaleVelocity > 0 && !this.IsOn)
            {
                multiplier = 4;
            }

            _scaleVelocity += Time.deltaTime * 8 * (this.IsOn ? 1 : -1) * multiplier;
            if (_scaleVelocity < 0 && this.Scale < DefaultOffScale)
            {
                _scaleVelocity = 0;
                this.Scale = DefaultOffScale;
            }
            else if (_scaleVelocity > 0 && this.Scale > this.TargetScale)
            {
                _scaleVelocity = 0;
                this.Scale = this.TargetScale;
            }

         // this.Get<OnTopMover>().ForceUpdate(); // first of all, the OnTopMover is in a child. second of all, this isn't probably needesd
        }

        protected override bool ExecuteOnInner()
        {
            FlaiDebug.LogWithTypeTag<PillarHandler>("On");
            _isExecutingOn = true;
            return true;
        }

        protected override bool ExecuteOffInner()
        {
            FlaiDebug.LogWithTypeTag<PillarHandler>("Off");
            _isExecutingOn = false;
            return true;
        }

        public void RefreshFromInspector()
        {
            this.Scale = this.IsOnByDefault ? this.TargetScale : DefaultOffScale;
        }
    }
}
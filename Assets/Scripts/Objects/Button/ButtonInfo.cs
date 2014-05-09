using Assets.Scripts.Objects.Button;
using Flai;
using Flai.Diagnostics;
using Flai.Scripts.Responses;
using System.Collections.Generic;

namespace Assets.Scripts.Objects
{
    public class ButtonInfo : FlaiScript
    {
        private bool _wasPressed = false;
        private IButtonState _buttonState;
        public List<Response> Responses = new List<Response>();

        public bool HasResponses
        {
            get { return this.Responses != null && this.Responses.Count > 0; }
        }

        public IButtonState ButtonState
        {
            get { return _buttonState ?? (_buttonState = (IButtonState) this.GetComponentInChildren(typeof (IButtonState))); }
        }

        protected override void Start()
        {
            _wasPressed = this.ButtonState.IsPressed;
        }

        protected override void LateUpdate()
        {
            if (_wasPressed != this.ButtonState.IsPressed)
            {
                foreach (Response response in this.Responses)
                {
                    if (response == null)
                    {
                        continue;
                    }

                    // toggle if possible
                    if (response.IsToggleable)
                    {
                        response.ExecuteToggle();
                    }
                    else
                    {
                        response.Execute(this.ButtonState.IsPressed);
                    }
                }

                _wasPressed = this.ButtonState.IsPressed;
                if (!this.HasResponses)
                {
                    FlaiDebug.LogWarningWithTypeTag<ButtonInfo>("Button doesn't have any responses!", this);
                }
            }
        }
    }
}

using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;

namespace Assets.Scripts.Objects
{
    public class ButtonInfo : FlaiScript
    {
        private bool _wasPressed = false;
        private ButtonPresser _buttonPresser;

        public Response Response;

        protected override void Awake()
        {
            _buttonPresser = this.GetComponentInChildren<ButtonPresser>();
        }

        protected override void Start()
        {
            _wasPressed = _buttonPresser.IsPressed;
        }

        protected override void LateUpdate()
        {
            if (_wasPressed != _buttonPresser.IsPressed)
            {
                if (_buttonPresser.IsPressed)
                {
                    _wasPressed = true;
                    if (this.Response != null)
                    {
                        this.Response.ExecuteOn(null);
                    }
                }
                else
                {
                    _wasPressed = false;
                    if (this.Response != null)
                    {
                        this.Response.ExecuteOff(null);
                    }
                }

                if (this.Response == null)
                {
                    FlaiDebug.LogWarningWithTypeTag<ButtonInfo>("Button doesn't have a response!", this);
                }
            }
        }
    }
}

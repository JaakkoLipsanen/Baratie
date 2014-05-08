﻿using Flai;
using Flai.Diagnostics;
using Flai.Scripts.Responses;
using System.Collections.Generic;

namespace Assets.Scripts.Objects
{
    public class ButtonInfo : FlaiScript
    {
        private bool _wasPressed = false;
        private ButtonPresser _buttonPresser;
        public List<Response> Responses = new List<Response>();

        public bool HasResponses
        {
            get { return this.Responses != null && this.Responses.Count > 0; }
        }

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
                    if (this.HasResponses)
                    {
                        foreach (Response response in this.Responses)
                        {
                            response.Execute();
                        }
                    }
                }
                else
                {
                    _wasPressed = false;
                    if (this.HasResponses)
                    {
                        foreach (Response response in this.Responses)
                        {
                            response.ExecuteOff();
                        }
                    }
                }

                if (!this.HasResponses)
                {
                    FlaiDebug.LogWarningWithTypeTag<ButtonInfo>("Button doesn't have any responses!", this);
                }
            }
        }
    }
}

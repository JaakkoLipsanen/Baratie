using Flai.Scripts.Responses;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [RequireComponent(typeof(Funnel))]
    public class FunnelToggleIsOnResponse : Response
    {
        private Funnel _funnel;
        private bool _wasOnByDefault = false;
        protected override void Awake()
        {
            _funnel = this.Get<Funnel>();
            _wasOnByDefault = _funnel.IsOn;
        }

        protected override bool ExecuteOnInner()
        {
            _funnel.IsOn = !_wasOnByDefault;
            return true;
        }

        protected override bool ExecuteOffInner()
        {
            _funnel.IsOn = _wasOnByDefault;
            return true;
        }

        protected override bool ExecuteToggleInner()
        {
            _funnel.IsOn = !_funnel.IsOn;
            return true;
        }
    }
}
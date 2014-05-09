using Flai.Scripts.Responses;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [RequireComponent(typeof(Funnel))]
    public class FunnelToggleIsReversedResponse : Response
    {
        private Funnel _funnel;
        private bool _wasReversedByDefault = false;
        protected override void Awake()
        {
            _funnel = this.Get<Funnel>();
            _wasReversedByDefault = _funnel.IsReversed;
        }

        protected override bool ExecuteOnInner()
        {
            _funnel.IsReversed = !_wasReversedByDefault;
            return true;
        }

        protected override bool ExecuteOffInner()
        {
            _funnel.IsReversed = _wasReversedByDefault;
            return true;
        }

        protected override bool ExecuteToggleInner()
        {
            _funnel.IsReversed = !_funnel.IsOn;
            return true;
        }
    }
}

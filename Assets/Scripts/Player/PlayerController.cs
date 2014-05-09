using Assets.Scripts.General;
using Flai;
using Flai.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(GravityState))]
    public class PlayerController : CharacterController2D
    {
        private GravityState _gravityState;
        public override VerticalDirection GroundDirection
        {
            get { return (VerticalDirection)((int)_gravityState.GravityDirection * FlaiMath.Sign(-Physics2D.gravity.y)); }
        }

        protected override void Awake()
        {
            _gravityState = this.Get<GravityState>();
            base.Awake();
        }

        protected override float CalculateHorizontalSpeedDrag()
        {
            return _gravityState.UseGravity ? base.CalculateHorizontalSpeedDrag() : 1;
        }
    }
}

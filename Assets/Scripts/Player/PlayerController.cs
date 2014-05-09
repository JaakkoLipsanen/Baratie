using Assets.Scripts.General;
using Assets.Scripts.Objects;
using Flai;
using Flai.Input;
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

        protected override void Update()
        {
            base.Update();
            var funnel = _gravityState.Tag as Funnel;
            if (funnel != null && funnel.IsInFunnel(this.GameObject))
            {
                if (funnel.Direction.ToAxis() == Axis.Horizontal)
                {
                    if (FlaiInput.IsButtonOrKeyPressed("Down", KeyCode.S))
                    {
                        this.rigidbody2D.velocity -= Vector2f.UnitY.ToVector2() * this.Speed * 5f * Time.deltaTime;
                    }
                }
            }
        }

        protected override float CalculateHorizontalForce()
        {
            float baseForce = base.CalculateHorizontalForce();
            var funnel = _gravityState.Tag as Funnel;
            if (funnel != null && funnel.IsInFunnel(this.GameObject))
            {
                if (funnel.Direction == Direction2D.Left && baseForce > 0)
                {
                    baseForce = 0;
                }
                else if (funnel.Direction == Direction2D.Right && baseForce < 0)
                {
                    baseForce = 0;
                }

                baseForce *= 0.5f;
            }

            return baseForce;
        }

        protected override float CalculateHorizontalSpeedDrag()
        {
            return _gravityState.UseGravity ? base.CalculateHorizontalSpeedDrag() : 1;
        }
    }
}

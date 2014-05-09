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
            get
            {
                VerticalDirection direction = VerticalDirection.Down;
                if (_gravityState != null)
                {
                    direction = _gravityState.GravityDirection;
                }

                if (Physics2D.gravity.y > 0)
                {
                    direction = direction.Opposite();
                }

                return direction;
            }
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
                if (funnel.Direction.ToAxis() != Axis.Horizontal)
                {
                    return;
                }

                const float FunnelSpeedMultiplier = 5;
                if (_gravityState.RealGravityDirection == VerticalDirection.Down)
                {
                    if (FlaiInput.IsButtonOrKeyPressed("Down", KeyCode.S))
                    {
                        this.rigidbody2D.velocity -= Vector2f.UnitY.ToVector2() * this.Speed * FunnelSpeedMultiplier * Time.deltaTime;
                    }
                }
                else if (_gravityState.RealGravityDirection == VerticalDirection.Up)
                {
                    if (FlaiInput.IsButtonOrKeyPressed("Up", KeyCode.W))
                    {
                        this.rigidbody2D.velocity += Vector2f.UnitY.ToVector2() * this.Speed * FunnelSpeedMultiplier * Time.deltaTime;
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
                if (funnel.CurrentDirection == Direction2D.Left && baseForce > 0)
                {
                    baseForce = 0;
                }
                else if (funnel.CurrentDirection == Direction2D.Right && baseForce < 0)
                {
                    baseForce = 0;
                }

                baseForce *= 0.25f;
            }

            return baseForce;
        }

        protected override float CalculateHorizontalSpeedDrag()
        {
            return _gravityState.UseGravity ? base.CalculateHorizontalSpeedDrag() : 1;
        }
    }
}

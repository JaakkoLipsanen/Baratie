using Flai;
using Flai.Input;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : FlaiScript
    {
        private float _timeSinceNotInGround = 0f;
        private bool _isJumping = false;

        public bool ShowDebug = false;
        public bool IsControllingEnabled = true;

        public float HorizontalSpeedDrag = 0.9f; // meh name
        public float AccelerationPower = 20;
        public float SpeedAirDrag = 0.5f;
        public float Speed = 10;
        public float JumpForce = 750;
        public float JumpTimeBias = 0.1f;

        public bool IsOnGround
        {
            get
            {
                BoxCollider2D collider = (BoxCollider2D)this.collider2D;
                Vector2f center = this.Position2D - Vector2f.UnitY * (collider.size.y / 2f + 0.01f);
                Vector2f left = center - Vector2f.UnitX * collider.size / 2f;
                Vector2f right = center + Vector2f.UnitX * collider.size / 2f;

                const float MaxDistance = 0.01f;
                return Physics2D.Raycast(center, Vector2f.Down, MaxDistance) || Physics2D.Raycast(left, Vector2f.Down, MaxDistance) || Physics2D.Raycast(right, Vector2f.Down, MaxDistance);

                //if (this.ShowDebug)
                //{
                //    FlaiDebug.DrawLine(center, raycastHit.point, BaratieConstants.DebugColor);
                //}
            }
        }

        public bool CanJump
        {
            get { return this.IsOnGround || (!_isJumping && _timeSinceNotInGround <= this.JumpTimeBias); }
        }

        protected override void Update()
        {
            if (this.IsOnGround)
            {
                _isJumping = false;
                _timeSinceNotInGround = 0;
            }
            else
            {
                _timeSinceNotInGround += Time.deltaTime;
            }

            this.Control();
        }

        private void Control()
        {
            float force = 0;
            if (this.IsControllingEnabled && FlaiInput.IsKeyPressed(KeyCode.A))
            {
                force -= this.Speed;
            }

            if (this.IsControllingEnabled && FlaiInput.IsKeyPressed(KeyCode.D))
            {
                force += this.Speed;
            }

            force *= this.IsOnGround ? 1 : this.SpeedAirDrag;
            rigidbody2D.velocity += Vector2f.UnitX.ToVector2() * force * Time.deltaTime * AccelerationPower;
            rigidbody2D.velocity = Vector2f.ClampX(rigidbody2D.velocity, -10, 10);
            if (this.CanJump && this.IsControllingEnabled)
            {
                if (FlaiInput.IsNewKeyPress(KeyCode.Space))
                {
                    rigidbody2D.AddForce(Vector2f.Up * this.JumpForce);
                    _isJumping = true;
                }
            }

            rigidbody2D.velocity *= new Vector2f(this.HorizontalSpeedDrag, 1);
        }
    }
}
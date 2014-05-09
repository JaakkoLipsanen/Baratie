using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using System.Collections.Generic;
using Flai.Inspector;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Funnel : FlaiScript
    {
        private HashSet<GameObject> _gameObjectsOnFunnel = new HashSet<GameObject>();

        public float Speed = 4f;
        public float Thickness = 2f; // in units
        public bool IsReversed = false;
        public bool IsOn = true;

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public Direction2D Direction
        {
            get
            {
                var direction = DirectionHelper.FromRotation(this.Rotation2D);
                this.Rotation2D = direction.ToDegrees(); // make sure that rotation is always correct (inspector calls this getter)
                return direction;
            }
            set { this.Rotation2D = value.ToDegrees(); }
        }

        [ShowInInspector]
        public Direction2D CurrentDirection
        {
            get { return this.IsReversed ? this.Direction.Opposite() : this.Direction; }
        }

        public bool IsInFunnel(GameObject go)
        {
            return _gameObjectsOnFunnel.Contains(go);
        }

        protected override void Update()
        {
            this.renderer.enabled = this.IsOn;
        }

        protected override void LateUpdate()
        {
            this.UpdateScale();
            this.MoveObjects();
        }

        private void UpdateScale()
        {
            float distance = this.CalculateHitDistance();
            this.Transform.SetScaleX(1 / this.SpriteRenderer.sprite.textureRect.width * distance);
            this.Transform.SetScaleY(1 / this.SpriteRenderer.sprite.textureRect.height * this.Thickness);
        }

        private void MoveObjects()
        {
            if (!this.IsOn)
            {
                this.RemoveAll();
                return;
            }

            this.RemoveGameObjectsOutside();
            foreach (GameObject go in _gameObjectsOnFunnel)
            {
                if (this.Direction == Direction2D.Up || this.Direction == Direction2D.Down)
                {
                    var targetVelocity = new Vector2((this.Position2D.X - go.GetPosition2D().X) * this.Speed * 4f, this.Direction.ToUnitVector().Y * this.Speed * (this.IsReversed ? -1 : 1));
                    go.rigidbody2D.velocity = Vector2f.Lerp(go.rigidbody2D.velocity, targetVelocity, Time.deltaTime * 2);
                }
                else // Left or Right
                {

                    var targetVelocity = new Vector2(this.Direction.ToUnitVector().X * this.Speed * (this.IsReversed ? -1 : 1), (this.Position2D.Y - go.GetPosition2D().Y) * this.Speed * 4f);
                    go.rigidbody2D.velocity = Vector2f.Lerp(go.rigidbody2D.velocity, targetVelocity, Time.deltaTime * 2);
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            var gravityState = other.TryGet<GravityState>();
            if (gravityState != null)
            {
                _gameObjectsOnFunnel.Add(gravityState.GameObject);
                gravityState.UseGravity = false;
                gravityState.Tag = this;
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (_gameObjectsOnFunnel.Remove(other.gameObject))
            {
                this.OnRemoveGameObject(other.gameObject);
            }
        }

        private void RemoveAll()
        {
            foreach (GameObject go in _gameObjectsOnFunnel)
            {
                if (go != null)
                {
                    this.OnRemoveGameObject(go);
                }
            }

            _gameObjectsOnFunnel.Clear();
        }

        private void RemoveGameObjectsOutside()
        {
            _gameObjectsOnFunnel.RemoveWhere(go =>
            {
                bool remove = (go == null) || !PhysicsHelper.Intersects(go.collider2D, this.collider2D);
                if (remove && go != null)
                {
                    this.OnRemoveGameObject(go);
                }

                return remove;
            });
        }

        private float CalculateHitDistance()
        {
            const float PositionBias = 0.01f;
            LayerMaskF layerMask = LayerMaskF.FromNames("Funnel", "Crates", "Player", "PlayerHoldingCrate", "Keys").Inverse;
            RaycastHit2D hit = Physics2D.Raycast(this.Position2D + this.RotationDirection2D * PositionBias, this.RotationDirection2D, float.PositiveInfinity, layerMask);
            if (!hit)
            {
                FlaiDebug.LogErrorWithTypeTag<Funnel>("No raycast hit");
                return 0f;
            }

            return hit.fraction + PositionBias;
        }

        [ShowInInspector]
        private void Flip()
        {
            float distance = this.CalculateHitDistance();
            this.Position2D += this.Direction.ToUnitVector() * distance;
            this.Direction = this.Direction.Opposite();
        }

        private void OnRemoveGameObject(GameObject go)
        {
            var gravityState = go.TryGet<GravityState>();
            gravityState.UseGravity = true;
            if (gravityState.Tag == this)
            {
                gravityState.Tag = null;
            }
        }
    }
}
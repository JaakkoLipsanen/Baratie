using System.Collections.Generic;
using Flai;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class Funnel : FlaiScript
    {
        private HashSet<GameObject> _gameObjectsOnFunnel = new HashSet<GameObject>();

        public float Speed = 2f;
        public float Thickness = 1f; // in units
        public bool IsReversed = false;
        public bool IsOn = true;

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
            const float PositionBias = 0.01f;
            LayerMaskF layerMask = LayerMaskF.FromNames("Funnel", "Crates", "Player", "PlayerHoldingCrate", "Keys").Inverse;
            RaycastHit2D hit = Physics2D.Raycast(this.Position2D + this.RotationDirection2D * PositionBias, this.RotationDirection2D, float.PositiveInfinity, layerMask);
            if (!hit)
            {
                FlaiDebug.LogErrorWithTypeTag<Funnel>("No raycast hit");
                return;
            }

            float distance = hit.fraction + PositionBias;
            this.Transform.SetScaleX(1 / this.SpriteRenderer.sprite.textureRect.width * distance);
            this.Transform.SetScaleY(1 / this.SpriteRenderer.sprite.textureRect.height * Thickness);
            FlaiDebug.DrawLine(this.Position2D, hit.point, ColorF.Red);
        }

        private void MoveObjects()
        {
            if (!this.IsOn)
            {
                _gameObjectsOnFunnel.Clear();
                return;
            }

            _gameObjectsOnFunnel.RemoveWhere(go =>
            {
                bool remove = go == null || !PhysicsHelper.Intersects(go.collider2D, this.collider2D);
                if (remove && go.rigidbody2D != null)
                {
                    go.rigidbody2D.gravityScale = 1;
                }

                return remove;
            });

            foreach (GameObject go in _gameObjectsOnFunnel)
            {
                go.AddPosition2D(this.RotationDirection2D * this.Speed * Time.deltaTime * (this.IsReversed ? -1 : 1));
                if (go.rigidbody2D)
                {
                    go.rigidbody2D.velocity *= 0.98f;
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            _gameObjectsOnFunnel.Add(other.gameObject);
            if (other.rigidbody2D != null)
            {
                other.rigidbody2D.gravityScale = 0;
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            _gameObjectsOnFunnel.Remove(other.gameObject);
            if (other.rigidbody2D != null)
            {
                other.rigidbody2D.gravityScale = 1;
            }
        }
    }
}
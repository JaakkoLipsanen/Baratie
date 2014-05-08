using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using System.Collections.Generic;
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
                this.RemoveAll();
                return;
            }
        
            this.RemoveGameObjectsOutside();
            foreach (GameObject go in _gameObjectsOnFunnel)
            {
                go.AddPosition2D(this.RotationDirection2D * this.Speed * Time.deltaTime * (this.IsReversed ? -1 : 1));
                if (go.rigidbody2D != null)
                {
                    go.rigidbody2D.velocity *= 0.9f;
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
            }
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (_gameObjectsOnFunnel.Remove(other.gameObject))
            {
                var gravityState = other.TryGet<GravityState>();
                gravityState.UseGravity = true;
            }
        }

        private void RemoveAll()
        {
            foreach (GameObject go in _gameObjectsOnFunnel)
            {
                if (go != null)
                {
                    go.Get<GravityState>().UseGravity = true;
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
                    go.Get<GravityState>().UseGravity = true;
                }

                return remove;
            });
        }
    }
}
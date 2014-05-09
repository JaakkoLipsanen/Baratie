using Assets.Misc;
using Assets.Scripts.Player;
using Flai;
using Flai.Inspector;
using Flai.Scene;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SeparationField : FlaiScript
    {
        private PlayerManager _playerManager;

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public float Length
        {
            get { return this.Scale2D.X; }
            set { this.Transform.SetScaleX(value); }
        }

        [ShowInInspector(IsEditableWhenNotPlaying = true)]
        public Direction2D Direction
        {
            get { return DirectionHelper.FromRotation(this.Rotation2D); }
            set { this.Rotation2D = value.ToDegrees(); }
        }

        protected override void Awake()
        {
            _playerManager = Scene.FindOfType<PlayerManager>();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (_playerManager.IsSeparated && BaratieHelper.IsPlayer(other.gameObject))
            {
                _playerManager.Combine();
            }
        }
    }
}
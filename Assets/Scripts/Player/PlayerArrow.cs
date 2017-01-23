using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerArrow : FlaiScript
    {
        private PlayerManager _playerManager;
        protected override void Start()
        {
            var playerManagerGameObject = this.RootParent;
            _playerManager = playerManagerGameObject.Get<PlayerManager>();

            if (_playerManager == null)
            {
                FlaiDebug.LogErrorWithTypeTag<PlayerArrow>("Could not find PlayerManager!");
            }
        }

        protected override void LateUpdate()
        {
            if (_playerManager == null)
            {
                return;
            }

            var currentPlayer = _playerManager.CurrentPlayer;
            this.Parent = currentPlayer.GameObject;
            this.LocalPosition2D = Vector2f.Up * Tile.Size * 1.5f;

            this.Scale2D = Vector2f.Abs(this.Scale2D);
            this.Rotation = Vector3.zero;

            this.GetComponent<Renderer>().enabled = _playerManager.IsSeparated;
        }
    }
}
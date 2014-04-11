﻿using System.Runtime.InteropServices;
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
            var playerController = currentPlayer.Get<PlayerController>();

            this.Parent = currentPlayer;
            this.LocalPosition2D = Vector2f.Up * Tile.Size * 1.5f;

            this.Scale2D = Vector2f.Abs(this.Scale2D);
            this.Rotation = Vector3.zero;
            if (playerController.GroundDirection == VerticalDirection.Up)
            {
              //  this.Scale2D = new Vector2f(this.Scale2D.X, -FlaiMath.Abs(this.Scale2D.Y));
            }
            else
            {
               // this.Scale2D = new Vector2f(this.Scale2D.X, FlaiMath.Abs(this.Scale2D.Y));
            }

            (this.renderer as SpriteRenderer).enabled = _playerManager.IsSeparated;
        }
    }
}
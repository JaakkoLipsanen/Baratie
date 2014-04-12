using Assets.Game.Model;
using Assets.Misc;
using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using System;
using Flai.Scene;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerManager : FlaiScript
    {
        #region Fields and Properties

        private float _shiftPressedDownTime = 0f;
        private bool _isShiftActionDone = false;
        private GameDimension _currentGameDimension;

        private PlayerInfo _combinedPlayer;
        private PlayerInfo _whitePlayer;
        private PlayerInfo _blackPlayer;
        private GameObject _playerArrow;

        public GameObject PlayerPrefab;
        public GameObject PlayerArrowPrefab;

        public GameDimension CurrentGameDimension
        {
            get { return _currentGameDimension; }
        }

        public PlayerInfo CurrentPlayer
        {
            get
            {
                switch (_currentGameDimension)
                {
                    case GameDimension.Both:
                        return _combinedPlayer;

                    case GameDimension.White:
                        return _whitePlayer;

                    case GameDimension.Black:
                        return _blackPlayer;

                    default:
                        throw new ArgumentException("value");
                }
            }
        }

        public PlayerInfo NonCurrentPlayer
        {
            get
            {
                Ensure.True(this.IsSeparated);
                switch (_currentGameDimension)
                {
                    case GameDimension.White:
                        return _blackPlayer;

                    case GameDimension.Black:
                        return _whitePlayer;

                    default:
                        throw new ArgumentException("value");
                }
            }
        }

        public bool IsSeparated
        {
            get { return _currentGameDimension != GameDimension.Both; }
        }

        public int KeyCount { get; private set; }

        #endregion

        protected override void Awake()
        {
            _combinedPlayer = this.CreatePlayer(this.Position2D, "CombinedPlayer");
            _playerArrow = this.PlayerArrowPrefab.Instantiate();
            _playerArrow.SetParent(this.GameObject);
        }

        public void AddKey()
        {
            this.KeyCount++;
        }

        protected override void Update()
        {
            this.HandleInput();
        }

        #region Handle Input

        private void HandleInput()
        {
            if (FlaiInput.IsButtonPressed("ChangeDimension"))
            {
                _shiftPressedDownTime += Time.deltaTime;
                if ((!_isShiftActionDone && (_shiftPressedDownTime >= 0.35f || !this.IsSeparated))) // if players are NOT separated, then player will split as soon as shift is pressed down
                {
                    this.ChangeMode();
                    _isShiftActionDone = true;
                }
            }
            else if (FlaiInput.IsNewButtonRelease("ChangeDimension"))
            {
                _shiftPressedDownTime = 0;
                if (_isShiftActionDone)
                {
                    _isShiftActionDone = false;
                    return;
                }

                if (!this.IsSeparated)
                {
                    this.ChangeMode();
                }
                else
                {
                    this.ChangePlayer();
                }
            }
        }

        #endregion

        #region Change Player/Mode

        private void ChangePlayer()
        {
            Ensure.True(this.IsSeparated);

            _currentGameDimension = _currentGameDimension.Opposite();
            this.RefreshPlayers();
            FlaiDebug.LogWithTypeTag<PlayerManager>("Player Changed");
        }

        private void ChangeMode()
        {
            _playerArrow.SetParent(null);
            if (this.IsSeparated)
            {
                this.Combine();
            }
            else
            {
                this.Split();
            }

            FlaiDebug.LogWithTypeTag<PlayerManager>("Player Mode Changed");
        }

        #endregion

        #region Split & Combine

        private void Combine()
        {
            Ensure.True(this.IsSeparated);

            _combinedPlayer = this.CreatePlayer(this.CurrentPlayer.Position2D, "Combined Player");
            _combinedPlayer.Controller.FacingDirection = this.CurrentPlayer.Controller.FacingDirection;
            _combinedPlayer.Velocity = this.CurrentPlayer.Velocity;

            // if the combined player is holding a crate, then set the new current player to hold the same crate
            if (this.CurrentPlayer.CratePicker.IsPicking)
            {
                this.CurrentPlayer.CratePicker.ChangeCrateOwner(_combinedPlayer.CratePicker);
            }

            if (this.NonCurrentPlayer.CratePicker.IsPicking)
            {
                this.NonCurrentPlayer.CratePicker.Drop();
            }

            _currentGameDimension = GameDimension.Both;
            Scene.DestroyGameObject(ref _whitePlayer);
            Scene.DestroyGameObject(ref _blackPlayer);
        }

        private void Split()
        {
            Ensure.True(!this.IsSeparated);

            _currentGameDimension = GameDimension.Black; // always black after splitted: looks good. think more about this later
            HorizontalDirection currentDirection = _combinedPlayer.Controller.FacingDirection;
            _blackPlayer = this.CreateSplittedPlayer(GameDimension.Black, "Black Player", currentDirection);
            _whitePlayer = this.CreateSplittedPlayer(GameDimension.White, "White Player", currentDirection.Opposite());

            // if the combined player is holding a crate, then set the new current player to hold the same crate
            if (_combinedPlayer.CratePicker.IsPicking)
            {
                _combinedPlayer.CratePicker.ChangeCrateOwner(this.CurrentPlayer.CratePicker);
            }

            this.CurrentPlayer.IsInForeground = true;
            this.NonCurrentPlayer.IsInForeground = false;

            Scene.DestroyGameObject(ref _combinedPlayer);
            this.RefreshPlayers();
        }

        #endregion

        #region Create Player

        private PlayerInfo CreatePlayer(Vector2 position, string name)
        {
            Ensure.NotNull(this.PlayerPrefab);
            GameObject player = this.PlayerPrefab.Instantiate(position);
            player.name = name;
            player.SetParent(this.GameObject);

            return player.Get<PlayerInfo>();
        }

        private PlayerInfo CreateSplittedPlayer(GameDimension gameDimension, string name, HorizontalDirection facingDirection)
        {
            Vector2f finalPosition = _combinedPlayer.Position2D;
            if (_currentGameDimension != gameDimension)
            {
                Vector2f shiftAmount = this.FindSplitShiftAmount();
                finalPosition += shiftAmount;
            }

            PlayerInfo player = this.CreatePlayer(finalPosition, name);
            SpriteRenderer spriteRenderer = (SpriteRenderer)player.renderer;
            if (gameDimension == GameDimension.Black)
            {
                spriteRenderer.color = new ColorF(64);
            }
            else // gameDimension == GameDimension.White
            {
                spriteRenderer.color = new ColorF(228);
                player.GetChild("Eye").Get<SpriteRenderer>().color = new ColorF(32);
            }

            player.Velocity = _combinedPlayer.Velocity;
            player.Controller.FacingDirection = facingDirection;
            return player;
        }

        private Vector2f FindSplitShiftAmount()
        {
            Vector2f defaultSplitAmount = Vector2f.UnitX * Tile.Size * 1.5f;
            if (this.CurrentPlayer.Controller.FacingDirection == HorizontalDirection.Right)
            {
                FlaiDebug.Log("change");
                defaultSplitAmount *= -1; // split amount is to left nows
            }

            RectangleF playerArea = this.CurrentPlayer.collider2D.GetBoundsHack().AsInflated(0, -0.01f);
            FlaiDebug.DrawRectangleOutlines(playerArea, ColorF.Blue, 1.5f);
            for (float fractionX = 1f; fractionX >= 0; fractionX -= 0.05f)
            {
                for (float yOffset = 0; yOffset <= 1f; yOffset += 0.1f)
                {
                    RectangleF newArea = playerArea;
                    newArea.Offset(defaultSplitAmount * fractionX + Vector2f.UnitY * yOffset);

                    FlaiDebug.DrawRectangleOutlines(newArea, ColorF.Green * (0.15f + (0.85f) * fractionX), 5.5f);
                    if (Physics2D.OverlapArea(newArea.TopLeft, newArea.BottomRight, BaratieConstants.IgnorePlayerLayerMask) == null)
                    {
                        return defaultSplitAmount * fractionX + Vector2f.UnitY * yOffset;
                    }
                }

            }

            return Vector2f.Zero;
        }

        #endregion

        private void RefreshPlayers()
        {
            if (!this.IsSeparated)
            {
                return;
            }

            this.CurrentPlayer.Controller.IsControllingEnabled = true;
            this.NonCurrentPlayer.Controller.IsControllingEnabled = false;

            this.CurrentPlayer.IsInForeground = true;
            this.NonCurrentPlayer.IsInForeground = false;
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.DrawIcon(this.Position, "PlayerManagerGizmo", false);
        }
    }
}

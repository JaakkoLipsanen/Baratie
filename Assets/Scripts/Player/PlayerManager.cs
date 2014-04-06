using System;
using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public enum GameDimension
    {
        Both,
        White,
        Black,
    }

    public static class GameDimensionHelper
    {
        public static GameDimension Opposite(this GameDimension gameDimension)
        {
            switch (gameDimension)
            {
                case GameDimension.Black:
                    return GameDimension.White;

                case GameDimension.White:
                    return GameDimension.Black;

                default:
                    return GameDimension.Both;
            }
        }
    }

    public class PlayerManager : FlaiScript
    {
        private float _shiftPressedDownTime = 0f;
        private bool _isShiftActionDone = false;

        public GameObject PlayerPrefab;
        private GameDimension _currentGameDimension; // null == not seperated, 0 == first one, 1 == second one

        private GameObject _combinedPlayer;
        private GameObject _whitePlayer;
        private GameObject _blackPlayer;

        public GameDimension CurrentGameDimension
        {
            get { return _currentGameDimension; }
        }

        public GameObject CurrentPlayer
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

        public GameObject NonCurrentPlayer
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

        protected override void Awake()
        {
            _combinedPlayer = this.CreatePlayer(this.Position2D, "CombinedPlayer");
        }

        protected override void Update()
        {
            this.HandleInput();
        }

        private void HandleInput()
        {
            if (FlaiInput.IsKeyPressed(KeyCode.LeftShift))
            {
                _shiftPressedDownTime += Time.deltaTime;
                if (!_isShiftActionDone && _shiftPressedDownTime >= 0.5f)
                {
                    this.ChangeMode();
                    _isShiftActionDone = true;
                }
            }
            else if (FlaiInput.IsNewKeyRelease(KeyCode.LeftShift))
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

        private void ChangePlayer()
        {
            if (!this.IsSeparated)
            {
                return;
            }

            _currentGameDimension = _currentGameDimension.Opposite();
            this.RefreshPlayers();
            FlaiDebug.LogWithTypeTag<PlayerManager>("Player Changed");
        }

        private void ChangeMode()
        {
            if (this.IsSeparated)
            {
                _combinedPlayer = this.PlayerPrefab.Instantiate(this.CurrentPlayer.GetPosition2D());
                _whitePlayer.Destroy();
                _blackPlayer.Destroy();

                _whitePlayer = null;
                _blackPlayer = null;
                _currentGameDimension = GameDimension.Both;
            }
            else
            {
                _whitePlayer = this.CreatePlayer(_combinedPlayer.GetPosition2D() - Vector2f.UnitX.ToVector2() * Tile.Size, "WhitePlayer");
                _blackPlayer = this.CreatePlayer(_combinedPlayer.GetPosition2D() + Vector2f.UnitX.ToVector2() * Tile.Size, "BlackPlayer");
                _combinedPlayer.Destroy();
                _combinedPlayer = null;

                _whitePlayer.Get<SpriteRenderer>().color = new Color32(228, 228, 228, 255);
                _blackPlayer.Get<SpriteRenderer>().color = new Color32(64, 64, 64, 255);
                _currentGameDimension = GameDimension.Black;
                this.RefreshPlayers();          
            }

            FlaiDebug.LogWithTypeTag<PlayerManager>("Player Mode Changed");
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.DrawIcon(this.Position, "PlayerManagerGizmo", false);
        }

        private GameObject CreatePlayer(Vector2 position, string name)
        {
            Ensure.NotNull(this.PlayerPrefab);
            GameObject player = this.PlayerPrefab.Instantiate(position);
            player.name = name;
            player.SetParent(this.GameObject);

            return player;
        }

        private void RefreshPlayers()
        {
            if (!this.IsSeparated)
            {
                return;
            }

            this.CurrentPlayer.Get<PlayerController>().IsControllingEnabled = true;
            this.NonCurrentPlayer.Get<PlayerController>().IsControllingEnabled = false;
        }
    }
}

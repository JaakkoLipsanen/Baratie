using Assets.Game.Model;
using Assets.Scripts.General;
using Flai;
using Flai.Diagnostics;
using Flai.Input;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerManager : FlaiScript
    {
        private float _shiftPressedDownTime = 0f;
        private bool _isShiftActionDone = false;
        private GameDimension _currentGameDimension;
        private int _keyCount;

        private GameDimension _previousSplitGameDimension = GameDimension.Black;

        private GameObject _combinedPlayer;
        private GameObject _whitePlayer;
        private GameObject _blackPlayer;
        private GameObject _playerArrow;

        public GameObject PlayerPrefab;
        public GameObject PlayerArrowPrefab;

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

        public int KeyCount
        {
            get { return _keyCount; }
        }

        protected override void Awake()
        {
            _combinedPlayer = this.CreatePlayer(this.Position2D, "CombinedPlayer");
            _playerArrow = this.PlayerArrowPrefab.Instantiate();
            _playerArrow.SetParent(this.GameObject);
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
            _previousSplitGameDimension = CurrentGameDimension;
            this.RefreshPlayers();
            FlaiDebug.LogWithTypeTag<PlayerManager>("Player Changed");
        }

        private void ChangeMode()
        {
            _playerArrow.SetParent(null);
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
                Vector2f shiftAmount = Vector2f.UnitX * Tile.Size * 1.5f;
                _whitePlayer = this.CreatePlayer(_combinedPlayer.GetPosition2D() + ((_previousSplitGameDimension == GameDimension.Black) ? shiftAmount : Vector2f.Zero), "WhitePlayer");
                _blackPlayer = this.CreatePlayer(_combinedPlayer.GetPosition2D() + ((_previousSplitGameDimension == GameDimension.White) ? shiftAmount : Vector2f.Zero), "BlackPlayer");
                _combinedPlayer.Destroy();
                _combinedPlayer = null;

                _whitePlayer.Get<SpriteRenderer>().color = new Color32(228, 228, 228, 255);
                _whitePlayer.GetChild("Eye").Get<SpriteRenderer>().color = new ColorF(32, 32, 32);
                _blackPlayer.Get<SpriteRenderer>().color = new Color32(64, 64, 64, 255);
                _currentGameDimension = _previousSplitGameDimension;
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

        public void AddKey()
        {
            _keyCount++;
        }
    }
}

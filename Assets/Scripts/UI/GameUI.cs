using Assets.Scripts.Player;
using Flai;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameUI : FlaiScript
    {
        public Texture2D KeyTexture;
        private PlayerManager _playerManager;

        protected override void Start()
        {
            _playerManager = Object.FindObjectOfType<PlayerManager>();
        }

        protected override void OnGUI()
        {
            this.DrawKey();
        }

        private void DrawKey()
        {
            if (_playerManager.KeyCount > 0)
            {
                GUI.color = ColorF.White * 0.5f;
                const float KeyScale = 4;
                const float OffsetFromBorder = 8;

                float width = this.KeyTexture.width * KeyScale;
                float height = this.KeyTexture.height * KeyScale;

                float x = Screen.width - OffsetFromBorder - width;
                float y = OffsetFromBorder + height;

                GUI.DrawTexture(new Rect(x, y, width, height), this.KeyTexture);
            }
        }
    }
}
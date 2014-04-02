using Assets.Game.Model.Tilemap;
using Assets.Scripts.General;
using Flai;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.View
{
    public class TilemapSpriteManager : Singleton<TilemapSpriteManager>
    {
        private readonly Dictionary<int, Sprite> _indexToSpriteDict = new Dictionary<int, Sprite>();
        private readonly Dictionary<string, Texture2D> _tilesetTextures = new Dictionary<string, Texture2D>();

        public Dictionary<int, Sprite>.ValueCollection Sprites
        {
            get { return _indexToSpriteDict.Values; }
        }

        public Sprite GetSprite(TilesetManager tilesetManager, int index)
        {
            Sprite sprite;
            if (!_indexToSpriteDict.TryGetValue(index, out sprite))
            {
                sprite = this.CreateSprite(tilesetManager, index);
                _indexToSpriteDict.Add(index, sprite);
            }

            return sprite;
        }

        private Sprite CreateSprite(TilesetManager tilesetManager, int index)
        {
            string textureName;
            Rect sourceRectangle;
            tilesetManager.GetTile(index, out textureName, out sourceRectangle);

            return Sprite.Create(this.LoadTexture(textureName), sourceRectangle, Vector2.zero, Tile.SizeInTexture);
        }

        private Texture2D LoadTexture(string tileset)
        {
            tileset = tileset.Trim();
            Texture2D texture;
            if (!_tilesetTextures.TryGetValue(tileset, out texture))
            {
                texture = Resources.Load<Texture2D>(tileset);
                _tilesetTextures.Add(tileset, texture);
            }

            return texture;
        }

        public void Reset()
        {
            _indexToSpriteDict.Clear();
            _tilesetTextures.Clear();
        }
    }
}

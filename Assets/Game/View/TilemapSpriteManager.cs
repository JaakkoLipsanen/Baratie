using Assets.Game.Model.Tilemap;
using Assets.Scripts.General;
using Flai;
using System.Collections.Generic;
using Flai.Diagnostics;
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

            Texture2D texture = this.LoadTexture(textureName);
            FlaiDebug.Log("{0} - {1} .. {2}", sourceRectangle, texture.GetSize(), texture == null);

            // !!! the SpriteMeshType.FullRect is needed  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return Sprite.Create(this.LoadTexture(textureName), sourceRectangle, Vector2.zero, Tile.SizeInTexture, 0, SpriteMeshType.FullRect);
        }

        private Texture2D LoadTexture(string tileset)
        {
            tileset = tileset.Trim();
            Texture2D texture;
            if (!_tilesetTextures.TryGetValue(tileset, out texture) || texture == null)
            {
                texture = Resources.Load<Texture2D>(tileset);
                _tilesetTextures.AddOrSetValue(tileset, texture);
                if (texture == null)
                {
                    FlaiDebug.LogWarningWithTypeTag<TilemapSpriteManager>("Couldn't find tileset texture {0}", tileset);
                }
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

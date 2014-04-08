using System;
using Assets.Game.Model.Tilemap;
using Assets.Game.View;
using Flai;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.General
{
    public class TilemapData : MonoBehaviour, IEquatable<TilemapData>
    {
        public MapData Map;

        public Tilemap Tilemap
        {
            get { return this.Map.Tilemap; }
        }

        public bool Equals(TilemapData other)
        {
            return MapData.AreEqual(this.Map, other.Map);
        }

        public void OnMapUpdated()
        {
            Ensure.IsEditor();
            this.GetChild("Tiles").DestroyImmediateIfNotNull();

            // if the map was removed, then no need to create a new map
            if (this.Map == null || this.Tilemap == null)
            {
                return;
            }

            FlaiDebug.LogWithTypeTag<TilemapData>("Building a tilemap ({0}x{1})", this.Tilemap.Width, this.Tilemap.Height);
            GameObject tiles = new GameObject("Tiles");
            tiles.SetParent(this.gameObject);
            for (int y = 0; y < this.Tilemap.Height; y++)
            {
                for (int x = 0; x < this.Tilemap.Width; x++)
                {
                    if (this.Tilemap[x, y] != 0)
                    {
                        try
                        {
                            GameObject tile = this.CreateTile(x, y);
                            tile.SetParent(tiles);
                        }
                        catch
                        {
                            FlaiDebug.LogErrorWithTypeTag<TilemapData>("Error while creating tiles - aborting");
                            this.Map = null;
                            this.OnMapUpdated();
                            return;
                        }
                    }
                }
            }

            this.Map.NeedsRefresh = false;
        }

        private GameObject CreateTile(int x, int y)
        {
            int realY = this.Tilemap.Height - y - 1; // inverted
            float xPosition = x * Tile.Size;
            float yPosition = realY * Tile.Size;

            GameObject tile = new GameObject("Tile");
            tile.SetPosition2D(xPosition, yPosition);
            tile.SetScale2D(Tile.Size);

            SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
            Sprite sprite = TilemapSpriteManager.Instance.GetSprite(this.Map.TilesetManager, this.Tilemap[x, y]);
            spriteRenderer.sprite = sprite;

            tile.AddComponent<PolygonCollider2D>();

            return tile;
        }
    }
}

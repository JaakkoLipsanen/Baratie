using Assets.Game.Model.Tilemap;
using Flai;
using UnityEngine;

namespace Assets.Scripts.General
{
    public static class Tile
    {
        public const float Size = 1;
        public const float SizeInTexture = 32;
    }

    public class Level : MonoBehaviour
    {
        private MapData _mapData;
        public Size MapSize { get; private set; }
        /*    public void LoadLevel(TmxData tmxData)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError("Level cannot be loaded from code");
                    return;
                }

                Debug.Log("Size!: " + tmxData.VersionTag);
                this.MapSize = tmxData.MapSize;
                this.CreateTilemaps(tmxData);
            }

            private void CreateTilemaps(TmxData tmxData)
            {
                GameObject tilemap = new GameObject("Tilemap");
                tilemap.transform.parent = this.transform;

                const float TileOffset = Tile.Size / 2f;

                TileLayer tileLayer = tmxData.GetLayer("Map");
                int[] tileData = tmxData.GetLayer("Map").TileData;

                string path = tmxData.Tilesets[0].ImagePath;
                string realPath = path.Substring(path.IndexOf("Assets/") + "Assets/".Length).Replace(".png", "");
                Texture2D texture = Resources.Load(realPath) as Texture2D;
                Debug.Log("Texture is " + (texture == null));
                Debug.Log("Texture path: " + realPath);
                for (int y = 0; y < this.MapSize.Height; y++)
                {
                    for (int x = 0; x < this.MapSize.Width; x++)
                    {
                        int tileIndex = tileData[x + y * this.MapSize.Width];
                        if (tileIndex != 0)
                        {
                            int realX = x;
                            int realyY = this.MapSize.Height - 1 - y;

                            GameObject tile = new GameObject("Tile");
                            var sprite = Sprite.Create(texture, new Rect(Tile.SizeInTexture * (tileIndex - 1), 0, Tile.SizeInTexture, Tile.SizeInTexture), Vector2.zero);
                            var spriteRenderer = tile.AddComponent<SpriteRenderer>();

                       //     Debug.Log(sprite == null);
                            spriteRenderer.sprite = sprite;
                            spriteRenderer.color = Color.green;
                            spriteRenderer.sharedMaterial = RenderInfo.Instance.DefaultSpriteMaterial;

                            // set transform
                            tile.transform.position = new Vector3(realX * Tile.Size + TileOffset, realyY * Tile.Size + TileOffset, 0);
                            tile.transform.localScale = new Vector3(Tile.Size, Tile.Size, Tile.Size);
                            tile.transform.parent = tilemap.transform;
                        }
                    }
                }
            } */
    }
}

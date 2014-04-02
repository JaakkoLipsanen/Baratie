using System.IO;
using System.Linq;
using Assets.Game.Model.Tilemap;
using Flai;
using Flai.Unity.Tiled;
using UnityEngine;

namespace Assets.Editor.Misc
{
    public class TilemapLoader
    {
        public static MapData LoadTilemap(string assetPath)
        {
            var tmxData = TmxData.Load(assetPath);
            var data = ScriptableObject.CreateInstance<MapData>();
            TilemapLoader.InitializeTilemapData(data, tmxData);

            return data;
        }

        private static void InitializeTilemapData(MapData data, TmxData tmxData)
        {
            // todo: white & black layers etc
            TmxTileLayer tileLayer = TilemapLoader.GetMapTileLayer(tmxData);
            Tilemap tilemap = new Tilemap(tileLayer.TileData, tmxData.MapSize);
            TilesetManager tilesetManager = TilemapLoader.CreateTilesetManager(tmxData);

            data.Initialize(tilemap, tilesetManager);
        }

        private static TmxTileLayer GetMapTileLayer(TmxData tmxData)
        {
            // todo: HAAACKKKK
            try
            {
                return tmxData.GetLayer("Map");
            }
            catch { }

            return tmxData.TmxTileLayers[0];
        }

        private static TilesetManager CreateTilesetManager(TmxData tmxData)
        {
            return new TilesetManager(tmxData.TmxTilesets.Select(ts =>
            {
                Debug.Log("!" + ts.ImageSize.Width + " " + ts.ImageSize.Height);
                Ensure.True(ts.TileSize.Width == ts.TileSize.Height);
                return new Tileset(ts.Name, ts.FirstGlobalTileID, ts.TileSize.Width, Path.GetFileNameWithoutExtension(ts.ImagePath), ts.ImageSize.Width, ts.ImageSize.Height);
            }).ToArray());
        }
    }
}
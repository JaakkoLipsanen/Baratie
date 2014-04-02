using Assets.Editor.Misc;
using Assets.Game.Model.Tilemap;
using Assets.Misc;
using Flai.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Assets.Editor.Importers
{
    public class TilemapImporter : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            if (importedAssets == null)
            {
                return;
            }

            foreach (string tilemapAsset in importedAssets.Where(asset => asset != null && asset.EndsWith(".tmx")))
            {
                if (tilemapAsset == null)
                {
                    continue;
                }
  
                string fileName = Path.GetFileNameWithoutExtension(tilemapAsset);
                string finalPath = Path.Combine(BaratiePaths.TilemapPath, fileName) + ".asset";

                MapData mapData = TilemapLoader.LoadTilemap(tilemapAsset);
                MapData previousData = (MapData)AssetDatabase.LoadAssetAtPath(finalPath, typeof (MapData));
                if (previousData != null)
                {
                    previousData.CopyFrom(mapData);
                    FlaiDebug.LogWithTypeTag<TilemapImporter>("Tilemap {0} updated!", fileName);
                }
                else
                {
                    AssetDatabase.CreateAsset(mapData, finalPath);
                    FlaiDebug.LogWithTypeTag<TilemapImporter>("Tilemap {0} imported succesfully!", fileName);
                }

                AssetDatabase.DeleteAsset(tilemapAsset);
            }

            AssetDatabase.SaveAssets();
        } 
    }
}

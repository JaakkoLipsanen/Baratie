﻿using System.Diagnostics;
using Assets.Editor.Misc;
using Assets.Game.Model.Tilemap;
using Assets.Misc;
using Flai.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
                MapData previousData = (MapData)AssetDatabase.LoadAssetAtPath(finalPath, typeof(MapData));
                if (previousData != null)
                {
                    previousData.CopyFrom(mapData);
                    FlaiDebug.LogWithTypeTag<TilemapImporter>("Tilemap {0} updated!", fileName);
                    EditorUtility.SetDirty(previousData);
                }
                else
                {
                    AssetDatabase.CreateAsset(mapData, finalPath);
                    EditorUtility.SetDirty(mapData);
                    FlaiDebug.LogWithTypeTag<TilemapImporter>("Tilemap {0} imported succesfully!", fileName);
                }

                TilemapImporter.SaveTilemapFile(tilemapAsset);
                AssetDatabase.DeleteAsset(tilemapAsset);
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static void SaveTilemapFile(string tilemapPath)
        {
            const string FolderName = "Saved .TMXs/TMX";
            const string TempFolderName = "Temp";

            string projectRootFolder = Directory.GetParent(Application.dataPath).FullName;
            string targetFolderPath = Path.Combine(projectRootFolder, FolderName);
            Directory.CreateDirectory(targetFolderPath);

            string tilemapName = Path.GetFileName(tilemapPath);
            string newFilePath = Path.Combine(targetFolderPath, tilemapName);
            if (File.Exists(newFilePath))
            {
                string tempFolder = Path.Combine(targetFolderPath, TempFolderName);
                Directory.CreateDirectory(tempFolder);

                for (int i = 1; ; i++)
                {
                    string newTempFilePath = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(tilemapName) + "_" + i + Path.GetExtension(tilemapName));
                    if (!File.Exists(newTempFilePath))
                    {
                        File.Move(newFilePath, newTempFilePath);
                        break;
                    }
                }
            }

            // copy the tilemap file
            File.Copy(tilemapPath, newFilePath, true);
        }
    }
}

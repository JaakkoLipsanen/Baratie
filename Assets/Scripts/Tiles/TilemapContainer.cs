using Assets.Game.Model.Tilemap;
using Flai;
using Flai.Diagnostics;
using System;
using UnityEngine;

namespace Assets.Scripts.Tiles
{
    public class TilemapContainer : FlaiScript, IEquatable<TilemapContainer>
    {
        public GameObject TilemapPrefab;
        public MapData Map;

        public TilemapData TilemapData
        {
            get { return this.Map.TilemapData; }
        }

        public bool Equals(TilemapContainer other)
        {
            return MapData.AreEqual(this.Map, other.Map);
        }

        public void OnMapUpdated()
        {
            Ensure.IsEditor();

            const string TileMapName = "Default Tilemap";
            this.GetChild(TileMapName).DestroyImmediateIfNotNull();

            // if the map was removed, then no need to create a new map
            if (this.Map == null || this.TilemapData == null)
            {
                return;
            }

            FlaiDebug.LogWithTypeTag<TilemapContainer>("Building a tilemap ({0}x{1})", this.TilemapData.Width, this.TilemapData.Height);
            GameObject tilemap = this.TilemapPrefab.Instantiate();
            tilemap.name = TileMapName;
            tilemap.SetParent(this.GameObject);
            tilemap.SetLayer("Tiles");

            tilemap.Get<Tilemap>().CreateFrom(this.Map.TilemapData, this.Map.TilesetManager);
            this.Map.NeedsRefresh = false;
        }
    }
}

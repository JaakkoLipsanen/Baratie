using Flai;
using System;
using UnityEngine;

namespace Assets.Game.Model.Tilemap
{
    /* todo: black & white tilemaps seperated */
    [Serializable]
    public class MapData : ScriptableObject // TilemapAsset?
    {
        public event GenericEvent Changed;

        [SerializeField]
        private TilemapData _tilemapData;

        [SerializeField]
        private TilesetManager _tilesetManager;

        [SerializeField]
        private int _creationGuid; // can't use GUID since it's not serializable

        [SerializeField]
        private bool _needsRefresh = true; 

        public TilemapData TilemapData
        {
            get { return _tilemapData; }
        }

        public TilesetManager TilesetManager
        {
            get { return _tilesetManager; }
        }

        public Size Size
        {
            get { return _tilemapData.Size; }
        }

        public int UniqueGUID
        {
            get { return _creationGuid; }
        }

        public bool NeedsRefresh
        {
            get { return _needsRefresh; }
            set { _needsRefresh = value; } // meh
        }

        public void Initialize(TilemapData tilemapData, TilesetManager tilesetManager)
        {
            _tilemapData = tilemapData;
            _tilesetManager = tilesetManager;
            _creationGuid = Global.Random.Next();
        }

        public void CopyFrom(MapData other)
        {
            this.Initialize(other.TilemapData, other.TilesetManager);
            _needsRefresh = true;
            this.Changed.InvokeIfNotNull();
        }

        public static bool AreEqual(MapData map1, MapData map2)
        {
            if (map1 == null || map2 == null)
            {
                return map1 == map2;
            }
            else if (map1.Size != map2.Size)
            {
                return false;
            }

            return TilemapData.AreEqual(map1.TilemapData, map2.TilemapData);
        }
    }
}

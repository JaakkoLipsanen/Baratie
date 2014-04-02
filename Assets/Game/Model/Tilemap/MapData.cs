using Flai;
using System;
using UnityEngine;
using TilemapClass = Assets.Game.Model.Tilemap.Tilemap;

namespace Assets.Game.Model.Tilemap
{
    /* todo: black & white tilemaps seperated */
    [Serializable]
    public class MapData : ScriptableObject // TilemapAsset?
    {
        public event GenericEvent Changed;

        [SerializeField]
        private Tilemap _tilemap;

        [SerializeField]
        private TilesetManager _tilesetManager;

        [SerializeField]
        private int _creationGuid; // can't use GUID since it's not serializable

        public Tilemap Tilemap
        {
            get { return _tilemap; }
        }

        public TilesetManager TilesetManager
        {
            get { return _tilesetManager; }
        }

        public Size Size
        {
            get { return _tilemap.Size; }
        }

        public int UniqueGUID
        {
            get { return _creationGuid; }
        }

        public void Initialize(Tilemap tilemap, TilesetManager tilesetManager)
        {
            if (Application.isPlaying)
            {
                throw new InvalidOperationException("Cannot initialize the tilemap outside of the editor");
            }

            _tilemap = tilemap;
            _tilesetManager = tilesetManager;
            _creationGuid = Global.Random.Next();
        }

        public void CopyFrom(MapData other)
        {
            this.Initialize(other.Tilemap, other.TilesetManager);
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

            return TilemapClass.AreEqual(map1.Tilemap, map2.Tilemap);
        }
    }
}

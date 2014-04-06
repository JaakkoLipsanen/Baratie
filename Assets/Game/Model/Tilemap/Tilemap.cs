using Flai;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Model.Tilemap
{
    [Serializable]
    public class Tilemap
    {
        [SerializeField]
        private int[] _tiles;
        [SerializeField]
        private int _width;
        [SerializeField]
        private int _height;

        public Size Size
        {
            get { return new Size(_width, _height); }
        }

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Tilemap(int[] tiles, Size size)
        {
            _tiles = tiles;
            _width = size.Width;
            _height = size.Height;
        }

        public int this[Vector2i v]
        {
            get { return this[v.X, v.Y]; }
        }

        public int this[int x, int y]
        {
            get { return _tiles[x + _width * y]; }
        }

        public bool HasTileAt(Vector2i v)
        {
            return this.HasTileAt(v.X, v.Y);
        }

        public bool HasTileAt(int x, int y)
        {
            return this[x, y] != 0;
        }

        public static bool AreEqual(Tilemap tm1, Tilemap tm2)
        {
            return tm1.Size == tm2.Size && tm1._tiles.SequenceEqual(tm2._tiles); // todo: for loop would be faster
        }
    }
}

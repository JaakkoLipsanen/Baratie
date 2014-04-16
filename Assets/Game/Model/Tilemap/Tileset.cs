﻿using Flai;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Model.Tilemap
{
    [Serializable]
    public class TilesetManager
    {
        [SerializeField]
        private Tileset[] _tilesets;

        public TilesetManager(Tileset[] tilesets)
        {
            _tilesets = tilesets.OrderBy(tileset => tileset.FirstGid).ToArray();
        }

        public void GetTile(int id, out string textureName, out RectangleF sourceRectangle)
        {
            Tileset tileset = _tilesets.First(ts => id >= ts.FirstGid  && id < ts.FirstGid + ts.TileCount); // the second check is not necessary
            textureName = tileset.ImageName;
            sourceRectangle = tileset.GetSourceRectangle(id);
        }

        public RectangleF GetTileSourceRectangle(int id)
        {
            string textureName;
            RectangleF sourceRectangle;
            this.GetTile(id, out textureName, out sourceRectangle);

            return sourceRectangle;
        }
    }

    [Serializable]
    public class Tileset
    {
        [SerializeField]
        private int _firstGid;
        [SerializeField]
        private string _name;
        [SerializeField]
        private int _tileSize; // width and height are always same
        [SerializeField]
        private string _imageName;
        [SerializeField]
        private int _imageWidth;
        [SerializeField]
        private int _imageHeight;

        public int FirstGid
        {
            get { return _firstGid; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int TileSize
        {
            get { return _tileSize; }
        }

        public string ImageName
        {
            get { return _imageName; }
        }

        public int ImageWidth
        {
            get { return _imageWidth; }
        }

        public int ImageHeight
        {
            get { return _imageHeight; }
        }

        public int TileCount
        {
            get { return this.WidthInTiles * this.HeightInTiles; }
        }

        private int WidthInTiles
        {
            get { return (this.ImageWidth / this.TileSize); }
        }

        private int HeightInTiles
        {
            get { return (this.ImageHeight / this.TileSize); }
        }

        public Tileset(string name, int firstGid, int tileSize, string imageName, int imageWidth, int imageHeight)
        {
            Ensure.True(imageWidth % tileSize == 0);
            Ensure.True(imageHeight % tileSize == 0);

            _name = name;
            _firstGid = firstGid;
            _tileSize = tileSize;
            _imageName = imageName;
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;
        }

        public Rect GetSourceRectangle(int id)
        {
            Ensure.WithinRange(id, _firstGid, _firstGid + this.TileCount - 1);

            int absoluteId = id - _firstGid;
            int x = absoluteId % this.WidthInTiles;
            int y = absoluteId / this.WidthInTiles;
            return new Rect(x * _tileSize, y * _tileSize, _tileSize, _tileSize);
        }
    }
}

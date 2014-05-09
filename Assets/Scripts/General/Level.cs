using Flai;
using Flai.Tilemap;
using UnityEngine;

namespace Assets.Scripts.General
{
    public static class Tile
    {
        public const float Size = 1;
        public const float SizeInTexture = 32;
    }

    public class Level : FlaiScript
    {
        public TilemapContainer TilemapContainer
        {
            get { return this.GetComponentInChildren<TilemapContainer>(); }
        }

        protected override void Awake()
        {
            Physics2D.gravity = -Vector2f.Abs(Physics2D.gravity);
        }
    }
}

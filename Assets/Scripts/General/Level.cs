using Flai.Tilemap;
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
        public TilemapContainer TilemapContainer
        {
            get { return this.GetComponentInChildren<TilemapContainer>(); }
        }
    }
}

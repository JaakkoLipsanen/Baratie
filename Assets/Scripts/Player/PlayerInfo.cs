using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInfo : MonoBehaviour
    {
        public Color Color
        {
            get { return this.Get<SpriteRenderer>().color; }
            set { this.Get<SpriteRenderer>().color = value; }
        }
    }
}

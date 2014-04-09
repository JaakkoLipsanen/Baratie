using Flai;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInfo : FlaiScript
    {
        public Color Color
        {
            get { return this.Get<SpriteRenderer>().color; }
            set { this.Get<SpriteRenderer>().color = value; }
        }

        public PlayerManager Manager
        {
            get { return this.Parent.Get<PlayerManager>(); }
        }
    }
}

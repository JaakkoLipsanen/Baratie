using Flai;

namespace Assets.Scripts.Player
{
    public class PlayerInfo : FlaiScript
    {
        public PlayerManager Manager
        {
            get { return this.Parent.Get<PlayerManager>(); }
        }

        public PlayerController Controller
        {
            get { return this.Get<PlayerController>(); }
        }

        public CratePicker CratePicker
        {
            get { return this.Get<CratePicker>(); }
        }

        public Vector2f Velocity
        {
            get { return this.rigidbody2D.velocity; }
            set { this.rigidbody2D.velocity = value; }
        }

        public bool IsInForeground
        {
            set
            {
                this.renderer.sortingOrder = value ? -5 : -10;
                this.GetChild("Eye").renderer.sortingOrder = value ? -4 : -9;
            }
        }
    }
}

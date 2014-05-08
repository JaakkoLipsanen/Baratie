using Flai;
using Flai.Scripts.Character;

namespace Assets.Scripts.Player
{
    public class PlayerInfo : FlaiScript
    {
        public PlayerManager Manager
        {
            get { return this.Parent.Get<PlayerManager>(); }
        }

        public CharacterController2D Controller
        {
            get { return this.Get<CharacterController2D>(); }
        }

        public CharacterInput2D Input
        {
            get { return this.Get<CharacterInput2D>(); }
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

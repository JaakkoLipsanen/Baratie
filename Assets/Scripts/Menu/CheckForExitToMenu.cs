using Flai;
using Flai.Input;
using Flai.Scene;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class CheckForExitToMenu : FlaiScript
    {
        public KeyCode ExitKey = KeyCode.Escape;

        protected override void Update()
        {
            if (FlaiInput.IsNewKeyPress(this.ExitKey))
            {
                SceneFader.Fade(SceneDescription.FromIndex(0), Fade.Create(0.75f), Fade.Create(0.75f));
            }
        }
    }
}
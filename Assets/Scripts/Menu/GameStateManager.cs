using Flai;
using Flai.Input;
using Flai.Scene;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class GameStateManager : FlaiScript
    {
        public KeyCode ExitKey = KeyCode.Escape;

        protected override void Awake()
        {
            Physics2D.gravity = -Vector2f.Abs(Physics2D.gravity);
        }

        protected override void Update()
        {
            if (FlaiInput.IsNewKeyPress(this.ExitKey))
            {
                SceneFader.Fade(SceneDescription.FromIndex(0), Fade.Create(0.75f), Fade.Create(0.75f));
            }
            else if (FlaiInput.IsNewKeyPress(KeyCode.Backspace))
            {
                SceneFader.Fade(SceneDescription.FromIndex(Application.loadedLevel), Fade.Create(0.15f), Fade.Create(0.15f));
            }
            else if (FlaiInput.IsNewKeyPress(KeyCode.PageDown))
            {
                SceneFader.Fade(SceneDescription.FromIndex(Application.loadedLevel - 1), Fade.Create(0.15f), Fade.Create(0.15f));
            }
            else if (FlaiInput.IsNewKeyPress(KeyCode.PageUp))
            {
                SceneFader.Fade(SceneDescription.FromIndex((Application.loadedLevel == Application.levelCount - 1) ? 0 : Application.loadedLevel + 1), Fade.Create(0.15f), Fade.Create(0.15f));
            }
        }
    }
}
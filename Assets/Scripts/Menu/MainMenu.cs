using Flai;
using Flai.Scene;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class MainMenu : FlaiScript
    {
        public GUISkin GUISkin;
        protected void OnGUI()
        {
            this.DrawTitle();
            const float ButtonSize = 80;
            const float ButtonMargin = 80;
            int levelCount = Application.levelCount - 1; // first is main menu

            float totalWidth = levelCount * ButtonSize + (levelCount - 1) * ButtonMargin;

            float currentY = Screen.height / 2f - ButtonSize / 2f;
            float currentX = Screen.width / 2f - totalWidth / 2f;
            for (int i = 0; i < levelCount; i++)
            {
                int level = i + 1;
                GUI.backgroundColor = new Color32(48, 48, 48, 255);
                GUI.depth = int.MaxValue;
                if (GUI.Button(new Rect(currentX, currentY, ButtonSize, ButtonSize), level.ToString(), GUISkin.button))
                {
                    SceneFader.Fade(SceneDescription.FromIndex(level), Fade.Create(0.75f), Fade.Create(0.75f));
                }

                currentX += ButtonSize + ButtonMargin;
            }
        }

        private void DrawTitle()
        {
            GUI.Label(RectangleF.CreateCentered(new Vector2f(Screen.width / 2f, Screen.height / 4f), new SizeF(550, 80)), "Level Selection", GUISkin.textArea);
        }
    }
}
using Flai;
using Flai.Tween;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class Fade
    {
        public static readonly ColorF DefaultColor = ColorF.Black;
        public static readonly TweenType DefaulTween = TweenType.Linear;

        public float Time { get; private set; }
        public ColorF Color { get; private set; }
        public TweenType TweenType { get; private set; }

        public static Fade Create(float time = 0.5f)
        {
            return Fade.Create(time, Fade.DefaulTween, Fade.DefaultColor);
        }

        public static Fade Create(float time, TweenType tweenType)
        {
            return Fade.Create(time, tweenType, Fade.DefaultColor);
        }

        public static Fade Create(float time, TweenType tweenType, ColorF color)
        {
            return new Fade { Time = time, TweenType = tweenType, Color = color };
        }
    }

    public class SceneDescription
    {
        private int _levelIndex;
        private string _levelName;

        public void Load()
        {
            if (_levelName != null)
            {
                Application.LoadLevel(_levelName);
            }
            else
            {
                Application.LoadLevel(_levelIndex);
            }
        }

        public static SceneDescription FromIndex(int index)
        {
            Ensure.WithinRange(index, 0, Application.levelCount - 1);
            return new SceneDescription { _levelIndex = index };
        }

        public static SceneDescription FromName(string name)
        {
            Ensure.NotNull(name);
            return new SceneDescription { _levelName = name };
        }
    }
}

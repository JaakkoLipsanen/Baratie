
namespace Assets.Game.Model
{
    public enum GameDimension
    {
        Both,
        White,
        Black,
    }

    public static class GameDimensionHelper
    {
        public static GameDimension Opposite(this GameDimension gameDimension)
        {
            switch (gameDimension)
            {
                case GameDimension.Black:
                    return GameDimension.White;

                case GameDimension.White:
                    return GameDimension.Black;

                default:
                    return GameDimension.Both;
            }
        }
    }
}

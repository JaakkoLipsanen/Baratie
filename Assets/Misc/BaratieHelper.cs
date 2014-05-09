using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Misc
{
    public static class BaratieHelper
    {
        public static bool IsPlayer(GameObject gameObject)
        {
            return gameObject.Has<PlayerInfo>();
        }
    }
}

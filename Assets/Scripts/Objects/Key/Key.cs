using Assets.Scripts.Player;
using Flai;
using UnityEngine;

namespace Assets.Scripts.Objects
{
	public class Key : FlaiScript
	{
	    protected override void OnTriggerEnter2D(Collider2D other)
	    {
	        var playerInfo = other.Get<PlayerInfo>();
            if (playerInfo != null)
            {
                playerInfo.Manager.AddKey();
                this.DestroyGameObject();
            }
	    }
	}
}
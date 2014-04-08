﻿using Assets.Scripts.Menu;
using Assets.Scripts.Player;
using Flai;
using Flai.Tween;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class Goal : FlaiScript
	{
        // todo todo: iTween & tweening!!!!
	    private GameObject _exitingGameObject;
	    private float _timeSinceExitStarted = 0f;

	    private bool IsExiting
	    {
	        get { return _exitingGameObject != null; }
	    }

	    protected override void Update()
	    {
	        if (this.IsExiting)
	        {
	            _timeSinceExitStarted += Time.deltaTime;
               // _exitingGameObject.SetScale2D();
	        }
	    }

	    protected override void OnTriggerEnter2D(Collider2D other)
	    {
	        if (this.IsExiting)
	        {
	            return;
	        }

	        if (other.gameObject.Has<PlayerInfo>())
	        {
	            _exitingGameObject = other.gameObject;

                Tween.Scale(other.gameObject, Vector3.zero, 0.5f).SetEase(TweenType.EaseInBack);
	            Tween.Move(other.gameObject, this.gameObject.transform.position - Vector3.up, 0.5f).SetEase(TweenType.EaseInBack);

	            int nextLevel = Application.loadedLevel + 1;
	            if (nextLevel == Application.levelCount) // this is the last level, go back to main menu
	            {
	                nextLevel = 0;
	            }

                SceneFader.Fade(SceneDescription.FromIndex(nextLevel), Fade.Create(0.75f), Fade.Create(0.75f), 0.5f);
	        }
	    }
	}
}
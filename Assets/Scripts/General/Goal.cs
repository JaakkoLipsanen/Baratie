using System.Linq;
using Assets.Scripts.Objects;
using Assets.Scripts.Player;
using Flai;
using Flai.Scene;
using Flai.Tween;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class Goal : FlaiScript
	{
        // todo todo: iTween & tweening!!!!
	    private GameObject _exitingGameObject;
	    private Key[] _keys;
	    private bool _isAllKeysPicked = false;

	    private bool IsExiting
	    {
	        get { return _exitingGameObject != null; }
	    }

	    protected override void Awake()
        {
            _keys = Scene.FindAllOfType<Key>();
	        if (_keys == null)
	        {
                _isAllKeysPicked = true;
                this.Parent.GetChild("Particle Emitter").particleSystem.Play();
	        }
	    }

	    protected override void Update()
	    {
	        if (_keys.All(key => key == null) && !_isAllKeysPicked)
	        {
	            this.Parent.GetChild("Particle Emitter").particleSystem.Play();
	            _isAllKeysPicked = true;
	        }
	    }

	    protected override void OnTriggerEnter2D(Collider2D other)
	    {
	        if (this.IsExiting || !_isAllKeysPicked)
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
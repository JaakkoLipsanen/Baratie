using Flai;
using Flai.Diagnostics;
using UnityEngine;

namespace Assets.Scripts.Rendering
{
    [ExecuteInEditMode]
    public class SetRendererSortingOrder : FlaiScript
    {
        public int SortingOrder = 0;
        public bool SetEveryFrame = false;
        protected override void Awake()
        {
            var renderer = this.renderer;
            if (renderer == null)
            {
                FlaiDebug.LogWarningWithTypeTag<SetRendererSortingOrder>("Renderer is null");
            }
            else
            {
                renderer.sortingOrder = this.SortingOrder;
            }
        }

        protected override void Update()
        {
            var renderer = this.renderer;
            if (this.SetEveryFrame && renderer != null)
            {
                renderer.sortingOrder = this.SortingOrder;
            }
        }
    }
}
using Flai;
using Flai.Diagnostics;

namespace Assets.Scripts.Rendering
{
	public class SetRendererSortingOrder : FlaiScript
	{
	    public int SortingOrder = 0;
		protected override void Awake ()
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
	
		protected override void Update () 
		{
		}
	}
}
using Flai;

namespace Assets.Scripts.General
{
	public abstract class Response : FlaiScript
	{
        public abstract void ExecuteOn(object context);
        public abstract void ExecuteOff(object context);
	}
}
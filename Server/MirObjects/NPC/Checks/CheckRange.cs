using System.Drawing;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKRANGE")]
	public class CheckRange : NPCCheck
	{
		protected readonly int X, Y, Range;
		public CheckRange(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[1], out X) ||
				!int.TryParse(parts[2], out Y) ||
				!int.TryParse(parts[3], out Range))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			return InitializationSuccess && Functions.InRange(ob.CurrentLocation, new Point(X, Y), Range);
		}
	}
}
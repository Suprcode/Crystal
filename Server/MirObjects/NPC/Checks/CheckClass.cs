using System;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKCLASS")]
	public class CheckClass : NPCCheck
	{
		protected readonly MirClass RequiredClass;
		public CheckClass(string line, string[] parts) : base(line, parts)
		{
			if (!Enum.TryParse(parts[1], out RequiredClass))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;

			switch (ob)
			{
				case PlayerObject player:
					return player.Class == RequiredClass;
			}
			return false;
		}
	}
}
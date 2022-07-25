using System;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKGENDER")]
	public class CheckGender : NPCCheck
	{
		protected readonly MirGender RequiredGender;
		public CheckGender(string line, string[] parts) : base(line, parts)
		{
			if (!Enum.TryParse(parts[1], true, out RequiredGender))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					return player.Gender == RequiredGender;
			}
			return false;
		}
	}
}
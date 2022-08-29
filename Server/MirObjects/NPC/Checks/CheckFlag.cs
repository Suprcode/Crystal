using System;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECK")]
	public class CheckFlag : NPCCheck
	{
		protected readonly ushort FlagIndex;
		protected readonly bool FlagOn;
		public CheckFlag(string line, string[] parts) : base(line, parts)
		{
			var match = FlagRegex.Match(parts[1]);
			if (!match.Success ||
				!ushort.TryParse(match.Groups[1].Captures[0].Value, out FlagIndex) ||
				!uint.TryParse(parts[2], out var check) ||
				FlagIndex > Globals.FlagIndexCount)
				return;
			FlagOn = Convert.ToBoolean(check);
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					return player.Info.Flags[FlagIndex] == FlagOn;
			}
			return false;
		}
	}
}
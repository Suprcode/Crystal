using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SET")]
	public class ActionSet : NPCAction
	{
		protected readonly int FlagIndex;
		protected readonly bool FlagOn;
		public ActionSet(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = FlagRegex.Match(parts[1]);
			if (!match.Success)
				return;
			if (!int.TryParse(match.Groups[1].Captures[0].Value, out FlagIndex))
				return;
			if (FlagIndex < 0 || FlagIndex >= Globals.FlagIndexCount)
				return;
			if (!uint.TryParse(parts[2], out var onCheck))
				return;
			FlagOn = Convert.ToBoolean(onCheck);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.Info.Flags[FlagIndex] = FlagOn;
					for (int i = player.CurrentMap.NPCs.Count - 1; i >= 0; i--)
					{
						if (!Functions.InRange(player.CurrentMap.NPCs[i].CurrentLocation, player.CurrentLocation, Globals.DataRange)) continue;
						player.CurrentMap.NPCs[i].CheckVisible(player);
					}
					if (FlagOn)
						player.CheckNeedQuestFlag(FlagIndex);
					break;
			}
		}
	}
}
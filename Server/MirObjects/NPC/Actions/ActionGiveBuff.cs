using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEBUFF")]
	public class ActionGiveBuff : NPCAction
	{
		protected readonly bool Visible;
		protected readonly bool Infinity;
		protected readonly bool Stackable;
		protected readonly BuffType BuffType;
		protected readonly int Duration;
		public ActionGiveBuff(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length > 3 &&
				!bool.TryParse(parts[3], out Visible))
				return;
			if (parts.Length > 4 &&
				!bool.TryParse(parts[4], out Infinity))
				return;
			if (parts.Length > 5 &&
				!bool.TryParse(parts[5], out Stackable))
				return;
			if (!int.TryParse(parts[2], out Duration))
				return;
			if (!Enum.TryParse(parts[1], true, out BuffType))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.AddBuff(BuffType, player, Settings.Second * Duration, new Stats(), Visible);
					break;
			}
		}
	}
}
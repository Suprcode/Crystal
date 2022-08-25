using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CHANGEHAIR")]
	public class ActionChangeHair : NPCAction
	{
		protected readonly byte Hair = 0;
		protected readonly bool RandomHair = false;
		public ActionChangeHair(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			RandomHair = parts.Length == 1;
			if (RandomHair)
			{
				InitializationSuccess = true;
				return;
			}
			if (!byte.TryParse(parts[1], out Hair))
				return;
			Hair = Math.Max(Math.Min(Hair, (byte)9), (byte)0);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			switch (ob)
			{
				case PlayerObject player:
					player.Info.Hair = RandomHair ? (byte) Random.Next(0, 9) : Hair;
					break;
			}
		}
	}
}
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SETTIMER")]
	public class ActionSetTimer : NPCAction
	{
		protected readonly string Key;
		protected readonly bool IsGlobal;
		protected readonly int Duration;
		protected readonly byte Type;
		public ActionSetTimer(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			Key = parts[1];
			if (!int.TryParse(parts[2], out Duration) ||
				!byte.TryParse(parts[3], out Type))
				return;
			IsGlobal = parts.Length >= 5;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			if (IsGlobal)
			{
				var timerKey = $"_{Key}";
				Envir.Timers[timerKey] = new Timer(timerKey, Duration, Type);
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					player.SetTimer(Key, Duration, Type);
					break;
			}
		}
	}
}
using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKTIMER")]
	public class CheckTimer : NPCCheck
	{
		protected readonly long Duration;
		protected readonly string GlobalTimerKey;
		protected readonly string PlayerTimerKey;
		protected readonly string Operator;
		protected Timer Timer;
		
		public CheckTimer(string line, string[] parts) : base(line, parts)
		{
			if (!long.TryParse(parts[3], out Duration))
				return;
			GlobalTimerKey = $"_-{parts[1]}";
			PlayerTimerKey = parts[1];
			Operator = parts[2];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			if (Timer is null && 
				Envir.Timers.ContainsKey(GlobalTimerKey))
				Timer = Envir.Timers[GlobalTimerKey];
			if (!(Timer is null))
			{
				var remaining = (Timer.RelativeTime - Envir.Time) / 1000;
				return Compare(Operator, remaining, Duration);
			}
			switch (ob)
			{
				case PlayerObject player:
					Timer = player.GetTimer(PlayerTimerKey);
					if (Timer is null) return false;
					var remaining = (Timer.RelativeTime - Envir.Time) / 1000;
					return Compare(Operator, remaining, Duration);
			}
			return false;
		}
	}
}
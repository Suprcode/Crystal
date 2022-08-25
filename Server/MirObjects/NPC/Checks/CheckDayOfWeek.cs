using System;
namespace Server.MirObjects.Checks
{
	[CheckCommand("DAYOFWEEK")]
	public class CheckDayOfWeek : NPCCheck
	{
		protected readonly DayOfWeek RequiredDay;
		public CheckDayOfWeek(string line, string[] parts) : base(line, parts)
		{
			if (!Enum.TryParse(parts[1], true, out RequiredDay))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;

			return Envir.Now.DayOfWeek == RequiredDay;
		}
	}
}
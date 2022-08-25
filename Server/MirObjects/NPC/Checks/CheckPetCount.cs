﻿namespace Server.MirObjects.Checks
{
	[CheckCommand("PETCOUNT")]
	public class CheckPetCount : NPCCheck
	{
		protected readonly int RequiredAmount;
		protected readonly string Operator;
		public CheckPetCount(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[2], out RequiredAmount))
				return;
			Operator = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					return Compare(Operator, player.Pets.Count, RequiredAmount);
			}
			return false;
		}
	}
}
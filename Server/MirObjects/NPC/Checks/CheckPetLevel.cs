namespace Server.MirObjects.Checks
{
	[CheckCommand("PETLEVEL")]
	public class CheckPetLevel : NPCCheck
	{
		protected readonly int RequiredLevel;
		protected readonly string Operator;
		public CheckPetLevel(string line, string[] parts) : base(line, parts)
		{
			if (!int.TryParse(parts[2], out RequiredLevel))
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
				{
					var success = false;
					for (int i = 0; i < player.Pets.Count; i++)
					{
						success = Compare(Operator, player.Pets[i].PetLevel, RequiredLevel);
						if (!success)
							break;
					}
					return success;
				}
			}
			return false;
		}
	}
}
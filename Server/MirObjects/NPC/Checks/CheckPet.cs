namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKPET")]
	public class CheckPet : NPCCheck
	{
		protected readonly string PetName;
		public CheckPet(string line, string[] parts) : base(line, parts)
		{
			PetName = parts[1];
			if (string.IsNullOrEmpty(PetName))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
				{
					var petMatch = false;
					for (int i = player.Pets.Count - 1; i >= 0; i--)
					{
						if (string.Compare(player.Pets[i].Info.Name, PetName, true) != 0)
							continue;
						petMatch = true;
						//Optimized, don't check for the pet's name again.
						break;
					}
					return petMatch;
				}
			}
			return false;
		}
	}
}
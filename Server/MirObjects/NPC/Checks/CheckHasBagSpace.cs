namespace Server.MirObjects.Checks
{
	[CheckCommand("HASBAGSPACE")]
	public class CheckHasBagSpace : NPCCheck
	{
		protected readonly int RequiredAmount;
		protected readonly string Operator;
		public CheckHasBagSpace(string line, string[] parts) : base(line, parts)
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
				{
					var freeSlotCount = 0;
					for (var i = 0; i < player.Info.Inventory.Length; i++)
					{
						if (player.Info.Inventory[i] is null)
							freeSlotCount++;
					}
					return Compare(Operator, freeSlotCount, RequiredAmount);
				}
			}
			return false;
		}
	}
}
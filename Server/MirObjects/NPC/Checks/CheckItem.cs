namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKITEM")]
	public class CheckItem : NPCCheck
	{
		protected readonly ItemInfo RequiredItem;
		protected readonly uint RequiredAmount;
		protected readonly ushort RequiredDura = 0;
		protected readonly bool RequiresDura = false;
		public CheckItem(string line, string[] parts) : base(line, parts)
		{
			if (parts.Length < 3)
				RequiredAmount = 1;
			else if (!uint.TryParse(parts[2], out RequiredAmount))
				return;
			RequiresDura = parts.Length > 3 && ushort.TryParse(parts[3], out RequiredDura);
			RequiredItem = Envir.GetItemInfo(parts[1]);
			if (RequiredItem is null)
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
					var count = RequiredAmount;
					for (var i = 0; i < player.Info.Inventory.Length; i++)
					{
						var playersItem = player.Info.Inventory[i];
						if (playersItem is null)
							continue;
						if (RequiresDura &&
							playersItem.CurrentDura < (RequiredDura * 1000))
							continue;
						if (count > playersItem.Count)
						{
							count -= playersItem.Count;
							continue;
						}
						if (count > playersItem.Count) continue;
						count = 0;
					}
					return count == 0;
				}
			}
			return false;
		}
	}
}
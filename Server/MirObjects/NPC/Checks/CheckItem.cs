namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKITEM")]
	public class CheckItem : NPCCheck
	{
		protected ItemInfo RequiredItem;
		protected readonly string ItemName;
		protected readonly uint RequiredAmount;
		protected readonly ushort RequiredDura = 0;
		protected readonly bool RequiresDura = false;
		public CheckItem(string line, string[] parts) : base(line, parts)
		{
			ItemName = parts[1];
			if (parts.Length > 2)
			{
				if (!uint.TryParse(parts[2], out RequiredAmount))
					return;
			}
			else
				RequiredAmount = 1;
			if (parts.Length > 3)
			{
				if (!ushort.TryParse(parts[3], out RequiredDura))
					return;
				RequiresDura = true;
			}
			RequiredItem = Envir.GetItemInfo(ItemName);
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) 
				return false;
			RequiredItem ??= Envir.GetItemInfo(ItemName);
			if (RequiredItem is null)
			{
				InitializationSuccess = false;
				return false;
			}
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
						if (playersItem.Info.Index != RequiredItem.Index)
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
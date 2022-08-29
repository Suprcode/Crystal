namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEITEM")]
	public class ActionGiveItem : NPCAction
	{
		protected ItemInfo Item;
		protected readonly string ItemName;
		protected readonly ushort Count;
		public ActionGiveItem(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length < 3)
				Count = 1;
			else if (!ushort.TryParse(parts[2], out Count))
				return;
			ItemName = parts[1];
			Item = Envir.GetItemInfo(ItemName);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess)
				return;
			Item ??= Envir.GetItemInfo(ItemName);
			if (Item is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					var count = Count;
					while (count > 0)
					{
						var item = Envir.CreateFreshItem(Item);
						if (item is null)
							break;
						if (item.Info.StackSize > count)
						{
							item.Count = count;
							count = 0;
						}
						else
						{
							count -= item.Info.StackSize;
							item.Count = item.Info.StackSize;
						}
						if (player.CanGainItem(item, false))
							player.GainItem(item);
					}
					break;
			}
		}
	}
}
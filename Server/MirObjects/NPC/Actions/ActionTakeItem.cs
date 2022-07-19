using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKEITEM")]
	public class ActionTakeItem : NPCAction
	{
		protected readonly string ItemName;
		protected readonly ushort Count;
		protected readonly ushort Dura;
		protected readonly bool CheckDura;
		protected ItemInfo Item;
		public ActionTakeItem(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			ItemName = parts[1];
			if (parts.Length <= 2 || !ushort.TryParse(parts[2], out Count))
				Count = 1;
			CheckDura = parts.Length > 3 && ushort.TryParse(parts[3], out Dura);
			Item = Envir.GetItemInfo(ItemName);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
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
					for (var i = 0; i < player.Info.Inventory.Length; i++)
					{
						var item = player.Info.Inventory[i];
						if (item is null) continue;
						if (item.Info != Item) continue;
						if (CheckDura &&
							item.CurrentDura < (Dura * 1000)) continue;
						if (count > item.Count)
						{
							player.Enqueue(new DeleteItem
							{
								UniqueID = item.UniqueID,
								Count = item.Count
							});
							count -= item.Count;
							continue;
						}
						player.Enqueue(new DeleteItem
						{
							UniqueID = item.UniqueID,
							Count = count
						});
						if (count == item.Count)
							player.Info.Inventory[i] = null;
						else
							item.Count -= count;
						break;
					}
					player.RefreshStats();
					break;
			}
		}
	}
}
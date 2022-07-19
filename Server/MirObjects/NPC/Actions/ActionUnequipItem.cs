using System;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("UNEQUIPITEM")]
	public class ActionUnequipItem : NPCAction
	{
		protected readonly EquipmentSlot Slot;
		public ActionUnequipItem(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!Enum.TryParse(parts[1], out Slot))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var playerSlot = player.Info.Equipment[(int)Slot];
					if (playerSlot is null) return;
					if (!player.CanRemoveItem(MirGridType.Inventory, playerSlot)) return;
					if (playerSlot.Cursed) return;
					if (playerSlot.WeddingRing != -1) return;
					for (var i = 0; i < player.Info.Inventory.Length; i++)
					{
						var currentSlot = player.Info.Inventory[i];
						if (!(currentSlot is null)) continue;
						player.Info.Equipment[(int)Slot] = null;
						player.Info.Inventory[i] = playerSlot;
						player.Report.ItemMoved(playerSlot, MirGridType.Equipment, MirGridType.Inventory, (int) Slot, i);
					}
					var packet = new UserSlotsRefresh
					{
						Inventory = new UserItem[player.Info.Inventory.Length],
						Equipment = new UserItem[player.Info.Equipment.Length]
					};
					player.Info.Inventory.CopyTo(packet.Inventory, 0);
					player.Info.Equipment.CopyTo(packet.Equipment, 0);
					player.Enqueue(packet);
					player.RefreshStats();
					break;
			}
		}
	}
}
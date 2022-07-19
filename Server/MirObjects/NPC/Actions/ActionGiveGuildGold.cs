using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEGUILDGOLD")]
	public class ActionGiveGuildGold : NPCAction
	{
		protected readonly uint Amount;
		public ActionGiveGuildGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!uint.TryParse(parts[1], out Amount))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null)
						return;
					var gold = Amount;
					if (gold + player.MyGuild.Gold >= uint.MaxValue)
						gold = uint.MaxValue - player.MyGuild.Gold;
					player.MyGuild.Gold += gold;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 3,
						Amount = gold
					});
					break;
			}
		}
	}
}
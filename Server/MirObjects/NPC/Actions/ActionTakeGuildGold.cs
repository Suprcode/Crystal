using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("TAKEGUILDGOLD")]
	public class ActionTakeGuildGold : NPCAction
	{
		protected readonly uint Amount;
		public ActionTakeGuildGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
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
					if (player.MyGuild is null) return;
					var gold = Amount;
					if (gold >= player.MyGuild.Gold)
						gold = player.MyGuild.Gold;
					player.MyGuild.Gold -= gold;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 2,
						Amount = gold
					});
					break;
			}
		}
	}
}
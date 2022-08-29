using System.Linq;
using Server.MirDatabase;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CONQUESTGUARD")]
	public class ActionConquestGuard : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly int GuardIndex;
		protected ConquestObject Conquest;
		protected ConquestGuildArcherInfo Guard;
		public ActionConquestGuard(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GuardIndex))
				return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			InitializationSuccess = true;
			if (Conquest == null)
				return;
			Guard = Conquest.ArcherList.FirstOrDefault(guard => guard.Info.Index == GuardIndex);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Conquest ??= Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			if (Conquest is null)
			{
				InitializationSuccess = false;
				return;
			}
			Guard ??= Conquest.ArcherList.FirstOrDefault(guard => guard.Info.Index == GuardIndex);
			if (Guard is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return;
					if (player.MyGuild.Gold < Guard.GetRepairCost()) return;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 2,
						Amount = Guard.GetRepairCost()
					});
					player.MyGuild.Gold -= Guard.GetRepairCost();
					break;
			}
		}
	}
}
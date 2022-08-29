using System.Linq;
using Server.MirDatabase;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("CONQUESTGATE")]
	public class ActionConquestGate : NPCAction
	{
		protected readonly int ConquestIndex;
		protected readonly int GuardIndex;
		protected ConquestObject Conquest;
		protected ConquestGuildGateInfo Gate;
		public ActionConquestGate(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out ConquestIndex) ||
				!int.TryParse(parts[2], out GuardIndex))
				return;
			Conquest = Envir.Conquests.FirstOrDefault(conquest => conquest.Info.Index == ConquestIndex);
			InitializationSuccess = true;
			if (Conquest == null)
				return;
			Gate = Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GuardIndex);
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
			Gate ??= Conquest.GateList.FirstOrDefault(gate => gate.Info.Index == GuardIndex);
			if (Gate is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					if (player.MyGuild is null) return;
					if (player.MyGuild.Gold < Gate.GetRepairCost()) return;
					player.MyGuild.SendServerPacket(new GuildStorageGoldChange
					{
						Type = 2,
						Amount = (uint) Gate.GetRepairCost()
					});
					player.MyGuild.Gold -= (uint)Gate.GetRepairCost();
					break;
			}
		}
	}
}
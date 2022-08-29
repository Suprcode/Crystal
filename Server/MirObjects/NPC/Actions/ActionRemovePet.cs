using System;
namespace Server.MirObjects.Actions
{
	[ActionCommand("REMOVEPET")]
	public class ActionRemovePet : NPCAction
	{
		protected readonly string MobName;
		protected bool MobChecked = false;
		public ActionRemovePet(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			MobName = parts[1];
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			if (!MobChecked)
			{
				if (Envir.GetMonsterInfo(MobName) is null)
				{
					InitializationSuccess = false;
					return;
				}
				MobChecked = true;
			}
			switch (ob)
			{
				case PlayerObject player:
					for (var i = player.Pets.Count - 1; i >= 0; i--)
					{
						if (string.Compare(player.Pets[i].Info.Name, MobName, StringComparison.OrdinalIgnoreCase) != 0) continue;
						player.Pets[i].Die();
					}
					break;
			}
		}
	}
}
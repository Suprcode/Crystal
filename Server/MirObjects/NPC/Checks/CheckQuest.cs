using System.Linq;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKQUEST")]
	public class CheckQuest : NPCCheck
	{
		protected readonly bool RequireActive;
		protected readonly int QuestIndex;
		public CheckQuest(string line, string[] parts) : base(line, parts)
		{
			if (parts.Length < 3) return;
			if (!int.TryParse(parts[1], out QuestIndex))
				return;
			RequireActive = parts[2].ToUpper() == "ACTIVE";
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					if (RequireActive)
						return player.CurrentQuests.Any(e => e.Index == QuestIndex);
					return player.CompletedQuests.Contains(QuestIndex);
			}
			return false;
		}
	}
}
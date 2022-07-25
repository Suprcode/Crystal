using System.Collections.Generic;
using System.IO;
using Server.MirDatabase;
namespace Server.MirObjects.Actions
{
	[ActionCommand("DROP")]
	public class ActionDrop : NPCAction
	{
		protected readonly string FileName;
		public ActionDrop(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var match = QuoteRegex.Match(line);
			var listPath = parts[1];
			if (match.Success)
				listPath = match.Groups[1].Captures[0].Value;
			FileName = Path.Combine(Settings.DropPath, listPath);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			
			switch (ob)
			{
				case PlayerObject player:
					var drops = new List<DropInfo>();
					DropInfo.Load(drops, "NPC", FileName, 0, false);
					foreach (var drop in drops)
					{
						var reward = drop.AttemptDrop(player?.Stats[Stat.ItemDropRatePercent] ?? 0, player?.Stats[Stat.GoldDropRatePercent] ?? 0);
						if (reward is null) continue;
						if (reward.Gold > 0)
							player.GainGold(reward.Gold);
						foreach (var rewardItem in reward.Items)
						{
							var item = Envir.CreateDropItem(rewardItem);
							if (item is null) continue;
							player.CheckGroupQuestItem(item);
							if (drop.QuestRequired) continue;
							if (!player.CanGainItem(item)) 
								continue;
							player.GainItem(item);
						}
					}
					break;
			}
			
		}
	}
}
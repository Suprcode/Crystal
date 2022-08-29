using System.IO;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKGUILDNAMELIST")]
	public class CheckGuildNameList : NPCCheck
	{
		protected readonly string FileName;
		public CheckGuildNameList(string line, string[] parts) : base(line, parts)
		{
			var quoteMatch = QuoteRegex.Match(Line);
			var listPath = parts[1];
			if (quoteMatch.Success)
				listPath = quoteMatch.Groups[1].Captures[0].Value;
			FileName = Path.Combine(Settings.NameListPath, listPath);
			var sDirectory = Path.GetDirectoryName(FileName);
			if (!Directory.Exists(sDirectory))
				Directory.CreateDirectory(sDirectory);
			InitializationSuccess = File.Exists(FileName);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject player:
					var read = File.ReadAllText(FileName);
					return !(player.MyGuild is null) && read.Contains(player.MyGuild.Name);
			}
			return false;
		}
	}
}
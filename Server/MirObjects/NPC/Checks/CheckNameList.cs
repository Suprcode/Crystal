using System.IO;
using System.Linq;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKNAMELIST")]
	public class CheckNameList : NPCCheck
	{
		protected readonly string FileName;
		public CheckNameList(string line, string[] parts) : base(line, parts)
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
			var read = File.ReadAllLines(FileName);
			switch (ob)
			{
				case PlayerObject player:
					return read.Contains(player.Name);
			}
			return false;
		}
	}
}
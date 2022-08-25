using System.IO;
namespace Server.MirObjects.Actions
{
	[ActionCommand("LOADVALUE")]
	public class ActionLoadValue : NPCAction
	{
		protected readonly string Value;
		protected readonly string FileName;
		protected readonly string Header;
		protected readonly string Key;
		public ActionLoadValue(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var quoteMatch = QuoteRegex.Match(line);
			if (!quoteMatch.Success) return;
			FileName = Path.Combine(Settings.ValuePath, quoteMatch.Groups[1].Captures[0].Value);
			Header = parts[parts.Length - 2];
			Key = parts[parts.Length - 1];
			var sDirectory = Path.GetDirectoryName(FileName);
			if (!Directory.Exists(sDirectory))
				Directory.CreateDirectory(sDirectory);
			if (!File.Exists(FileName))
				File.Create(FileName).Close();
			
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					var reader = new InIReader(FileName);
					var loadedString = reader.ReadString(Header, Key, "");
					if (string.IsNullOrEmpty(loadedString)) return;
					AddVariable(player, Value, loadedString);
					break;
			}
		}
	}
}
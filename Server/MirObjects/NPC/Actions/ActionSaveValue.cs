using System.IO;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SAVEVALUE")]
	public class ActionSaveValue : NPCAction
	{
		protected readonly string FileName;
		protected readonly string Header;
		protected readonly string Key;
		protected readonly string Value;
		public ActionSaveValue(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			var matchCol = QuoteRegex.Matches(line);
			if (matchCol.Count > 0 && matchCol[0].Success)
			{
				FileName = Path.Combine(Settings.ValuePath, matchCol[0].Groups[1].Captures[0].Value);
				Value = parts[parts.Length - 1];
				if (matchCol.Count > 1 && matchCol[1].Success)
					Value = matchCol[1].Groups[1].Captures[0].Value;
				var newParts = line.Replace(Value, string.Empty).Replace("\"", "").Trim().Split(' ');
				Header = newParts[newParts.Length - 2];
				Key = newParts[newParts.Length - 1];
				var sDirectory = Path.GetDirectoryName(FileName);
				if (!Directory.Exists(sDirectory))
					Directory.CreateDirectory(sDirectory);
				if (!File.Exists(FileName))
					File.Create(FileName).Close();
			}
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			var reader = new InIReader(FileName);
			reader.Write(Header, Key, Value);
		}
	}
}
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ADDMAILGOLD")]
	public class ActionAddMailGold : NPCAction
	{
		protected readonly uint Amount;
		public MailInfo MailInfo { get; protected set; }
		public ActionAddMailGold(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!uint.TryParse(parts[0], out Amount))
				return;
			InitializationSuccess = true;
		}
		
		public void SetMailInfo(MailInfo info)
		{
			MailInfo = info;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			if (MailInfo is null) return;
			MailInfo.Gold += Amount;
		}
	}
}
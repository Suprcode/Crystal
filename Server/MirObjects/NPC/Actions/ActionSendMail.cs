using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("SENDMAIL")]
	public class ActionSendMail : NPCAction
	{
		public MailInfo MailInfo { get; protected set; }
		public ActionSendMail(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
		}
		public void SetMailInfo(MailInfo info)
		{
			MailInfo = info;
		}
		public override void Execute(MapObject ob)
		{
			if (MailInfo is null)
				return;
			MailInfo.Send();
			MailInfo = null;
		}
	}
}
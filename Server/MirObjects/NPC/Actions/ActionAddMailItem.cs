using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("ADDMAILITEM")]
	public class ActionAddMailItem : NPCAction
	{
		protected readonly string ItemName;
		protected readonly ushort Count;
		public MailInfo MailInfo { get; protected set; }
		protected ItemInfo Item;
		public ActionAddMailItem(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			ItemName = parts[1];
			if (parts.Length <= 2 || ushort.TryParse(parts[2], out Count))
				Count = 1;
			Item = Envir.GetItemInfo(ItemName);
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
			if (MailInfo.Items.Count >= 5) return;
			Item ??= Envir.GetItemInfo(ItemName);
			if (Item is null)
			{
				InitializationSuccess = false;
				return;
			}
			var count = Count;
			while (count > 0 && MailInfo.Items.Count < 5)
			{
				var item = Envir.CreateFreshItem(Item);
				if (item is null)
				{
					MessageQueue.Enqueue($"Failed to create UserItem: {ItemName} Page: {Page.Key}");
					return;
				}
				if (item.Info.StackSize > count)
				{
					item.Count = count;
					count = 0;
				}
				else
				{
					count -= item.Info.StackSize;
					item.Count = item.Info.StackSize;
				}
				MailInfo.Items.Add(item);
			}
		}
	}
}
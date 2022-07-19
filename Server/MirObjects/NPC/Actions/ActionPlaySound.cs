using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("PLAYSOUND")]
	public class ActionPlaySound : NPCAction
	{
		protected readonly int SoundIndex;
		public ActionPlaySound(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!int.TryParse(parts[1], out SoundIndex))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					player.Enqueue(new PlaySound
					{
						Sound = SoundIndex
					});
					break;
			}
		}
	}
}
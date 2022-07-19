using System;
using System.Linq;
using ServerPackets;
namespace Server.MirObjects.Actions
{
	[ActionCommand("REMOVESKILL")]
	public class ActionRemoveSkill : NPCAction
	{
		protected readonly Spell Spell;
		public ActionRemoveSkill(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!Enum.TryParse(parts[1], out Spell))
				return;
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject playerObject:
					if (playerObject.Info.Magics.All(spell => spell.Spell != Spell))
						return;
					for (int i = playerObject.Info.Magics.Count - 1; i >= 0; i--)
					{
						if (playerObject.Info.Magics[i].Spell != Spell) continue;
						playerObject.Info.Magics.RemoveAt(i);
						playerObject.Enqueue(new RemoveMagic
						{
							PlaceId = i
						});
						break;
					}
					break;
			}
		}
	}
}
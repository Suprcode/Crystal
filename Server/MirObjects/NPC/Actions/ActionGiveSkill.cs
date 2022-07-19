using System;
using System.Linq;
using Server.MirDatabase;
namespace Server.MirObjects.Actions
{ 
	[ActionCommand("GIVESKILL")]
	public class ActionGiveSkill : NPCAction
	{
		protected readonly Spell Spell;
		protected readonly byte Level;
		public ActionGiveSkill(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (!Enum.TryParse(parts[1], out Spell))
				return;
			if (parts.Length <= 2)
			{
				Level = 0;
				InitializationSuccess = true;
				return;
			}
			if (!byte.TryParse(parts[2], out Level))
				return;
			InitializationSuccess = true;

		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			switch (ob)
			{
				case PlayerObject player:
					if (player.Info.Magics.Any(spell => spell.Spell == Spell))
						return;
					var magic = new UserMagic(Spell) { Level = Level };
					if (magic.Info is null) return;
					player.Info.Magics.Add(magic);
					player.SendMagicInfo(magic);
					break;
			}
		}
	}
}
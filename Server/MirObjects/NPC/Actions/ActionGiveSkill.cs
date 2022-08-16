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
					int index = player.Info.Magics.FindIndex(spell => spell.Spell == Spell); // Get index of spell
					if (index > 0) // If the player already has the spell we need to check its level
					{
						if (player.Info.Magics[index].Level >= Level) return; // Return if user has higher spell level

						player.Info.Magics[index].Level = Level; // Set spell level to new level
						player.Enqueue(new S.MagicLeveled { ObjectID = player.ObjectID, Spell = Spell, Level = Level, Experience = 0 });
					}
					else // If the player doesn't have the spell, award it to them
					{
						var magic = new UserMagic(Spell) { Level = Level };
						if (magic.Info is null) return;
						player.Info.Magics.Add(magic);
						player.SendMagicInfo(magic);
					}
					break;
			}
		}
	}
}

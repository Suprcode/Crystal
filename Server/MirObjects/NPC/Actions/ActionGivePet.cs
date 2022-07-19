using System;
using Server.MirDatabase;
namespace Server.MirObjects.Actions
{
	[ActionCommand("GIVEPET")]
	public class ActionGivePet : NPCAction
	{
		protected readonly string MobName;
		protected MonsterInfo Monster;
		protected readonly byte Count;
		protected readonly byte Level;
		public ActionGivePet(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			MobName = parts[1];
			if (parts.Length > 3)
				Level = !byte.TryParse(parts[3], out Level) ? (byte)0 : Math.Min(Level, (byte)7);

			if (parts.Length > 2)
				Count = !byte.TryParse(parts[2], out Count) ? (byte)1 : Math.Min(Count, (byte)5);
			Monster = Envir.GetMonsterInfo(MobName);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Monster ??= Envir.GetMonsterInfo(MobName);
			if (Monster is null)
			{
				InitializationSuccess = false;
				return;
			}
			switch (ob)
			{
				case PlayerObject player:
					for (var i = 0; i < Count; i++)
					{
						var monster = MonsterObject.GetMonster(Monster);
						if (monster is null) return;
						monster.PetLevel = Level;
						monster.Master = player;
						monster.MaxPetLevel = 7;
						monster.Direction = player.Direction;
						monster.ActionTime = Envir.Time + 1000;
						monster.Spawn(player.CurrentMap, player.CurrentLocation);
						player.Pets.Add(monster);
					}
					break;
			}
		}
	}
}
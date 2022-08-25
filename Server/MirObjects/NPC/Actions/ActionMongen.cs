using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("MONGEN")]
	public class ActionMongen : NPCAction
	{
		protected MonsterInfo Monster;
		protected Map Map;
		protected readonly string MobName;
		protected readonly byte Count;
		public ActionMongen(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length > 2)
			{
				if (!byte.TryParse(parts[2], out Count))
					return;
			}
			else
				Count = 1;
			MobName = parts[1];
			Monster = Envir.GetMonsterInfo(MobName);
			InitializationSuccess = true;
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			if (Segment.Param1 is null ||
				Segment.Param2 == 0 ||
				Segment.Param3 == 0)
				return;
			Map ??= Envir.GetMapByNameAndInstance(Segment.Param1, Segment.Param1Instance);
			if (Map is null)
				return;
			Monster ??= Envir.GetMonsterInfo(MobName);
			if (Monster is null)
			{
				InitializationSuccess = false;
				return;
			}
			for (byte i = 0; i < Count; i++)
			{
				MonsterObject mob = MonsterObject.GetMonster(Monster);
				if (mob is null)
					return;
				mob.Direction = 0;
				mob.ActionTime = Envir.Time + 1000;
				mob.Spawn(Map, new Point(Segment.Param2, Segment.Param3));
			}
		}
	}
}
using System;
using System.Linq;
using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKEXACTMON")]
	public class CheckExactMon : NPCCheck
	{
		protected readonly string MonsterName;
		protected readonly int RequiredAmount;
		protected readonly int RequestedInstance;
		protected readonly string MapName;
		protected Map RequestedMap;
		protected readonly string Operator;
		public CheckExactMon(string line, string[] parts) : base(line, parts)
		{
			if (Envir.GetMonsterInfo(parts[1]) is null)
				return;
			MonsterName = parts[1];
			if (!int.TryParse(parts[3], out RequiredAmount))
				return;
			if (parts.Length > 5)
			{
				if (!int.TryParse(parts[5], out RequestedInstance))
					return;
			}
			else
				RequestedInstance = 0;
			MapName = parts[4];
			Operator = parts[2];
			InitializationSuccess = true;
			RequestedMap = Envir.GetMapByNameAndInstance(parts[4], RequestedInstance);
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			RequestedMap ??= Envir.GetMapByNameAndInstance(MapName, RequestedInstance);
			if (RequestedMap is null)
			{
				return false;
			}
			var count = 0;
			var monsters = Envir.Objects.Where(a => a.Race == ObjectType.Monster).ToArray();
			for (var i = 0; i < monsters.Length; i++)
			{
				var monster = monsters[i];
				if (monster.Dead) continue;
				if (monster.CurrentMap != RequestedMap) continue;
				if (!string.Equals(monster.Name.Replace(" ", ""), MonsterName, StringComparison.OrdinalIgnoreCase)) continue;
				count++;
			}
			return Compare(Operator, RequiredAmount, count);
		}
	}
}
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	[ActionCommand("MONCLEAR")]
	public class ActionMonClear : NPCAction
	{
		protected readonly int InstanceId;
		protected readonly string MobName;
		protected readonly string MapName;
		protected Map Map;
		public ActionMonClear(NPCSegment segment, string line, string[] parts) : base(segment, line, parts)
		{
			if (parts.Length < 3)
			{
				InstanceId = 1;
			}
			else if (!int.TryParse(parts[2], out InstanceId))
				return;
			MobName = parts.Length < 4 ? "" : parts[3];
			MapName = parts[1];
			InitializationSuccess = true;
			Map = Envir.GetMapByNameAndInstance(MapName, InstanceId);
		}
		public override void Execute(MapObject ob)
		{
			if (!InitializationSuccess) return;
			Map ??= Envir.GetMapByNameAndInstance(MapName, InstanceId);
			if (Map is null)
			{
				InitializationSuccess = false;
				return;
			}
			foreach (var mapCell in Map.Cells)
			{
				if (mapCell?.Objects is null) continue;
				for (int i = 0; i < mapCell.Objects.Count; i++)
				{
					var mapObject = mapCell.Objects[i];
					if (mapObject.Race != ObjectType.Monster) continue;
					if (mapObject.Dead) continue;
					mapObject.Die();
				}
			}
		}
	}
}
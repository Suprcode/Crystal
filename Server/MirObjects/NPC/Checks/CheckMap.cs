using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKMAP")]
	public class CheckMap : NPCCheck
	{
		protected readonly string MapName;
		protected Map Map;
		public CheckMap(string line, string[] parts) : base(line, parts)
		{
			Map = Envir.GetMapByNameAndInstance(parts[1]);
			MapName = parts[1];
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			Map ??= Envir.GetMapByNameAndInstance(MapName);
			if (Map is null)
			{
				InitializationSuccess = false;
				return false;
			}
			return  ob.CurrentMap == Map;
		}
	}
}
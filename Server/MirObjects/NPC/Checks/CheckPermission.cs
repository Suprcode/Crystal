using System;
namespace Server.MirObjects.Checks
{
	[CheckCommand("CHECKPERMISSION")]
	public class CheckPermission : NPCCheck
	{
		protected readonly GuildRankOptions RequiredPermissions;
		
		public CheckPermission(string line, string[] parts) : base(line, parts)
		{
			if (!Enum.TryParse(parts[1], out RequiredPermissions))
				return;
			InitializationSuccess = true;
		}
		public override bool Check(MapObject ob)
		{
			if (!InitializationSuccess) return false;
			switch (ob)
			{
				case PlayerObject playerObject:
					return playerObject.MyGuild is {} && playerObject.MyGuildRank.Options.HasFlag(RequiredPermissions);
			}
			return false;
		}
	}
}
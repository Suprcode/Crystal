using System;
namespace Server.MirObjects.Checks
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CheckCommandAttribute : Attribute
	{
		public readonly string Command;
		public CheckCommandAttribute(string command)
		{
			if (string.IsNullOrEmpty(command))
				throw new ArgumentNullException(nameof(command), "Command cannot be null or empty.");
			Command = command;
		}
	}
}
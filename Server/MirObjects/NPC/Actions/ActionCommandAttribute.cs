using System;
namespace Server.MirObjects.Actions
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionCommandAttribute : Attribute
	{
		public readonly string Command;
		public ActionCommandAttribute(string command)
		{
			if (string.IsNullOrEmpty(command))
				throw new ArgumentNullException(nameof(command), "Command cannot be null or empty.");
			Command = command;
		}
		
	}
}
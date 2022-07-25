using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Server.MirEnvir;
namespace Server.MirObjects.Checks
{
	public abstract class NPCCheck
	{
		protected static Envir Envir => Envir.Main;
		protected static MessageQueue MessageQueue => MessageQueue.Instance;
		protected static RandomProvider Random => Envir.Random;
		public NPCPage Page;
		public readonly string Line;
		public readonly string[] Parts;
		protected readonly Regex FlagRegex = new Regex(@"\[(.*?)\]");
		protected readonly Regex QuoteRegex = new Regex("\"([^\"]*)\"");
		protected bool InitializationSuccess;
		protected NPCCheck(string line, string[] parts)
		{
			Line = line;
			Parts = parts;
		}
		
		public abstract bool Check(MapObject ob);
		
		public static string GetCommand(Type type)
		{
			var attr = type.GetCustomAttribute<CheckCommandAttribute>();
			return attr?.Command;
		}


		public bool Compare<T>(string op, T left, T right) where T : IComparable<T>
		{

			switch (op)
			{
				case "=":
					return left.Equals(right);
				case "!":
					return !left.Equals(right);
				case "<":
					return left.CompareTo(right) < 0;
				case ">":
					return left.CompareTo(right) > 0;
				case "<=":
					return left.CompareTo(right) <= 0;
				case ">=":
					return left.CompareTo(right) >= 0;
				default:
					MessageQueue.Instance.Enqueue($"{nameof(NPCCheck)} Error: {nameof(Compare)} for Command {GetType().Name}: Unknown operator: {op}.\n[{DateTime.Now:dd-MM-yy hh:mm:ss}] {Page.Key}");
					return false;
			}
		}
	}
}
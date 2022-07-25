using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Server.MirEnvir;
namespace Server.MirObjects.Actions
{
	public abstract class NPCAction
	{
		protected MessageQueue MessageQueue => MessageQueue.Instance;
		protected static Envir Envir => Envir.Main;
		protected static RandomProvider Random => Envir.Random;
		protected readonly string Line;
		public string[] Parts { get; protected set; }
		protected bool InitializationSuccess;
		protected readonly Regex FlagRegex = new Regex(@"\[(.*?)\]");
		protected readonly Regex QuoteRegex = new Regex("\"([^\"]*)\"");
		protected readonly NPCPage Page;
		protected readonly NPCSegment Segment;
		protected NPCAction(NPCSegment segment, string line, string[] parts)
		{
			Segment = segment;
			Page = Segment.Page;
			Line = line;
			Parts = parts;
		}
		public abstract void Execute(MapObject ob);
		public static string GetCommand(Type type)
		{
			var attr = type.GetCustomAttribute<ActionCommandAttribute>();
			return attr?.Command;
		}
		
		public void AddVariable(MapObject player, string key, string value)
		{
			Regex regex = new Regex(@"[A-Za-z][0-9]");

			if (!regex.Match(key).Success) return;

			for (int i = 0; i < player.NPCVar.Count; i++)
			{
				if (!String.Equals(player.NPCVar[i].Key, key, StringComparison.CurrentCultureIgnoreCase)) continue;
				player.NPCVar[i] = new KeyValuePair<string, string>(player.NPCVar[i].Key, value);
				return;
			}

			player.NPCVar.Add(new KeyValuePair<string, string>(key, value));
		}

		public string FindVariable(MapObject player, string key)
		{
			Regex regex = new Regex(@"\%[A-Za-z][0-9]");

			if (!regex.Match(key).Success) return key;

			string tempKey = key.Substring(1);

			foreach (KeyValuePair<string, string> t in player.NPCVar)
			{
				if (String.Equals(t.Key, tempKey, StringComparison.CurrentCultureIgnoreCase)) return t.Value;
			}

			return key;
		}
		
		public int Calculate(string op, int left, int right)
		{
			switch (op)
			{
				case "+": return left + right;
				case "-": return left - right;
				case "*": return left * right;
				case "/": return left / right;
				default:  throw new ArgumentException("Invalid sum operator: {0}", op);
			}
		}
	}
}
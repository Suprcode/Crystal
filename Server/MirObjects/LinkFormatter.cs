using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Server.MirEnvir;

namespace Server.MirObjects
{
    public static class LinkFormatter
    {
        private static readonly Regex LinkRegex = new Regex(@"\<\$(?<type>MONSTER|NPC|ITEM):(?<idx>\d+)\>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string ReplacePlaceholders(string input, Action<string, int> linkRequested = null)
        {
            if (string.IsNullOrEmpty(input)) return input;
            if (!input.Contains("<$")) return input;

            return LinkRegex.Replace(input, match =>
            {
                string type = match.Groups["type"].Value.ToUpper();
                if (!int.TryParse(match.Groups["idx"].Value, out int index))
                    return match.Value;

                linkRequested?.Invoke(type, index);

                string displayName = GetDisplayName(type, index);
                if (string.IsNullOrEmpty(displayName))
                    return $"[{type}:{index}]";

                return $"[{type}:{index}|{displayName}]";
            });
        }

        public static List<string> ReplacePlaceholders(IEnumerable<string> source, Action<string, int> linkRequested = null)
        {
            if (source == null) return null;
            return source.Select(line => ReplacePlaceholders(line, linkRequested)).ToList();
        }

        private static string GetDisplayName(string type, int index)
        {
            switch (type)
            {
                case "MONSTER":
                    return Envir.Main.GetMonsterInfo(index)?.Name;
                case "NPC":
                    return Envir.Main.GetNPCInfo(index)?.Name;
                case "ITEM":
                    return Envir.Main.GetItemInfo(index)?.Name;
                default:
                    return null;
            }
        }
    }
}


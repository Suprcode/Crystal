﻿using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public static class RegexFunctions
{
    public static Regex ChatItemLinks = new Regex(@"<(.*?/.*?)>");

    public enum RegexMatchEvalType
    {
        ChatLinkName
    }

    private static string RegexReplace(string text, Regex regex, MatchEvaluator ev)
    {
        try
        {
            return regex.Replace(text, ev);
        }
        catch
        {
            return text;
        }
    }

    private static MatchEvaluator GetMatchEv(RegexMatchEvalType type)
    {
        switch (type)
        {
            case RegexMatchEvalType.ChatLinkName:
                return m => m.Groups[1].Captures[0].Value.Split('/')[0];
            default:
                return null;
        }
    }

    public static string CleanChatString(string text)
    {
        return RegexReplace(text, ChatItemLinks, GetMatchEv(RegexMatchEvalType.ChatLinkName));
    }
}

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

    public static string SeperateCamelCase(this string value) 
    { 
        return Regex.Replace(value, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); 
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

using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Runtime localization for game and server UI text. Source strings are discovered while the
/// game runs and appended to the language file on first use.
/// </summary>
public static class GameLanguage
{
    public static readonly LocalizationStore ServerStore = new LocalizationStore();
    public static readonly LocalizationStore ClientStore = new LocalizationStore();

    private static LocalizationStore Store
    {
        get { return Packet.IsServer ? ServerStore : ClientStore; }
    }

    /// <summary>
    /// Language whose keys are already English, so it needs no file: missing translations fall back
    /// to the key itself and runtime additions are never written back.
    /// </summary>
    public const string SourceLanguage = "English";

    public static void LoadClientLanguage(string languageJsonPath)
    {
        ClientStore.Load(languageJsonPath, !IsSourceLanguage(languageJsonPath));
        Seed(ClientStore);
    }

    public static void SaveClientLanguage(string languageJsonPath) => ClientStore.Save(languageJsonPath);

    public static void LoadServerLanguage(string languageJsonPath)
    {
        ServerStore.Load(languageJsonPath, !IsSourceLanguage(languageJsonPath));
        Seed(ServerStore);
    }

    private static bool IsSourceLanguage(string path) =>
        string.Equals(Path.GetFileNameWithoutExtension(path), SourceLanguage, StringComparison.OrdinalIgnoreCase);

    public static void SaveServerLanguage(string languageJsonPath) => ServerStore.Save(languageJsonPath);

    /// <summary>
    /// Registers every template found by the build-time scanner, so a fresh install starts with a
    /// complete table instead of waiting for each line of text to be executed.
    /// </summary>
    private static void Seed(LocalizationStore store)
    {
        foreach (var template in LocalizationCatalog.Templates)
        {
            store.TryAdd(template, template);
        }
    }

    /// <summary>
    /// Translates an interpolated string. The compiler supplies the format string, so argument
    /// values never become part of the key.
    /// </summary>
    public static string Get(LocalizationStringHandler text)
    {
        return Resolve(text.Template, text.Arguments);
    }

    /// <summary>
    /// Translates a plain string, used when there is nothing to interpolate.
    /// </summary>
    public static string Get(string template)
    {
        return Resolve(template, Array.Empty<object>());
    }

    /// <summary>
    /// Translates an explicit template with {0} placeholders. Used for dynamic templates.
    /// </summary>
    public static string Get(string template, params object[] args)
    {
        return Resolve(template, args);
    }

    private static string Resolve(string template, object[] args)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;

        var translation = Store.Translate(template, template);
        var formatted = Format(translation, args);

        return formatted ?? Format(template, args) ?? template;
    }

    private static string Format(string text, object[] args)
    {
        if (string.IsNullOrEmpty(text) || args == null || args.Length == 0) return text;

        try
        {
            return string.Format(text, args);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Translates an enum value through the "enum.Type_Value" key, falling back to the enum name.
    /// </summary>
    public static string Get(Enum value)
    {
        var name = value.ToString();
        return Store.Translate("enum." + value.GetType().Name + "_" + name, name);
    }

    /// <summary>
    /// Captures the format string and arguments of an interpolated string without formatting it,
    /// so the template can be used as the localization key.
    /// </summary>
    [InterpolatedStringHandler]
    public ref struct LocalizationStringHandler
    {
        private readonly StringBuilder _template;
        private readonly object[] _arguments;
        private int _index;

        public LocalizationStringHandler(int literalLength, int formattedCount)
        {
            _template = new StringBuilder(literalLength + formattedCount * 4);
            _arguments = formattedCount == 0 ? Array.Empty<object>() : new object[formattedCount];
        }

        public void AppendLiteral(string value) => _template.Append(value.Replace("{", "{{").Replace("}", "}}"));

        public void AppendFormatted<T>(T value)
        {
            _arguments[_index] = value;
            _template.Append('{').Append(_index).Append('}');
            _index++;
        }

        public void AppendFormatted<T>(T value, string format)
        {
            _arguments[_index] = value;
            _template.Append('{').Append(_index).Append(':').Append(format).Append('}');
            _index++;
        }

        public void AppendFormatted<T>(T value, int alignment)
        {
            _arguments[_index] = value;
            _template.Append('{').Append(_index).Append(',').Append(alignment).Append('}');
            _index++;
        }

        public void AppendFormatted<T>(T value, int alignment, string format)
        {
            _arguments[_index] = value;
            _template.Append('{').Append(_index).Append(',').Append(alignment).Append(':').Append(format).Append('}');
            _index++;
        }

        public string Template => _template.ToString();
        public object[] Arguments => _arguments;
    }
}

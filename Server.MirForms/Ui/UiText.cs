using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Server
{
    /// <summary>
    /// Runtime localization layer for the server admin forms.
    /// <para>
    /// It walks the control tree of a form, applies translations from the active table and
    /// registers every English string that is not in the table yet. New entries are appended with
    /// an empty translation, so the table can be maintained without a separate extraction tool.
    /// Any language is supported: the table name is derived from the configured language and the
    /// English source text is displayed until a translation exists.
    /// </para>
    /// </summary>
    internal static class UiText
    {
        private const string TitleSuffix = "#Title";

        private static LocalizationStore Store
        {
            get { return GameLanguage.ServerStore; }
        }
        private static readonly ConditionalWeakTable<Form, object> LocalizedForms = new ConditionalWeakTable<Form, object>();

        /// <summary>
        /// Fonts that cannot render CJK characters. A control using one of them is switched to a
        /// CJK capable font only when its text actually contains CJK, so languages that use a
        /// Latin script keep the original metrics and autoscaling baseline.
        /// </summary>
        private static readonly HashSet<string> LatinOnlyFonts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Microsoft Sans Serif",
            "Tahoma",
            "Arial",
            "Verdana",
            "Segoe UI",
            "Calibri",
            "Trebuchet MS",
            "Times New Roman",
            "Courier New"
        };

        private static FontFamily _cjkFamily;
        private static bool _cjkResolved;

        private static string _directory = "Localization";
        private static string _language = "English";

        public static string Language => _language;
        public static string FilePath => GameLanguage.ServerStore.FilePath;

        /// <summary>
        /// Loads the table of the given language. A missing table is not an error: it is created as
        /// soon as the first form is localized.
        /// </summary>
        public static void Load(string directory, string language)
        {
            _directory = string.IsNullOrEmpty(directory) ? "Localization" : directory;
            _language = string.IsNullOrEmpty(language) ? "English" : language;

            // The table is loaded once by GameLanguage.LoadServerLanguage for both game text and UI text.
        }

        /// <summary>
        /// Applies translations to a form and appends unknown strings to the table.
        /// </summary>
        public static void Apply(Form form)
        {
            if (form == null) return;
            if (LocalizedForms.TryGetValue(form, out _)) return;

            LocalizedForms.Add(form, null);

            try
            {
                var dirty = false;

                ApplyText(form.Text, value => form.Text = value, form.GetType().Name + "." + TitleSuffix, ref dirty);
                FixFont(form, form.Text);

                foreach (System.Windows.Forms.Control child in form.Controls)
                {
                    Walk(child, form.GetType().Name, ref dirty);
                }

                if (dirty) Store.SaveIfDirty();
            }
            catch (Exception ex)
            {
                Log("Failed to localize form " + form.GetType().Name, ex);
            }
        }

        /// <summary>
        /// Startup self check: scans the forms that are not listed in the table metadata yet, so a
        /// fresh install produces a complete table on first launch without any command line switch.
        /// </summary>
        public static void EnsureCoverage()
        {
            try
            {
                var assembly = typeof(SMain).Assembly;
                var all = assembly.GetTypes()
                    .Where(type => typeof(Form).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericType)
                    .ToList();

                var missing = all.Where(type => !Store.Forms.Contains(type.Name)).ToList();
                if (missing.Count == 0) return;

                var success = 0;
                var failed = 0;

                foreach (var type in missing)
                {
                    try
                    {
                        var form = CreateInstance(type);
                        if (form == null)
                        {
                            failed++;
                            continue;
                        }

                        // A form that already localized itself in its constructor must not be walked
                        // again: the second pass would read translated text and register it as source.
                        if (!LocalizedForms.TryGetValue(form, out _))
                        {
                            var dirty = false;

                            ApplyText(form.Text, value => form.Text = value, type.Name + "." + TitleSuffix, ref dirty);
                            FixFont(form, form.Text);

                            foreach (System.Windows.Forms.Control child in form.Controls)
                            {
                                Walk(child, type.Name, ref dirty);
                            }

                            Store.SaveIfDirty();
                        }

                        form.Dispose();
                        Store.MarkCovered(type.Name);
                        success++;
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        Log("Skipped form during UI scan: " + type.Name, ex);
                    }
                }

                Store.SaveIfDirty();
                Log(GameLanguage.Get("UI table check finished: {0} forms, {1} scanned, {2} failed, {3} entries, {4} untranslated",
                    all.Count, success, failed, Store.Count, Store.UntranslatedCount), null);
            }
            catch (Exception ex)
            {
                Log("UI table check failed", ex);
            }
        }

        public static void Report()
        {
            Log(GameLanguage.Get("UI table {0}: {1} entries, {2} forms covered, {3} untranslated",
                FilePath, Store.Count, Store.Forms.Count, Store.UntranslatedCount), null);
        }

        private static Form CreateInstance(Type type)
        {
            var parameterless = type.GetConstructor(Type.EmptyTypes);
            if (parameterless != null) return parameterless.Invoke(null) as Form;

            var constructor = type.GetConstructors()
                .OrderBy(item => item.GetParameters().Length)
                .FirstOrDefault();

            if (constructor == null) return null;

            var arguments = constructor.GetParameters()
                .Select(parameter => parameter.HasDefaultValue
                    ? parameter.DefaultValue
                    : (parameter.ParameterType.IsValueType ? Activator.CreateInstance(parameter.ParameterType) : null))
                .ToArray();

            return constructor.Invoke(arguments) as Form;
        }

        private static void Walk(System.Windows.Forms.Control control, string form, ref bool dirty)
        {
            if (!string.IsNullOrEmpty(control.Name) && HasVisibleText(control) && !IsPlaceholder(control.Text, control.Name))
            {
                ApplyText(control.Text, value => control.Text = value, form + "." + control.Name, ref dirty);
            }

            FixFont(control, control.Text);

            if (control is ListView listView && !string.IsNullOrEmpty(listView.Name))
            {
                var cjkHeader = false;

                for (var index = 0; index < listView.Columns.Count; index++)
                {
                    var column = listView.Columns[index];
                    if (string.IsNullOrEmpty(column.Name) && string.Equals(column.Text, "ColumnHeader", StringComparison.Ordinal)) continue;

                    var suffix = string.IsNullOrEmpty(column.Name) ? "Column[" + index + "]" : column.Name;
                    var text = column.Text;

                    ApplyText(text, value => column.Text = value, form + "." + listView.Name + "." + suffix, ref dirty);

                    if (ContainsCjk(column.Text)) cjkHeader = true;
                }

                if (cjkHeader) ReplaceFont(listView);
            }

            if (control is DataGridView grid && !string.IsNullOrEmpty(grid.Name))
            {
                var cjkHeader = false;

                for (var index = 0; index < grid.Columns.Count; index++)
                {
                    var column = grid.Columns[index];
                    var suffix = string.IsNullOrEmpty(column.Name) ? "Column[" + index + "]" : column.Name;
                    var header = column.HeaderText;

                    ApplyText(header, value => column.HeaderText = value, form + "." + grid.Name + "." + suffix, ref dirty);

                    if (ContainsCjk(column.HeaderText)) cjkHeader = true;
                }

                if (cjkHeader) ReplaceFont(grid);
            }

            if (control is ToolStrip strip)
            {
                WalkItems(strip.Items, form + "." + strip.Name, ref dirty);
            }

            foreach (System.Windows.Forms.Control child in control.Controls)
            {
                Walk(child, form, ref dirty);
            }
        }

        private static void WalkItems(ToolStripItemCollection items, string owner, ref bool dirty)
        {
            if (items == null) return;

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var suffix = string.IsNullOrEmpty(item.Name) ? "Item[" + index + "]" : item.Name;
                var key = owner + "." + suffix;

                if (!IsPlaceholder(item.Text, item.Name))
                {
                    ApplyText(item.Text, value => item.Text = value, key, ref dirty);
                }

                if (ContainsCjk(item.Text)) ReplaceFont(item);

                if (item is ToolStripDropDownItem dropDown && dropDown.HasDropDownItems)
                {
                    WalkItems(dropDown.DropDownItems, key, ref dirty);
                }
            }
        }

        private static void ApplyText(string source, Action<string> apply, string key, ref bool dirty)
        {
            if (string.IsNullOrWhiteSpace(source)) return;
            if (!HasLetter(source)) return;

            // Placeholders such as $map or $x/y are replaced by the forms at runtime.
            if (source.TrimStart().StartsWith("$", StringComparison.Ordinal)) return;

            key = "ui." + key;

            var entry = Store.Resolve(key, source);

            if (entry == null)
            {
                if (Store.TryAdd(key, source)) dirty = true;
                return;
            }

            if (!string.IsNullOrEmpty(entry.Text)) apply(entry.Text);
        }

        /// <summary>
        /// Controls whose Text is actually rendered. Containers such as ToolStrip, StatusStrip and
        /// MenuStrip report their own control name as Text, and TextBox text is user data, so both
        /// stay out of the table.
        /// </summary>
        private static bool HasVisibleText(System.Windows.Forms.Control control)
        {
            return control is Label
                || control is Button
                || control is CheckBox
                || control is RadioButton
                || control is GroupBox
                || control is TabPage
                || control is LinkLabel;
        }

        private static bool IsPlaceholder(string text, string name)
        {
            return !string.IsNullOrEmpty(name) && string.Equals(text, name, StringComparison.Ordinal);
        }

        private static bool HasLetter(string text)
        {
            foreach (var character in text)
            {
                if (char.IsLetter(character)) return true;
            }

            return false;
        }

        /// <summary>
        /// True when the text contains characters that require a CJK capable font: CJK unified
        /// ideographs and extension A, kana, and Hangul syllables.
        /// </summary>
        private static bool ContainsCjk(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (var character in text)
            {
                if (character >= '\u4E00' && character <= '\u9FFF') return true;
                if (character >= '\u3400' && character <= '\u4DBF') return true;
                if (character >= '\u3040' && character <= '\u30FF') return true;
                if (character >= '\uAC00' && character <= '\uD7AF') return true;
            }

            return false;
        }

        private static void FixFont(System.Windows.Forms.Control control, string text)
        {
            if (ContainsCjk(text)) ReplaceFont(control);
        }

        private static void ReplaceFont(System.Windows.Forms.Control control)
        {
            try
            {
                var font = control.Font;
                if (font == null || !LatinOnlyFonts.Contains(font.FontFamily.Name)) return;

                var family = ResolveCjkFamily();
                if (family == null) return;

                control.Font = new Font(family, font.Size, font.Style, font.Unit);
            }
            catch (Exception)
            {
                // Some controls reject a font change, keep the original one.
            }
        }

        private static void ReplaceFont(ToolStripItem item)
        {
            try
            {
                var font = item.Font;
                if (font == null || !LatinOnlyFonts.Contains(font.FontFamily.Name)) return;

                var family = ResolveCjkFamily();
                if (family == null) return;

                item.Font = new Font(family, font.Size, font.Style, font.Unit);
            }
            catch (Exception)
            {
            }
        }

        private static FontFamily ResolveCjkFamily()
        {
            if (_cjkResolved) return _cjkFamily;

            _cjkResolved = true;

            foreach (var name in PreferredFamilies())
            {
                try
                {
                    _cjkFamily = new FontFamily(name);
                    return _cjkFamily;
                }
                catch (Exception)
                {
                }
            }

            return null;
        }

        /// <summary>
        /// Font candidates for the active language, most specific first.
        /// </summary>
        private static IEnumerable<string> PreferredFamilies()
        {
            var language = _language.ToLowerInvariant();

            if (language.StartsWith("ja"))
            {
                yield return "Yu Gothic UI";
                yield return "Meiryo";
            }
            else if (language.StartsWith("ko"))
            {
                yield return "Malgun Gothic";
            }

            yield return "Microsoft YaHei UI";
            yield return "Microsoft YaHei";
            yield return "SimSun";
        }

        private static void Log(string message, Exception exception)
        {
            try
            {
                var logger = Logger.GetLogger();

                if (exception == null) logger.Info(message);
                else logger.Error(message, exception);
            }
            catch (Exception)
            {
            }
        }
    }
}

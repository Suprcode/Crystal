using System.Reflection;

namespace Server
{
    public static partial class Settings
    {
        // Also used by the editor's save-time reference transaction.
        public static object MonsterNameSyncRoot { get; } = new();

        private static readonly Dictionary<(string Section, string Key), FieldInfo> MonsterNameFields =
            new[]
            {
                nameof(SkeletonName), nameof(BugBatName), nameof(ShinsuName),
                nameof(Zuma1), nameof(Zuma2), nameof(Zuma3), nameof(Zuma4), nameof(Zuma5), nameof(Zuma6), nameof(Zuma7),
                nameof(Turtle1), nameof(Turtle2), nameof(Turtle3), nameof(Turtle4), nameof(Turtle5),
                nameof(BoneMonster1), nameof(BoneMonster2), nameof(BoneMonster3), nameof(BoneMonster4),
                nameof(BehemothMonster1), nameof(BehemothMonster2), nameof(BehemothMonster3),
                nameof(HellKnight1), nameof(HellKnight2), nameof(HellKnight3), nameof(HellKnight4),
                nameof(HellBomb1), nameof(HellBomb2), nameof(HellBomb3),
                nameof(GeneralMeowMeowMob1), nameof(GeneralMeowMeowMob2), nameof(GeneralMeowMeowMob3), nameof(GeneralMeowMeowMob4),
                nameof(KingHydraxMob), nameof(HornedCommanderMob), nameof(HornedCommanderBombMob), nameof(SnowWolfKingMob),
                nameof(ScrollMob1), nameof(ScrollMob2), nameof(ScrollMob3), nameof(ScrollMob4),
                nameof(WhiteSnake), nameof(AngelName), nameof(BombSpiderName), nameof(CloneName), nameof(AssassinCloneName),
                nameof(VampireName), nameof(ToadName), nameof(SnakeTotemName), nameof(SnakesName), nameof(StoneName),
                nameof(AncientBatName), nameof(TucsonGeneralEgg), nameof(FishingMonster)
            }.ToDictionary(name => ("Game", name == nameof(FishingMonster) ? "FishMonster" : name),
                name => typeof(Settings).GetField(name, BindingFlags.Public | BindingFlags.Static));

        public static (string Section, string Key, string Value)[] GetMonsterNameSettings()
        {
            lock (MonsterNameSyncRoot)
                return MonsterNameFields.Select(x => (x.Key.Section, x.Key.Key, (string)x.Value.GetValue(null))).ToArray();
        }

        public static void CheckMonsterNameSettings(IEnumerable<(string Section, string Key, string Value)> values)
        {
            lock (MonsterNameSyncRoot)
                foreach (var value in values)
                    if (!MonsterNameFields.TryGetValue((value.Section, value.Key), out var field) ||
                        (string)field.GetValue(null) != value.Value)
                        throw new IOException($"Monster setting changed after validation. Save was cancelled: [{value.Section}] {value.Key}");
        }

        public static void SetMonsterNameSettings(IEnumerable<(string Section, string Key, string Value)> values)
        {
            lock (MonsterNameSyncRoot)
            {
                var updates = values.Select(value => (Field: MonsterNameFields[(value.Section, value.Key)], value.Value)).ToArray();
                foreach (var update in updates) update.Field.SetValue(null, update.Value);
            }
        }

        // Refresh only the INI writer cache. Do not reload or save any Settings fields.
        public static void RefreshSetupCache(IEnumerable<string> contents = null)
        {
            lock (MonsterNameSyncRoot)
            {
                var path = Path.Combine(ConfigPath, "Setup.ini");
                Reader.ReplaceContents(contents ?? (File.Exists(path) ? File.ReadAllLines(path) : Array.Empty<string>()));
            }
        }
    }
}

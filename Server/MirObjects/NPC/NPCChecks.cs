namespace Server.MirObjects
{
    public class NPCChecks
    {
        public CheckType Type;
        public List<string> Params = new List<string>();

        public NPCChecks(CheckType check, params string[] p)
        {
            Type = check;

            for (int i = 0; i < p.Length; i++)
                Params.Add(p[i]);
        }
    }

    public enum CheckType
    {
        IsAdmin,
        Level,
        CheckItem,
        CheckGold,
        CheckGuildGold,
        CheckCredit,
        CheckGender,
        CheckClass,
        CheckDay,
        CheckHour,
        CheckMinute,
        CheckNameList,
        CheckPkPoint,
        CheckRange,
        Check,
        CheckHum,
        CheckMon,
        CheckExactMon,
        Random,
        Groupleader,
        GroupCount,
        GroupCheckNearby,
        PetLevel,
        PetCount,
        CheckCalc,
        InGuild,
        CheckMap,
        CheckQuest,
        CheckRelationship,
        CheckWeddingRing,
        CheckPet,
        HasBagSpace,
        IsNewHuman,
        CheckConquest,
        AffordGuard,
        AffordGate,
        AffordWall,
        AffordSiege,
        CheckPermission,
        ConquestAvailable,
        ConquestOwner,
        CheckGuildNameList,
        CheckTimer
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MirObjects
{
    public class NPCActions
    {
        public ActionType Type;
        public List<string> Params = new List<string>();

        public NPCActions(ActionType action, params string[] p)
        {
            Type = action;

            Params.AddRange(p);
        }
    }

    public enum ActionType
    {
        ReloadNpcs,
        BuyGT,
        TeleportGT,
        ExtendGT,
        Move,
        InstanceMove,
        GiveGold,
        TakeGold,
        GiveGuildGold,
        TakeGuildGold,
        GiveCredit,
        TakeCredit,
        GiveHuntPoints,
        TakeHuntPoints,
        GiveItem,
        TakeItem,
        GiveExp,
        GivePet,
        ClearPets,
        AddNameList,
        DelNameList,
        ClearNameList,
        GiveHP,
        GiveMP,
        ChangeLevel,
        ChangeReborn,
        ChangeInstanceStage,
        ChangeChallengeStage,
        SetPkPoint,
        ReducePkPoint,
        IncreasePkPoint,
        ChangeGender,
        ChangeClass,
        LocalMessage,
        Goto,
        Call,
        GiveSkill,
        RemoveSkill,
        SetNameColor,
        Set,
        Param1,
        Param2,
        Param3,
        Mongen,
        TimeRecall,
        TimeRecallGroup,
        BreakTimeRecall,
        MonClear,
        GroupRecall,
        GroupTeleport,
        DelayGoto,
        Mov,
        Calc,
        GiveBuff,
        RemoveBuff,
        AddToGuild,
        RemoveFromGuild,
        RefreshEffects,
        ChangeHair,
        CanGainExp,
        ComposeMail,
        AddMailItem,
        AddMailGold,
        AddMailCredit,
        SendMail,
        GroupGoto,
        EnterMap,
        GivePearls,
        TakePearls,
        MakeWeddingRing,
        ForceDivorce,
        GlobalMessage,
        MapMessage,
        LoadValue,
        SaveValue,
        RemovePet,
        ConquestGuard,
        ConquestGate,
        ConquestWall,
        ConquestSiege,
        TakeConquestGold,
        SetConquestRate,
        StartConquest,
        TakeConquest,
        ScheduleConquest,
        OpenGate,
        CloseGate,
        Break,
        AddGuildNameList,
        DelGuildNameList,
        ClearGuildNameList,
        OpenBrowser,
        GetRandomText,
        PlaySound,
        SetTimer,
        SetTimer2,
        ExpireTimer,
        ExpireTimer2,
        UnequipItem,
        RollDie,
        RollYut,
        Drop,

        //ATTRIBUTES SYSTEM
        AddAttributePoint,
    }
}

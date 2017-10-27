using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace ClientPackets
{
    public sealed class ClientVersion : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.ClientVersion; }
        }

        public byte[] VersionHash;

        protected override void ReadPacket(BinaryReader reader)
        {
            VersionHash = reader.ReadBytes(reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(VersionHash.Length);
            writer.Write(VersionHash);
        }
    }
    public sealed class Disconnect : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.Disconnect; }
        }
        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class KeepAlive : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.KeepAlive; }
        }
        public long Time;
        protected override void ReadPacket(BinaryReader reader)
        {
            Time = reader.ReadInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Time);
        }
    }
    public sealed class NewAccount: Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.NewAccount; }
        }

        public string AccountID = string.Empty;
        public string Password = string.Empty;
        public DateTime BirthDate;
        public string UserName = string.Empty;
        public string SecretQuestion = string.Empty;
        public string SecretAnswer = string.Empty;
        public string EMailAddress = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            AccountID = reader.ReadString();
            Password = reader.ReadString();
            BirthDate = DateTime.FromBinary(reader.ReadInt64());
            UserName = reader.ReadString();
            SecretQuestion = reader.ReadString();
            SecretAnswer = reader.ReadString();
            EMailAddress = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AccountID);
            writer.Write(Password);
            writer.Write(BirthDate.ToBinary());
            writer.Write(UserName);
            writer.Write(SecretQuestion);
            writer.Write(SecretAnswer);
            writer.Write(EMailAddress);
        }
    }
    public sealed class ChangePassword: Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.ChangePassword; }
        }

        public string AccountID = string.Empty;
        public string CurrentPassword = string.Empty;
        public string NewPassword = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            AccountID = reader.ReadString();
            CurrentPassword = reader.ReadString();
            NewPassword = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AccountID);
            writer.Write(CurrentPassword);
            writer.Write(NewPassword);
        }
    }
    public sealed class Login : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.Login; }
        }

        public string AccountID = string.Empty;
        public string Password = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            AccountID = reader.ReadString();
            Password = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AccountID);
            writer.Write(Password);
        }
    }
    public sealed class NewCharacter : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.NewCharacter; } }

        public string Name = string.Empty;
        public MirGender Gender;
        public MirClass Class;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Gender = (MirGender)reader.ReadByte();
            Class = (MirClass)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)Gender);
            writer.Write((byte)Class);
        }
    }
    public sealed class DeleteCharacter : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DeleteCharacter; } }

        public int CharacterIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
        }
    }
    public sealed class StartGame : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.StartGame; } }

        public int CharacterIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
        }
    }
    public sealed class LogOut : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.LogOut; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class Turn : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Turn; } }

        public MirDirection Direction;

        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
        }
    }
    public sealed class Walk : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Walk; } }

        public MirDirection Direction;
        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
        }
    }
    public sealed class Run : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Run; } }

        public MirDirection Direction;
        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
        }
    }
    public sealed class Chat : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Chat; } }

        public string Message = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Message = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Message);
        }
    }
    public sealed class MoveItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MoveItem; } }

        public MirGridType Grid;
        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(From);
            writer.Write(To);
        }
    }
    public sealed class StoreItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.StoreItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class DepositRefineItem : Packet
    {

        public override short Index { get { return (short)ClientPacketIds.DepositRefineItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class RetrieveRefineItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RetrieveRefineItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class RefineCancel : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RefineCancel; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class RefineItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RefineItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class CheckRefine : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CheckRefine; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class ReplaceWedRing : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ReplaceWedRing; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }


    public sealed class DepositTradeItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DepositTradeItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class RetrieveTradeItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RetrieveTradeItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }
    public sealed class TakeBackItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TakeBackItem; } }

        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }
    public sealed class MergeItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MergeItem; } }

        public MirGridType GridFrom, GridTo;
        public ulong IDFrom, IDTo;
        protected override void ReadPacket(BinaryReader reader)
        {
            GridFrom = (MirGridType)reader.ReadByte();
            GridTo = (MirGridType)reader.ReadByte();
            IDFrom = reader.ReadUInt64();
            IDTo = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)GridFrom);
            writer.Write((byte)GridTo);
            writer.Write(IDFrom);
            writer.Write(IDTo);
        }
    }
    public sealed class EquipItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.EquipItem; } }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
        }
    }
    public sealed class RemoveItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RemoveItem; } }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
        }
    }
    public sealed class RemoveSlotItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RemoveSlotItem; } }

        public MirGridType Grid;
        public MirGridType GridTo;
        public ulong UniqueID;
        public int To;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            GridTo = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write((byte)GridTo);
            writer.Write(UniqueID);
            writer.Write(To);
        }
    }
    public sealed class SplitItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SplitItem; } }

        public MirGridType Grid;
        public ulong UniqueID;
        public uint Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }
    public sealed class UseItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.UseItem; } }

        public ulong UniqueID;
        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }
    public sealed class DropItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DropItem; } }

        public ulong UniqueID;
        public uint Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }
    public sealed class DropGold : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DropGold; } }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }
    public sealed class PickUp : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.PickUp; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class Inspect : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.Inspect; }
        }

        public uint ObjectID;
        public bool Ranking = false;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Ranking = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Ranking);
        }
    }
    public sealed class ChangeAMode : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ChangeAMode; } }

        public AttackMode Mode;

        protected override void ReadPacket(BinaryReader reader)
        {
            Mode = (AttackMode)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Mode);
        }
    }
    public sealed class ChangePMode : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ChangePMode; } }

        public PetMode Mode;

        protected override void ReadPacket(BinaryReader reader)
        {
            Mode = (PetMode)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Mode);
        }
    }
    public sealed class ChangeTrade : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ChangeTrade; } }

        public bool AllowTrade;

        protected override void ReadPacket(BinaryReader reader)
        {
            AllowTrade = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AllowTrade);
        }
    }
    public sealed class Attack : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Attack; } }

        public MirDirection Direction;
        public Spell Spell;

        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection) reader.ReadByte();
            Spell = (Spell) reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
            writer.Write((byte)Spell);
        }
    }
    public sealed class RangeAttack : Packet //ArcherTest
    {
        public override short Index { get { return (short)ClientPacketIds.RangeAttack; } }

        public MirDirection Direction;
        public Point Location;
        public uint TargetID;
        public Point TargetLocation;

        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection)reader.ReadByte();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            TargetID = reader.ReadUInt32();
            TargetLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(TargetID);
            writer.Write(TargetLocation.X);
            writer.Write(TargetLocation.Y);
        }
    }
    public sealed class Harvest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Harvest; } }

        public MirDirection Direction;
        protected override void ReadPacket(BinaryReader reader)
        {
            Direction = (MirDirection)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Direction);
        }
    }
    public sealed class CallNPC : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CallNPC; } }

        public uint ObjectID;
        public string Key = string.Empty;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Key = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Key);
        }
    }
    public sealed class TalkMonsterNPC : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TalkMonsterNPC; } }

        public uint ObjectID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
        }
    }
    public sealed class BuyItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.BuyItem; } }

        public ulong ItemIndex;
        public uint Count;
        public PanelType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            ItemIndex = reader.ReadUInt64();
            Count = reader.ReadUInt32();
            Type = (PanelType)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ItemIndex);
            writer.Write(Count);
            writer.Write((byte)Type);
        }
    }
    public sealed class SellItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SellItem; } }

        public ulong UniqueID;
        public uint Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }
    public sealed class CraftItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CraftItem; } }

        public ulong UniqueID;
        public uint Count;
        public int[] Slots;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt32();
            Slots = new int[reader.ReadInt32()];

            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i] = reader.ReadInt32();
            }
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
            writer.Write(Slots.Length);

            for (int i = 0; i < Slots.Length; i++)
            {
                writer.Write(Slots[i]);
            }
        }
    }
    public sealed class RepairItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RepairItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }
    public sealed class BuyItemBack : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.BuyItemBack; } }

        public ulong UniqueID;
        public uint Count;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Count = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Count);
        }
    }
    public sealed class SRepairItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SRepairItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }
    public sealed class MagicKey : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MagicKey; } }

        public Spell Spell;
        public byte Key;

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell) reader.ReadByte();
            Key = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte) Spell);
            writer.Write(Key);
        }
    }
    public sealed class Magic : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Magic; } }

        public Spell Spell;
        public MirDirection Direction;
        public uint TargetID;
        public Point Location;

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell) reader.ReadByte();
            Direction = (MirDirection)reader.ReadByte();
            TargetID = reader.ReadUInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte) Spell);
            writer.Write((byte)Direction);
            writer.Write(TargetID);
            writer.Write(Location.X);
            writer.Write(Location.Y);
        }
    }

    public sealed class SwitchGroup : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SwitchGroup; } }

        public bool AllowGroup;
        protected override void ReadPacket(BinaryReader reader)
        {
            AllowGroup = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AllowGroup);
        }
    }
    public sealed class AddMember : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AddMember; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class DelMember : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DellMember; } }

        public string Name = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GroupInvite : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.GroupInvite; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }

    public sealed class MarriageRequest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarriageRequest; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class MarriageReply : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarriageReply; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }

    public sealed class ChangeMarriage : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ChangeMarriage; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class DivorceRequest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DivorceRequest; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class DivorceReply : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DivorceReply; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }

    public sealed class AddMentor : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AddMentor; } }

        public string Name;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }

    public sealed class MentorReply : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MentorReply; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }

    public sealed class AllowMentor : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AllowMentor; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class CancelMentor : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CancelMentor; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }


    public sealed class TradeReply : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TradeReply; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }
    public sealed class TradeRequest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TradeRequest; } }

        protected override void ReadPacket(BinaryReader reader)
        {  }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class TradeGold : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TradeGold; } }

        public uint Amount;
        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }
    public sealed class TradeConfirm : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TradeConfirm; } }

        public bool Locked;
        protected override void ReadPacket(BinaryReader reader)
        {
            Locked = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Locked);
        }
    }
    public sealed class TradeCancel : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TradeCancel; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class TownRevive : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.TownRevive; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class SpellToggle : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SpellToggle; } }
        public Spell Spell;
        public bool CanUse;

        protected override void ReadPacket(BinaryReader reader)
        {
            Spell = (Spell)reader.ReadByte();
            CanUse = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Spell);
            writer.Write(CanUse);
        }
    }
    public sealed class ConsignItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ConsignItem; } }

        public ulong UniqueID;
        public uint Price;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Price = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Price);
        }
    }
    public sealed class MarketSearch : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarketSearch; } }

        public string Match = string.Empty;
        protected override void ReadPacket(BinaryReader reader)
        {
            Match = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Match);
        }
    }
    public sealed class MarketRefresh : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarketRefresh; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class MarketPage : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarketPage; } }
        public int Page;

        protected override void ReadPacket(BinaryReader reader)
        {
            Page = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Page);
        }
    }
    public sealed class MarketBuy : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarketBuy; } }

        public ulong AuctionID;

        protected override void ReadPacket(BinaryReader reader)
        {
            AuctionID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AuctionID);
        }
    }
    public sealed class MarketGetBack : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MarketGetBack; } }

        public ulong AuctionID;

        protected override void ReadPacket(BinaryReader reader)
        {
            AuctionID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AuctionID);
        }
    }
    public sealed class RequestUserName : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RequestUserName; } }

        public uint UserID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UserID = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UserID);
        }
    }
    public sealed class RequestChatItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RequestChatItem; } }

        public ulong ChatItemID;

        protected override void ReadPacket(BinaryReader reader)
        {
            ChatItemID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ChatItemID);
        }
    }
    public sealed class EditGuildMember : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.EditGuildMember; } }

        public byte ChangeType = 0;
        public byte RankIndex = 0;
        public string Name = "";
        public string RankName = "";

        protected override void ReadPacket(BinaryReader reader)
        {
            ChangeType = reader.ReadByte();
            RankIndex = reader.ReadByte();
            Name = reader.ReadString();
            RankName = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ChangeType);
            writer.Write(RankIndex);
            writer.Write(Name);
            writer.Write(RankName);
        }
    }
    public sealed class EditGuildNotice : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.EditGuildNotice; } }

        public List<string> notice = new List<string>();

        protected override void ReadPacket(BinaryReader reader)
        {
            int LineCount = reader.ReadInt32();
            for (int i = 0; i < LineCount; i++)
                notice.Add(reader.ReadString());
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(notice.Count);
            for (int i = 0; i < notice.Count; i++)
                writer.Write(notice[i]);
        }
    }
    public sealed class GuildInvite : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.GuildInvite; } }

        public bool AcceptInvite;
        protected override void ReadPacket(BinaryReader reader)
        {
            AcceptInvite = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AcceptInvite);
        }
    }
    public sealed class RequestGuildInfo : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.RequestGuildInfo; } 
        }
        public byte Type;
        protected override void ReadPacket(BinaryReader reader)
        {
            Type = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Type);
        }
    }
    public sealed class GuildNameReturn : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.GuildNameReturn; }
        }
        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GuildWarReturn : Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.GuildWarReturn; }
        }
        public string Name;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
        }
    }
    public sealed class GuildStorageGoldChange: Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.GuildStorageGoldChange; }
        }
        public byte Type = 0;
        public uint Amount = 0;        
        protected override void ReadPacket(BinaryReader reader)
        {
            Type = reader.ReadByte();
            Amount = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(Amount);
        }
    }
    public sealed class GuildStorageItemChange: Packet
    {
        public override short Index
        {
            get { return (short)ClientPacketIds.GuildStorageItemChange; }
        }
        public byte Type = 0;
        public int From, To;
        protected override void ReadPacket(BinaryReader reader)
        {
            Type = reader.ReadByte();
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class EquipSlotItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.EquipSlotItem; } }

        public MirGridType Grid;
        public ulong UniqueID;
        public int To;
        public MirGridType GridTo;

        protected override void ReadPacket(BinaryReader reader)
        {
            Grid = (MirGridType)reader.ReadByte();
            UniqueID = reader.ReadUInt64();
            To = reader.ReadInt32();
            GridTo = (MirGridType)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Grid);
            writer.Write(UniqueID);
            writer.Write(To);
            writer.Write((byte)GridTo);
        }
    }

    public sealed class FishingCast : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.FishingCast; } }

        public bool CastOut;

        protected override void ReadPacket(BinaryReader reader)
        {
            CastOut = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CastOut);
        }
    }

    public sealed class FishingChangeAutocast : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.FishingChangeAutocast; } }

        public bool AutoCast;

        protected override void ReadPacket(BinaryReader reader)
        {
            AutoCast = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(AutoCast);
        }
    }

    public sealed class AcceptQuest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AcceptQuest; } }

        public uint NPCIndex;
        public int QuestIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            NPCIndex = reader.ReadUInt32();
            QuestIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(NPCIndex);
            writer.Write(QuestIndex);
        }
    }

    public sealed class FinishQuest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.FinishQuest; } }

        public int QuestIndex;
        public int SelectedItemIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            QuestIndex = reader.ReadInt32();
            SelectedItemIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(QuestIndex);
            writer.Write(SelectedItemIndex);
        }
    }

    public sealed class AbandonQuest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AbandonQuest; } }

        public int QuestIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            QuestIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(QuestIndex);
        }
    }

    public sealed class ShareQuest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ShareQuest; } }

        public int QuestIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            QuestIndex = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(QuestIndex);
        }
    }

    public sealed class AcceptReincarnation : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AcceptReincarnation; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class CancelReincarnation : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CancelReincarnation; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

    public sealed class CombineItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CombineItem; } }

        public ulong IDFrom, IDTo;
        protected override void ReadPacket(BinaryReader reader)
        {
            IDFrom = reader.ReadUInt64();
            IDTo = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(IDFrom);
            writer.Write(IDTo);
        }
    }

    public sealed class SetConcentration : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SetConcentration; } }

        public uint ObjectID;
        public bool Enabled;
        public bool Interrupted;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = reader.ReadUInt32();
            Enabled = reader.ReadBoolean();
            Interrupted = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID);
            writer.Write(Enabled);
            writer.Write(Interrupted);
        }
    }
public sealed class AwakeningNeedMaterials : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AwakeningNeedMaterials; } }

        public ulong UniqueID;
        public AwakeType Type;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Type = (AwakeType)reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write((byte)Type);
        }
    }

    public sealed class AwakeningLockedItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AwakeningLockedItem; } }

        public ulong UniqueID;
        public bool Locked;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Locked = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Locked);
        }
    }

    public sealed class Awakening : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Awakening; } }

        public ulong UniqueID;
        public AwakeType Type;
        public uint PositionIdx;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Type = (AwakeType)reader.ReadByte();
            PositionIdx = reader.ReadUInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write((byte)Type);
            writer.Write(PositionIdx);
        }
    }

    public sealed class DisassembleItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DisassembleItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class DowngradeAwakening : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DowngradeAwakening; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class ResetAddedItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ResetAddedItem; } }

        public ulong UniqueID;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
        }
    }

    public sealed class SendMail : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.SendMail; } }

        public string Name;
        public string Message;
        public uint Gold;
        public ulong[] ItemsIdx = new ulong[5];
        public bool Stamped;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Message = reader.ReadString();
            Gold = reader.ReadUInt32();

            for (int i = 0; i < 5; i++)
            {
                ItemsIdx[i] = reader.ReadUInt64();
            }

            Stamped = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Message);
            writer.Write(Gold);

            for (int i = 0; i < 5; i++)
            {
                writer.Write(ItemsIdx[i]);
            }

            writer.Write(Stamped);
        }
    }

    public sealed class ReadMail : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ReadMail; } }

        public ulong MailID;

        protected override void ReadPacket(BinaryReader reader)
        {
            MailID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MailID);
        }
    }

    public sealed class CollectParcel : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CollectParcel; } }

        public ulong MailID;

        protected override void ReadPacket(BinaryReader reader)
        {
            MailID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MailID);
        }
    }

    public sealed class DeleteMail : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DeleteMail; } }

        public ulong MailID;

        protected override void ReadPacket(BinaryReader reader)
        {
            MailID = reader.ReadUInt64();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MailID);
        }
    }

    public sealed class LockMail : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.LockMail; } }

        public ulong MailID;
        public bool Lock;

        protected override void ReadPacket(BinaryReader reader)
        {
            MailID = reader.ReadUInt64();
            Lock = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MailID);
            writer.Write(Lock);
        }
    }

    public sealed class MailLockedItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MailLockedItem; } }

        public ulong UniqueID;
        public bool Locked;

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = reader.ReadUInt64();
            Locked = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UniqueID);
            writer.Write(Locked);
        }
    }

    public sealed class MailCost : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.MailCost; } }

        public uint Gold;
        public ulong[] ItemsIdx = new ulong[5];
        public bool Stamped;

        protected override void ReadPacket(BinaryReader reader)
        {
            Gold = reader.ReadUInt32();

            for (int i = 0; i < 5; i++)
            {
                ItemsIdx[i] = reader.ReadUInt64();
            }

            Stamped = reader.ReadBoolean();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Gold);

            for (int i = 0; i < 5; i++)
            {
                writer.Write(ItemsIdx[i]);
            }

            writer.Write(Stamped);
        }
    }

    public sealed class UpdateIntelligentCreature : Packet//IntelligentCreature
    {
        public override short Index { get { return (short)ClientPacketIds.UpdateIntelligentCreature; } }


        public ClientIntelligentCreature Creature;
        public bool SummonMe = false;
        public bool UnSummonMe = false;
        public bool ReleaseMe = false;

        protected override void ReadPacket(BinaryReader reader)
        {
            Creature = new ClientIntelligentCreature(reader);
            SummonMe = reader.ReadBoolean();
            UnSummonMe = reader.ReadBoolean();
            ReleaseMe = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            Creature.Save(writer);
            writer.Write(SummonMe);
            writer.Write(UnSummonMe);
            writer.Write(ReleaseMe);
        }
    }

    public sealed class IntelligentCreaturePickup : Packet//IntelligentCreature
    {
        public override short Index { get { return (short)ClientPacketIds.IntelligentCreaturePickup; } }

        public bool MouseMode = false;
        public Point Location = new Point(0,0);

        protected override void ReadPacket(BinaryReader reader)
        {
            MouseMode = reader.ReadBoolean();
            Location.X = reader.ReadInt32();
            Location.Y = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MouseMode);
            writer.Write(Location.X);
            writer.Write(Location.Y);
        }
    }

    public sealed class AddFriend : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AddFriend; } }

        public string Name;
        public bool Blocked;

        protected override void ReadPacket(BinaryReader reader)
        {
            Name = reader.ReadString();
            Blocked = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(Blocked);
        }
    }

    public sealed class RemoveFriend : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RemoveFriend; } }

        public int CharacterIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
        }
    }

    public sealed class RefreshFriends : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RefreshFriends; } }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }


    public sealed class AddMemo : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.AddMemo; } }

        public int CharacterIndex;
        public string Memo;

        protected override void ReadPacket(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
            Memo = reader.ReadString();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
            writer.Write(Memo);
        }
    }

    public sealed class GuildBuffUpdate : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.GuildBuffUpdate; } }

        public byte Action = 0; //0 = request list, 1 = request a buff to be enabled, 2 = request a buff to be activated
        public int Id;

        protected override void ReadPacket(BinaryReader reader)
        {
            Action = reader.ReadByte();
            Id = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Action);
            writer.Write(Id);
        }
    }

    public sealed class GameshopBuy : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.GameshopBuy; } }

        public int GIndex;
        public byte Quantity;

        protected override void ReadPacket(BinaryReader reader)
        {
            GIndex = reader.ReadInt32();
            Quantity = reader.ReadByte();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(GIndex);
            writer.Write(Quantity);
        }
    }

    public sealed class NPCConfirmInput : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.NPCConfirmInput; } }

        public uint NPCID;
        public string PageName;
        public string Value;

        protected override void ReadPacket(BinaryReader reader)
        {
            NPCID = reader.ReadUInt32();
            PageName = reader.ReadString();
            Value = reader.ReadString();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(NPCID);
            writer.Write(PageName);
            writer.Write(Value);
        }
    }

    public sealed class ReportIssue : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ReportIssue; } }

        public byte[] Image;
        public int ImageSize;
        public int ImageChunk;

        public string Message;

        protected override void ReadPacket(BinaryReader reader)
        {
            Image = reader.ReadBytes(reader.ReadInt32());
            ImageSize = reader.ReadInt32();
            ImageChunk = reader.ReadInt32();
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Image.Length);
            writer.Write(Image);
            writer.Write(ImageSize);
            writer.Write(ImageChunk);
        }
    }
    public sealed class GetRanking : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.GetRanking; } }
        public byte RankIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            RankIndex = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(RankIndex);
        }
    }

    public sealed class Opendoor : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.Opendoor; } }
        public byte DoorIndex;

        protected override void ReadPacket(BinaryReader reader)
        {
            DoorIndex = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(DoorIndex);
        }
    }

    public sealed class GetRentedItems : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ClientPacketIds.GetRentedItems;
            }
        }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ItemRentalRequest : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ItemRentalRequest; } }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ItemRentalFee : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ItemRentalFee; } }

        public uint Amount;

        protected override void ReadPacket(BinaryReader reader)
        {
            Amount = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Amount);
        }
    }

    public sealed class ItemRentalPeriod : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ItemRentalPeriod; } }

        public uint Days;

        protected override void ReadPacket(BinaryReader reader)
        {
            Days = reader.ReadUInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Days);
        }
    }

    public sealed class DepositRentalItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.DepositRentalItem; } }

        public int From, To;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class RetrieveRentalItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.RetrieveRentalItem; } }

        public int From, To;

        protected override void ReadPacket(BinaryReader reader)
        {
            From = reader.ReadInt32();
            To = reader.ReadInt32();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(From);
            writer.Write(To);
        }
    }

    public sealed class CancelItemRental : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.CancelItemRental; } }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ItemRentalLockFee : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ItemRentalLockFee; } }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ItemRentalLockItem : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ItemRentalLockItem; } }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }

    public sealed class ConfirmItemRental : Packet
    {
        public override short Index { get { return (short)ClientPacketIds.ConfirmItemRental; } }

        protected override void ReadPacket(BinaryReader reader)
        { }

        protected override void WritePacket(BinaryWriter writer)
        { }
    }
}

using System.IO;

public class ClientMonsterInfo
{
    public int Index;
    public string Name = string.Empty;
    public Monster Image;
    public byte AI, Effect, ViewRange, CoolEye;
    public ushort Level;
    public byte Light;
    public ushort AttackSpeed, MoveSpeed;
    public uint Experience;
    public bool CanTame, CanPush, AutoRev, Undead, CanRecall;
    // Stats for detailed tooltip
    public int HP;
    public ushort MinAC, MaxAC, MinMAC, MaxMAC;
    public ushort MinDC, MaxDC, MinMC, MaxMC, MinSC, MaxSC;

    public ClientMonsterInfo()
    {
    }

    public ClientMonsterInfo(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Image = (Monster)reader.ReadUInt16();
        AI = reader.ReadByte();
        Effect = reader.ReadByte();
        Level = reader.ReadUInt16();
        ViewRange = reader.ReadByte();
        CoolEye = reader.ReadByte();
        Light = reader.ReadByte();
        AttackSpeed = reader.ReadUInt16();
        MoveSpeed = reader.ReadUInt16();
        Experience = reader.ReadUInt32();
        CanPush = reader.ReadBoolean();
        CanTame = reader.ReadBoolean();
        AutoRev = reader.ReadBoolean();
        Undead = reader.ReadBoolean();
        CanRecall = reader.ReadBoolean();
        // Read stats
        HP = reader.ReadInt32();
        MinAC = reader.ReadUInt16();
        MaxAC = reader.ReadUInt16();
        MinMAC = reader.ReadUInt16();
        MaxMAC = reader.ReadUInt16();
        MinDC = reader.ReadUInt16();
        MaxDC = reader.ReadUInt16();
        MinMC = reader.ReadUInt16();
        MaxMC = reader.ReadUInt16();
        MinSC = reader.ReadUInt16();
        MaxSC = reader.ReadUInt16();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write((ushort)Image);
        writer.Write(AI);
        writer.Write(Effect);
        writer.Write(Level);
        writer.Write(ViewRange);
        writer.Write(CoolEye);
        writer.Write(Light);
        writer.Write(AttackSpeed);
        writer.Write(MoveSpeed);
        writer.Write(Experience);
        writer.Write(CanPush);
        writer.Write(CanTame);
        writer.Write(AutoRev);
        writer.Write(Undead);
        writer.Write(CanRecall);
        // Write stats
        writer.Write(HP);
        writer.Write(MinAC);
        writer.Write(MaxAC);
        writer.Write(MinMAC);
        writer.Write(MaxMAC);
        writer.Write(MinDC);
        writer.Write(MaxDC);
        writer.Write(MinMC);
        writer.Write(MaxMC);
        writer.Write(MinSC);
        writer.Write(MaxSC);
    }

}


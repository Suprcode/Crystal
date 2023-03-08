using System.Drawing;

public class SelectInfo
{
    public int Index;
    public string Name = string.Empty;
    public ushort Level;
    public MirClass Class;
    public MirGender Gender;
    public DateTime LastAccess;

    public SelectInfo() { }

    public SelectInfo(BinaryReader reader)
    {
        Index = reader.ReadInt32();
        Name = reader.ReadString();
        Level = reader.ReadUInt16();
        Class = (MirClass)reader.ReadByte();
        Gender = (MirGender)reader.ReadByte();
        LastAccess = DateTime.FromBinary(reader.ReadInt64());
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(Index);
        writer.Write(Name);
        writer.Write(Level);
        writer.Write((byte)Class);
        writer.Write((byte)Gender);
        writer.Write(LastAccess.ToBinary());
    }
}

public class Door
{
    public byte index;
    public DoorState DoorState;
    public byte ImageIndex;
    public long LastTick;
    public Point Location;
}

public class RankCharacterInfo
{
    public long PlayerId;
    public string Name;
    public MirClass Class;
    public int level;

    public long Experience;//clients shouldnt care about this only server
    public object info;//again only keep this on server!
    public DateTime LastUpdated;

    public RankCharacterInfo()
    {

    }
    public RankCharacterInfo(BinaryReader reader)
    {
        PlayerId = reader.ReadInt64();
        Name = reader.ReadString();
        level = reader.ReadInt32();
        Class = (MirClass)reader.ReadByte();

    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(PlayerId);
        writer.Write(Name);
        writer.Write(level);
        writer.Write((byte)Class);
    }
}

public class QuestItemReward
{
    public ItemInfo Item;
    public ushort Count = 1;

    public QuestItemReward() { }

    public QuestItemReward(BinaryReader reader)
    {
        Item = new ItemInfo(reader);
        Count = reader.ReadUInt16();
    }

    public void Save(BinaryWriter writer)
    {
        Item.Save(writer);
        writer.Write(Count);
    }
}

public class WorldMapSetup
{
    public bool Enabled;
    public List<WorldMapIcon> Icons = new List<WorldMapIcon>();

    public WorldMapSetup() { }

    public WorldMapSetup(BinaryReader reader)
    {
        Enabled = reader.ReadBoolean();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            Icons.Add(new WorldMapIcon(reader));
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Enabled);
        writer.Write(Icons.Count);
        for (int i = 0; i < Icons.Count; i++)
            Icons[i].Save(writer);
    }
}
public class WorldMapIcon
{
    public int ImageIndex;
    public string Title;
    public int MapIndex;
    public WorldMapIcon() { }

    public WorldMapIcon(BinaryReader reader)
    {
        ImageIndex = reader.ReadInt32();
        Title = reader.ReadString();
        MapIndex = reader.ReadInt32();
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(ImageIndex);
        writer.Write(Title);
        writer.Write(MapIndex);
    }
}
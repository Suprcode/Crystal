namespace Server.MirDatabase
{
    public class MineSet
    {
        public string Name = string.Empty;
        public byte SpotRegenRate = 5;
        public byte MaxStones = 80;
        public byte HitRate = 25;
        public byte DropRate = 10;
        public byte TotalSlots = 100;
        public List<MineDrop> Drops = new List<MineDrop>();
        private bool DropsSet = false;

        public MineSet(byte mineType = 0)
        {
            switch (mineType)
            {
                case 1:
                    TotalSlots = 120;
                    Drops.Add(new MineDrop() { ItemName = "GoldOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "SilverOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "CopperOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "BlackIronOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    break;
                case 2:
                    TotalSlots = 100;
                    Drops.Add(new MineDrop() { ItemName = "PlatinumOre", MinSlot = 1, MaxSlot = 2, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "RubyOre", MinSlot = 3, MaxSlot = 20, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "NephriteOre", MinSlot = 21, MaxSlot = 45, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    Drops.Add(new MineDrop() { ItemName = "AmethystOre", MinSlot = 46, MaxSlot = 56, MinDura = 3, MaxDura = 16, BonusChance = 20, MaxBonusDura = 10 });
                    break;
            }
        }

        public void SetDrops(List<ItemInfo> items)
        {
            if (DropsSet) return;
            for (int i = 0; i < Drops.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    ItemInfo info = items[j];
                    if (String.Compare(info.Name.Replace(" ", ""), Drops[i].ItemName, StringComparison.OrdinalIgnoreCase) != 0) continue;
                    Drops[i].Item = info;
                    break;
                }
            }
            DropsSet = true;
        }
    }

    public class MineSpot
    {
        public byte StonesLeft = 0;
        public long LastRegenTick = 0;
        public MineSet Mine;
    }

    public class MineDrop
    {
        public string ItemName;
        public ItemInfo Item;
        public byte MinSlot = 0;
        public byte MaxSlot = 0;
        public byte MinDura = 1;
        public byte MaxDura = 1;
        public byte BonusChance = 0;
        public byte MaxBonusDura = 1;
    }

    public class MineZone
    {
        public byte Mine;
        public Point Location;
        public ushort Size;

        public MineZone()
        {
        }

        public MineZone(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Mine = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(Mine);
        }
        public override string ToString()
        {
            return string.Format("Mine: {0}- {1}", Functions.PointToString(Location), Mine);
        }
    }
}

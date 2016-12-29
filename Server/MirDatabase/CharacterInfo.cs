using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using Server.MirEnvir;
using Server.MirNetwork;
using Server.MirObjects;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Server.MirDatabase
{
    public class CharacterInfo
    {
        [Key]
        public int Index { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public ushort Level { get; set; }

        public int DBLevel
        {
            get { return Level; }
            set { Level = (ushort)value; }
        }

        public MirClass Class { get; set; }
        public MirGender Gender { get; set; }
        public byte Hair { get; set; }
        public int GuildIndex { get; set; } = -1;

        public string CreationIP { get; set; }
        public DateTime? CreationDate { get; set; } = SqlDateTime.MinValue.Value;

        public bool Banned { get; set; }
        public string BanReason { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; } = SqlDateTime.MinValue.Value;

        public bool ChatBanned { get; set; }
        public DateTime? ChatBanExpiryDate { get; set; } = SqlDateTime.MinValue.Value;

        public string LastIP { get; set; } = string.Empty;
        public DateTime? LastDate { get; set; } = SqlDateTime.MinValue.Value;

        public bool Deleted { get; set; }
        public DateTime? DeleteDate { get; set; } = SqlDateTime.MinValue.Value;

        public ListViewItem ListItem;

        //Marriage
        public int Married { get; set; } = 0;
        public DateTime? MarriedDate { get; set; }

        //Mentor
        public int Mentor { get; set; } = 0;
        public DateTime? MentorDate { get; set; } = SqlDateTime.MinValue.Value;
        public bool isMentor { get; set; }
        public long MentorExp { get; set; } = 0;

        //Location
        public int CurrentMapIndex { get; set; }
        public Point CurrentLocation;

        public string DBCurrentLocation
        {
            get { return CurrentLocation.X + "," + CurrentLocation.Y; }
            set
            {
                if(string.IsNullOrEmpty(value)) return;
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    CurrentLocation.X = 0;
                    CurrentLocation.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    CurrentLocation.X = result;
                    int.TryParse(tempArray[1], out result);
                    CurrentLocation.Y = result;
                }
            }
        }
        public MirDirection Direction { get; set; }
        public int BindMapIndex { get; set; }
        public Point BindLocation;

        public string DBBindLocatoin
        {
            get { return BindLocation.X + "," + BindLocation.Y; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    BindLocation.X = 0;
                    BindLocation.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    BindLocation.X = result;
                    int.TryParse(tempArray[1], out result);
                    BindLocation.Y = result;
                }
            }
        }
        [NotMapped]
        public ushort HP { get; set; }

        public int DBHP
        {
            get { return HP; }
            set { HP = (ushort) value; }
        }
        [NotMapped]
        public ushort MP { get; set; }

        public int DBMP
        {
            get { return MP; }
            set { MP = (ushort) value; }
        }
        public long Experience { get; set; }

        public AttackMode AMode { get; set; }
        public PetMode PMode { get; set; }
        public bool AllowGroup { get; set; }
        public bool AllowTrade { get; set; }

        public int PKPoints { get; set; }

        public bool NewDay;

        public bool Thrusting { get; set; }
        public bool HalfMoon { get; set; }
        public bool CrossHalfMoon { get; set; }
        public bool DoubleSlash { get; set; }
        public byte MentalState { get; set; }
        public byte MentalStateLvl { get; set; }

        public UserItem[] Inventory = new UserItem[46];
        public UserItem[] Equipment = new UserItem[14];
        public UserItem[] Trade = new UserItem[10];
        public UserItem[] QuestInventory = new UserItem[40];
        public UserItem[] Refine = new UserItem[16];
        public UserItem CurrentRefine = null;
        public long CollectTime { get; set; } = 0;
        public List<UserMagic> Magics = new List<UserMagic>();
        public List<PetInfo> Pets = new List<PetInfo>();
        public List<Buff> Buffs = new List<Buff>();
        public List<Poison> Poisons = new List<Poison>();
        public List<MailInfo> Mail = new List<MailInfo>();
        public List<FriendInfo> Friends = new List<FriendInfo>();

        //IntelligentCreature
        public List<UserIntelligentCreature> IntelligentCreatures = new List<UserIntelligentCreature>();
        public int PearlCount { get; set; }

        public List<QuestProgressInfo> CurrentQuests = new List<QuestProgressInfo>();
        public List<int> CompletedQuests = new List<int>();

        public string DbCompletedQuests
        {
            get { return string.Join(",", CompletedQuests); }
            set { CompletedQuests = string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList(); }
        }

        public bool[] Flags { get; set; } = new bool[Globals.FlagIndexCount];
        public string DbFlags
        {
            get { return string.Join(",", Flags); }
            set { Flags = value.Split(',').Select(bool.Parse).ToArray(); }
        }
        [ForeignKey("AccountInfo")]
        public int AccountInfoIndex { get; set; }
        public AccountInfo AccountInfo { get; set; }

        public PlayerObject Player;
        public MountInfo Mount;

        public Dictionary<int, int> GSpurchases = new Dictionary<int, int>();
        [NotMapped]
        public int[] Rank = new int[2];//dont save this in db!(and dont send it to clients :p)

        public CharacterInfo()
        {
        }

        public CharacterInfo(ClientPackets.NewCharacter p, MirConnection c)
        {
            Name = p.Name;
            Class = p.Class;
            Gender = p.Gender;

            CreationIP = c.IPAddress;
            CreationDate = SMain.Envir.Now;
        }

        public CharacterInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();

             if (Envir.LoadVersion < 62)
             {
                 Level = (ushort)reader.ReadByte();
             }
             else
             {
                 Level = reader.ReadUInt16();
             }
 
            Class = (MirClass) reader.ReadByte();
            Gender = (MirGender) reader.ReadByte();
            Hair = reader.ReadByte();

            CreationIP = reader.ReadString();
            CreationDate = DateTime.FromBinary(reader.ReadInt64());

            Banned = reader.ReadBoolean();
            BanReason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());

            LastIP = reader.ReadString();
            LastDate = DateTime.FromBinary(reader.ReadInt64());

            Deleted = reader.ReadBoolean();
            DeleteDate = DateTime.FromBinary(reader.ReadInt64());

            CurrentMapIndex = reader.ReadInt32();
            CurrentLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
            Direction = (MirDirection)reader.ReadByte();
            BindMapIndex = reader.ReadInt32();
            BindLocation = new Point(reader.ReadInt32(), reader.ReadInt32());

            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            Experience = reader.ReadInt64();
            
            AMode = (AttackMode) reader.ReadByte();
            PMode = (PetMode) reader.ReadByte();

            if (Envir.LoadVersion > 34)
            {
                PKPoints = reader.ReadInt32();
            }

            int count = reader.ReadInt32();

            Array.Resize(ref Inventory, count);

            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                if (SMain.Envir.BindItem(item) && i < Inventory.Length)
                    Inventory[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                if (SMain.Envir.BindItem(item) && i < Equipment.Length)
                    Equipment[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                if (SMain.Envir.BindItem(item) && i < QuestInventory.Length)
                    QuestInventory[i] = item;
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UserMagic magic = new UserMagic(reader);
                if (magic.Info == null) continue;
                Magics.Add(magic);
            }
            //reset all magic cooldowns on char loading < stops ppl from having none working skills after a server crash
            for (int i = 0; i < Magics.Count; i++)
            {
                Magics[i].CastTime = 0;
            }

            if (Envir.LoadVersion < 2) return;

            Thrusting = reader.ReadBoolean();
            HalfMoon = reader.ReadBoolean();
            CrossHalfMoon = reader.ReadBoolean();
            DoubleSlash = reader.ReadBoolean();

            if(Envir.LoadVersion > 46)
            {
                MentalState = reader.ReadByte();
            }

            if (Envir.LoadVersion < 4) return;

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Pets.Add(new PetInfo(reader));


            if (Envir.LoadVersion < 5) return;

            AllowGroup = reader.ReadBoolean();

            if (Envir.LoadVersion < 12) return;

            if (Envir.LoadVersion == 12) count = reader.ReadInt32();

            for (int i = 0; i < Globals.FlagIndexCount; i++)
                Flags[i] = reader.ReadBoolean();

            if (Envir.LoadVersion > 27)
                GuildIndex = reader.ReadInt32();

            if (Envir.LoadVersion > 30)
                AllowTrade = reader.ReadBoolean();

            if (Envir.LoadVersion > 33)
            {
                count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    QuestProgressInfo quest = new QuestProgressInfo(reader);
                    if (SMain.Envir.BindQuest(quest))
                        CurrentQuests.Add(quest);
                }
            }

            if(Envir.LoadVersion > 42)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Buff buff = new Buff(reader);

                    if (Envir.LoadVersion == 51)
                    {
                        buff.Caster = SMain.Envir.GetObject(reader.ReadUInt32());
                    }

                    Buffs.Add(buff);
                }
            }

            if(Envir.LoadVersion > 43)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    Mail.Add(new MailInfo(reader, Envir.LoadVersion, Envir.LoadCustomVersion));
            }

            //IntelligentCreature
            if (Envir.LoadVersion > 44)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    UserIntelligentCreature creature = new UserIntelligentCreature(reader);
                    if (creature.Info == null) continue;
                    IntelligentCreatures.Add(creature);
                }

                if (Envir.LoadVersion == 45)
                {
                    var old1 = (IntelligentCreatureType)reader.ReadByte();
                    var old2 = reader.ReadBoolean();
                }

                PearlCount = reader.ReadInt32();
            }

            if (Envir.LoadVersion > 49)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    CompletedQuests.Add(reader.ReadInt32());
            }

            if (Envir.LoadVersion > 50 && Envir.LoadVersion < 54)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Poison poison = new Poison(reader);

                    if (Envir.LoadVersion == 51)
                    {
                        poison.Owner = SMain.Envir.GetObject(reader.ReadUInt32());
                    }

                    Poisons.Add(poison);
                }
            }

            if (Envir.LoadVersion > 56)
            {
                if (reader.ReadBoolean()) CurrentRefine = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                  if (CurrentRefine != null)
                    SMain.Envir.BindItem(CurrentRefine);

                CollectTime = reader.ReadInt64();
                CollectTime += SMain.Envir.Time;
            }

            if (Envir.LoadVersion > 58)
            {
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    Friends.Add(new FriendInfo(reader));
            }

            if (Envir.LoadVersion > 59)
            {
                Married = reader.ReadInt32();
                MarriedDate = DateTime.FromBinary(reader.ReadInt64());
                Mentor = reader.ReadInt32();
                MentorDate = DateTime.FromBinary(reader.ReadInt64());
                isMentor = reader.ReadBoolean();
                MentorExp = reader.ReadInt64();
            }

            if (Envir.LoadVersion >= 63)
            {
                int logCount = reader.ReadInt32();

                for (int i = 0; i < logCount; i++)
                {
                    GSpurchases.Add(reader.ReadInt32(), reader.ReadInt32());
                }
            }

        }

        public void Save(BinaryWriter writer)
        {
            if (Settings.UseSQLServer)
            {
                using (var ctx = new DataContext())
                {
                    var dbCharacter = ctx.CharacterInfos.FirstOrDefault(c => c.Index == Index);
                    if (dbCharacter == null)
                    {
                        dbCharacter = this;
                        dbCharacter.AccountInfo = null;
                        ctx.CharacterInfos.Add(dbCharacter);
                    }
                    else
                    {
                        ctx.Entry(dbCharacter).CurrentValues.SetValues(this);
                    }

                    ctx.SaveChanges();
                    //ctx.CharacterInfos.Attach(this);


                    var dbCurrentRefine = ctx.CurrentRefines.FirstOrDefault(r => r.CharacterIndex == Index);
                    if (dbCurrentRefine == null)
                    {
                        if (CurrentRefine != null)
                        {
                            var currentRefine = CurrentRefine;
                            currentRefine.Info = null;
                            ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, currentRefine);
                            ctx.CurrentRefines.Add(new CurrentRefineItem()
                            {
                                CharacterIndex = Index,
                                ItemUniqueID = CurrentRefine.UniqueID
                            });
                            if ((CollectTime - SMain.Envir.Time) < 0)
                                CollectTime = 0;
                            else
                                CollectTime = CollectTime - SMain.Envir.Time;
                        }
                    }
                    else
                    {
                        if (CurrentRefine == null)
                        {
                            ctx.CurrentRefines.Remove(dbCurrentRefine);
                            CollectTime = 0;
                        }
                        else
                        {
                            var currentRefine = CurrentRefine;
                            currentRefine.Info = null;
                            ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, currentRefine);
                            dbCurrentRefine.CharacterIndex = Index;
                            dbCurrentRefine.ItemUniqueID = CurrentRefine.UniqueID;
                            if ((CollectTime - SMain.Envir.Time) < 0)
                                CollectTime = 0;
                            else
                                CollectTime = CollectTime - SMain.Envir.Time;
                        }
                    }

                    dbCharacter.CollectTime = CollectTime;
                    ctx.SaveChanges();

                    ctx.Inventories.RemoveRange(ctx.Inventories.Where(i => i.CharacterIndex == Index));
                    foreach (var item in Inventory)
                    {
                        if (item != null)
                        {
                            item.Info = null;
                            ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, item);
                        }

                        ctx.Inventories.Add(new InventoryItem()
                        {
                            CharacterIndex = Index,
                            ItemUniqueID = item?.UniqueID,
                        });
                        ctx.SaveChanges();
                        if(item == null) continue;

                        var dbItem = ctx.UserItems.FirstOrDefault(i => i.UniqueID == item.UniqueID);
                        if (dbItem == null)
                        {
                            ctx.UserItems.Add(item);
                        }
                        else
                        {
                            ctx.Entry(dbItem).CurrentValues.SetValues(item);
                        }
                        
                    }
                    ctx.SaveChanges();
                    ctx.Equipments.RemoveRange(ctx.Equipments.Where(i => i.CharacterIndex == Index));
                    foreach (var item in Equipment)
                    {
                        if (item != null)
                        {
                            item.Info = null;
                            ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, item);
                        }
                            
                        var uid = item?.UniqueID;
                        ctx.Equipments.Add(new EquipmentItem()
                        {
                            CharacterIndex = Index,
                            ItemUniqueID = item?.UniqueID,
                        });
                        ctx.SaveChanges();
                        if(item == null) continue;

                        var dbUserItem = ctx.UserItems.FirstOrDefault(i => i.UniqueID == item.UniqueID);
                        if (dbUserItem == null)
                        {
                            dbUserItem = item;
                            ctx.UserItems.Add(dbUserItem);
                            ctx.SaveChanges();
                        }
                        var dbItem = ctx.UserItems.FirstOrDefault(i => i.UniqueID == item.UniqueID);
                        if (dbItem == null)
                        {
                            ctx.UserItems.Add(item);
                        }
                        else
                        {
                            ctx.Entry(dbItem).CurrentValues.SetValues(item);
                        }
                    }
                    ctx.SaveChanges();
                    ctx.QuestInventories.RemoveRange(ctx.QuestInventories.Where(i => i.CharacterIndex == Index));
                    foreach (var item in QuestInventory)
                    {
                        if (item != null)
                        {
                            item.Info = null;
                            ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, item);
                        }
                        ctx.QuestInventories.Add(new QuestInventoryItem()
                        {
                            CharacterIndex = Index,
                            ItemUniqueID = item?.UniqueID,
                        });
                        ctx.SaveChanges();
                        if (item == null) continue;

                        var dbUserItem = ctx.UserItems.FirstOrDefault(i => i.UniqueID == item.UniqueID);
                        if (dbUserItem == null)
                        {
                            dbUserItem = item;
                            ctx.UserItems.Add(dbUserItem);
                            ctx.SaveChanges();
                        }
                        var dbItem = ctx.UserItems.FirstOrDefault(i => i.UniqueID == item.UniqueID);
                        if (dbItem == null)
                        {
                            ctx.UserItems.Add(item);
                        }
                        else
                        {
                            ctx.Entry(dbItem).CurrentValues.SetValues(item);
                        }
                    }
                    ctx.SaveChanges();
                    foreach (var magic in Magics)
                    {
                        var dbMagic = ctx.UserMagics.FirstOrDefault(m => m.Spell == magic.Spell && m.CharacterIndex == Index);
                        if (dbMagic == null)
                        {
                            magic.CharacterIndex = Index;
                            ctx.UserMagics.Add(magic);
                        }
                        else
                        {
                            ctx.Entry(dbMagic).CurrentValues.SetValues(magic);
                        }
                    }
                    ctx.SaveChanges();
                    /*
                    foreach (var pet in Pets)
                    {
                            
                    }
                    */
                    ctx.QuestProgressInfos.RemoveRange(ctx.QuestProgressInfos.Where(q => q.CharacterIndex == Index));
                    foreach (var questProgressInfo in CurrentQuests)
                    {
                        questProgressInfo.CharacterIndex = Index;
                        ctx.QuestProgressInfos.Add(questProgressInfo);
                        
                    }
                    ctx.SaveChanges();
                    ctx.UserBuffs.RemoveRange(ctx.UserBuffs.Where(b => b.CharacterIndex == Index));
                    foreach (var buff in Buffs)
                    {
                        var userBuff = new UserBuff()
                        {
                            Caster = buff.Caster,
                            CharacterIndex = Index,
                            ExpireTime = buff.ExpireTime,
                            Infinite = buff.Infinite,
                            ObjectID = buff.ObjectID,
                            Paused = buff.Paused,
                            RealTime = buff.RealTime,
                            Type = buff.Type,
                            Visible = buff.Visible,
                            Values = buff.Values,
                        };
                        ctx.UserBuffs.Add(userBuff);
                    }
                    ctx.SaveChanges();
                    ctx.Mails.RemoveRange(ctx.Mails.Where(m => m.RecipientIndex == Index));
                    ctx.SaveChanges();
                    foreach (MailInfo mail in Mail)
                        mail.Save(writer);
                    ctx.UserIntelligentCreatures.RemoveRange(
                        ctx.UserIntelligentCreatures.Where(i => i.CharacterIndex == Index));
                    foreach (var userIntelligentCreature in IntelligentCreatures)
                    {
                        userIntelligentCreature.CharacterIndex = Index;
                        ctx.UserIntelligentCreatures.Add(userIntelligentCreature);
                    }
                    ctx.SaveChanges();
                    ctx.Friends.RemoveRange(ctx.Friends.Where(f => f.CharacterIndex == Index));
                    foreach (var friendInfo in Friends)
                    {
                        friendInfo.CharacterIndex = Index;
                        ctx.Friends.Add(friendInfo);
                    }

                    ctx.GameShopPurchases.RemoveRange(ctx.GameShopPurchases.Where(p => p.CharacterIndex == Index));
                    foreach (var item in GSpurchases)
                    {
                        ctx.GameShopPurchases.Add(new GameShopPurchase()
                        {
                            CharacterIndex = Index,
                            GameShopItemIndex = item.Key,
                            GameShopItemQty = item.Value
                        });
                    }
                    ctx.SaveChanges();
                }
                return;
            }
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Level);
            writer.Write((byte) Class);
            writer.Write((byte) Gender);
            writer.Write(Hair);

            writer.Write(CreationIP);
            writer.Write(CreationDate.GetValueOrDefault().ToBinary());

            writer.Write(Banned);
            writer.Write(BanReason);
            writer.Write(ExpiryDate.GetValueOrDefault().ToBinary());

            writer.Write(LastIP);
            writer.Write(LastDate.GetValueOrDefault().ToBinary());

            writer.Write(Deleted);
            writer.Write(DeleteDate.GetValueOrDefault().ToBinary());

            writer.Write(CurrentMapIndex);
            writer.Write(CurrentLocation.X);
            writer.Write(CurrentLocation.Y);
            writer.Write((byte)Direction);
            writer.Write(BindMapIndex);
            writer.Write(BindLocation.X);
            writer.Write(BindLocation.Y);

            writer.Write(HP);
            writer.Write(MP);
            writer.Write(Experience);

            writer.Write((byte) AMode);
            writer.Write((byte) PMode);

            writer.Write(PKPoints);

            writer.Write(Inventory.Length);
            for (int i = 0; i < Inventory.Length; i++)
            {
                writer.Write(Inventory[i] != null);
                if (Inventory[i] == null) continue;

                Inventory[i].Save(writer);
            }

            writer.Write(Equipment.Length);
            for (int i = 0; i < Equipment.Length; i++)
            {
                writer.Write(Equipment[i] != null);
                if (Equipment[i] == null) continue;

                Equipment[i].Save(writer);
            }

            writer.Write(QuestInventory.Length);
            for (int i = 0; i < QuestInventory.Length; i++)
            {
                writer.Write(QuestInventory[i] != null);
                if (QuestInventory[i] == null) continue;

                QuestInventory[i].Save(writer);
            }

            writer.Write(Magics.Count);
            for (int i = 0; i < Magics.Count; i++)
                Magics[i].Save(writer);

            writer.Write(Thrusting);
            writer.Write(HalfMoon);
            writer.Write(CrossHalfMoon);
            writer.Write(DoubleSlash);
            writer.Write(MentalState);

            writer.Write(Pets.Count);
            for (int i = 0; i < Pets.Count; i++)
                Pets[i].Save(writer);

            writer.Write(AllowGroup);

            for (int i = 0; i < Flags.Length; i++)
                writer.Write(Flags[i]);
            writer.Write(GuildIndex);

            writer.Write(AllowTrade);

            writer.Write(CurrentQuests.Count);
            for (int i = 0; i < CurrentQuests.Count; i++)
                CurrentQuests[i].Save(writer);

            writer.Write(Buffs.Count);
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].Save(writer);
            }

            writer.Write(Mail.Count);
            for (int i = 0; i < Mail.Count; i++)
                Mail[i].Save(writer);

            //IntelligentCreature
            writer.Write(IntelligentCreatures.Count);
            for (int i = 0; i < IntelligentCreatures.Count; i++)
                IntelligentCreatures[i].Save(writer);
            writer.Write(PearlCount);

            writer.Write(CompletedQuests.Count);
            for (int i = 0; i < CompletedQuests.Count; i++)
                writer.Write(CompletedQuests[i]);


            writer.Write(CurrentRefine != null);
            if (CurrentRefine != null)
                CurrentRefine.Save(writer);

            if ((CollectTime - SMain.Envir.Time) < 0)
                CollectTime = 0;
            else
                CollectTime = CollectTime - SMain.Envir.Time;

            writer.Write(CollectTime);

            writer.Write(Friends.Count);
            for (int i = 0; i < Friends.Count; i++)
                Friends[i].Save(writer);

            writer.Write(Married);
            writer.Write(MarriedDate.GetValueOrDefault().ToBinary());
            writer.Write(Mentor);
            writer.Write(MentorDate.GetValueOrDefault().ToBinary());
            writer.Write(isMentor);
            writer.Write(MentorExp);

            writer.Write(GSpurchases.Count);

            foreach (var item in GSpurchases)
            {
                writer.Write(item.Key);
                writer.Write(item.Value);
            }
        }

        public ListViewItem CreateListView()
        {
            if (ListItem != null)
                ListItem.Remove();

            ListItem = new ListViewItem(Index.ToString()) { Tag = this };

            ListItem.SubItems.Add(Name);
            ListItem.SubItems.Add(Level.ToString());
            ListItem.SubItems.Add(Class.ToString());
            ListItem.SubItems.Add(Gender.ToString());

            return ListItem;
        }

        public SelectInfo ToSelectInfo()
        {
            return new SelectInfo
                {
                    Index = Index,
                    Name = Name,
                    Level = Level,
                    Class = Class,
                    Gender = Gender,
                    LastAccess = LastDate.GetValueOrDefault()
            };
        }

        public bool CheckHasIntelligentCreature(IntelligentCreatureType petType)
        {
            for (int i = 0; i < IntelligentCreatures.Count; i++)
                if (IntelligentCreatures[i].PetType == petType) return true;
            return false;
        }
        public int ResizeInventory()
        {
            if (Inventory.Length >= 86) return Inventory.Length;

            if (Inventory.Length == 46)
                Array.Resize(ref Inventory, Inventory.Length + 8);
            else
                Array.Resize(ref Inventory, Inventory.Length + 4);

            return Inventory.Length;
        }
    }

    public class PetInfo
    {
        public int MonsterIndex;
        public uint HP, Experience;
        public byte Level, MaxPetLevel;

        public long Time;

        public PetInfo(MonsterObject ob)
        {
            MonsterIndex = ob.Info.Index;
            HP = ob.HP;
            Experience = ob.PetExperience;
            Level = ob.PetLevel;
            MaxPetLevel = ob.MaxPetLevel;
        }

        public PetInfo(BinaryReader reader)
        {
            MonsterIndex = reader.ReadInt32();
            if (MonsterIndex == 271) MonsterIndex = 275;
            HP = reader.ReadUInt32();
            Experience = reader.ReadUInt32();
            Level = reader.ReadByte();
            MaxPetLevel = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterIndex);
            writer.Write(HP);
            writer.Write(Experience);
            writer.Write(Level);
            writer.Write(MaxPetLevel);
        }
    }

    public class MountInfo
    {
        public PlayerObject Player;
        public short MountType = -1;

        public bool CanRide
        {
            get { return HasMount && Slots[(int)MountSlot.Saddle] != null; }
        }
        public bool CanMapRide
        {
            get { return HasMount && !Player.CurrentMap.Info.NoMount; }
        }
        public bool CanDungeonRide
        {
            get { return HasMount && CanMapRide && (!Player.CurrentMap.Info.NeedBridle || Slots[(int)MountSlot.Reins] != null); }
        }
        public bool CanAttack
        {
            get { return HasMount && Slots[(int)MountSlot.Bells] != null || !RidingMount; }
        }
        public bool SlowLoyalty
        {
            get { return HasMount && Slots[(int)MountSlot.Ribbon] != null; }
        }

        public bool HasMount
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount] != null; }
        }

        private bool RidingMount
        {
            get { return Player.RidingMount; }
            set { Player.RidingMount = value; }
        }

        public UserItem[] Slots
        {
            get { return Player.Info.Equipment[(int)EquipmentSlot.Mount].Slots; }
        }


        public MountInfo(PlayerObject ob)
        {
            Player = ob;
        }
    }

    public class FriendInfo
    {
        public long id { get; set; }

        public int Index { get; set; }

        private CharacterInfo _Info;
        [NotMapped]
        public CharacterInfo Info
        {
            get 
            {
                if (_Info == null) 
                    _Info = SMain.Envir.GetCharacterInfo(Index);

                return _Info;
            }
        }

        public bool Blocked { get; set; }
        public string Memo { get; set; }
        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        public FriendInfo() { }

        public FriendInfo(CharacterInfo info, bool blocked) 
        {
            Index = info.Index;
            Blocked = blocked;
            Memo = "";
        }

        public FriendInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Blocked = reader.ReadBoolean();
            Memo = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Blocked);
            writer.Write(Memo);
        }

        public ClientFriend CreateClientFriend()
        {
            return new ClientFriend()
            {
                Index = Index,
                Name = Info.Name,
                Blocked = Blocked,
                Memo = Memo,
                Online = Info.Player != null && Info.Player.Node != null
            };
        }
    }

    public class IntelligentCreatureInfo
    {
        public static List<IntelligentCreatureInfo> Creatures = new List<IntelligentCreatureInfo>();

        public static IntelligentCreatureInfo BabyPig,
                                                Chick,
                                                Kitten,
                                                BabySkeleton,
                                                Baekdon,
                                                Wimaen,
                                                BlackKitten,
                                                BabyDragon,
                                                OlympicFlame,
                                                BabySnowMan,
                                                Frog,
                                                Monkey;

        public IntelligentCreatureType PetType;

        public int Icon;
        public int MinimalFullness = 1000;

        public bool MousePickupEnabled = false;
        public int MousePickupRange = 0;
        public bool AutoPickupEnabled = false;
        public int AutoPickupRange = 0;
        public bool SemiAutoPickupEnabled = false;
        public int SemiAutoPickupRange = 0;

        public bool CanProduceBlackStone = false;

        public string Info = "";
        public string Info1 = "Unable to produce BlackStones.";
        public string Info2 = "Can produce Pearls, used to buy Creature items.";

        static IntelligentCreatureInfo()
        {
            BabyPig = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabyPig, Icon = 500, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 3, MinimalFullness = 4000, Info = "Can pickup items (3x3 semi-auto)." };
            Chick = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Chick, Icon = 501, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            Kitten = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Kitten, Icon = 502, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 3, MinimalFullness = 6000, Info = "Can pickup items (5x5 semi-auto)." };
            BabySkeleton = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabySkeleton, Icon = 503, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            Baekdon = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Baekdon, Icon = 504, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 7, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 7, CanProduceBlackStone = true, Info = "Can pickup items (7x7 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            Wimaen = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Wimaen, Icon = 505, MousePickupEnabled = true, MousePickupRange = 7, AutoPickupEnabled = true, AutoPickupRange = 5, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 5, MinimalFullness = 5000, Info = "Can pickup items (5x5 auto/semi-auto, 7x7 mouse)." };
            BlackKitten = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BlackKitten, Icon = 506, MousePickupEnabled = true, MousePickupRange = 7, AutoPickupEnabled = true, AutoPickupRange = 5, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 5, MinimalFullness = 5000, Info = "Can pickup items (5x5 auto/semi-auto, 7x7 mouse)." };
            BabyDragon = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabyDragon, Icon = 507, MousePickupEnabled = true, MousePickupRange = 7, AutoPickupEnabled = true, AutoPickupRange = 5, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 5, MinimalFullness = 7000, Info = "Can pickup items (5x5 auto/semi-auto, 7x7 mouse)." };
            OlympicFlame = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.OlympicFlame, Icon = 508, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 11, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 11, CanProduceBlackStone = true, Info = "Can pickup items (11x11 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            BabySnowMan = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabySnowMan, Icon = 509, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 11, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 11, CanProduceBlackStone = true, Info = "Can pickup items (11x11 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            Frog = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.Frog, Icon = 510, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 11, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 11, CanProduceBlackStone = true, Info = "Can pickup items (11x11 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
            Monkey = new IntelligentCreatureInfo { PetType = IntelligentCreatureType.BabyMonkey, Icon = 511, MousePickupEnabled = true, MousePickupRange = 11, AutoPickupEnabled = true, AutoPickupRange = 11, SemiAutoPickupEnabled = true, SemiAutoPickupRange = 11, CanProduceBlackStone = true, Info = "Can pickup items (11x11 auto/semi-auto, 11x11 mouse).", Info1 = "Can produce BlackStones." };
        }

        public IntelligentCreatureInfo()
        {
            Creatures.Add(this);
        }

        public static IntelligentCreatureInfo GetCreatureInfo(IntelligentCreatureType petType)
        {
            for (int i = 0; i < Creatures.Count; i++)
            {
                IntelligentCreatureInfo info = Creatures[i];
                if (info.PetType != petType) continue;
                return info;
            }
            return null;
        }
    }
    public class UserIntelligentCreature
    {
        [Key]
        public int Index { get; set; }

        public IntelligentCreatureType PetType { get; set; }
        [NotMapped]
        public IntelligentCreatureInfo Info;
        [NotMapped]
        public IntelligentCreatureItemFilter Filter;

        public IntelligentCreaturePickupMode petMode { get; set; } = IntelligentCreaturePickupMode.SemiAutomatic;
        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        
        public CharacterInfo CharacterInfo { get; set; }

        public string CustomName { get; set; }
        public int Fullness { get; set; }
        public int SlotIndex { get; set; }
        public long ExpireTime { get; set; } = -9999;//
        public long BlackstoneTime { get; set; } = 0;
        public long MaintainFoodTime { get; set; } = 0;

        public UserIntelligentCreature() { }

        public UserIntelligentCreature(IntelligentCreatureType creatureType, int slot, byte effect = 0)
        {
            PetType = creatureType;
            Info = IntelligentCreatureInfo.GetCreatureInfo(PetType);
            CustomName = Settings.IntelligentCreatureNameList[(byte)PetType];
            Fullness = 7500;//starts at 75% food
            SlotIndex = slot;

            if (effect > 0) ExpireTime = effect * 86400;//effect holds the amount in days
            else ExpireTime = -9999;//permanent

            BlackstoneTime = 0;
            MaintainFoodTime = 0;

            Filter = new IntelligentCreatureItemFilter();
        }

        public UserIntelligentCreature(BinaryReader reader)
        {
            PetType = (IntelligentCreatureType)reader.ReadByte();
            Info = IntelligentCreatureInfo.GetCreatureInfo(PetType);

            CustomName = reader.ReadString();
            Fullness = reader.ReadInt32();
            SlotIndex = reader.ReadInt32();
            ExpireTime = reader.ReadInt64();
            BlackstoneTime = reader.ReadInt64();

            petMode = (IntelligentCreaturePickupMode)reader.ReadByte();

            Filter = new IntelligentCreatureItemFilter(reader);
            if (Envir.LoadVersion > 48)
            {
                Filter.PickupGrade = (ItemGrade)reader.ReadByte();
                
                MaintainFoodTime = reader.ReadInt64();//maintain food buff
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((byte)PetType);

            writer.Write(CustomName);
            writer.Write(Fullness);
            writer.Write(SlotIndex);
            writer.Write(ExpireTime);
            writer.Write(BlackstoneTime);

            writer.Write((byte)petMode);

            Filter.Save(writer);
            writer.Write((byte)Filter.PickupGrade);//since Envir.Version 49

            writer.Write(MaintainFoodTime);//maintain food buff

        }

        public Packet GetInfo()
        {
            return new ServerPackets.NewIntelligentCreature
            {
                Creature = CreateClientIntelligentCreature()
            };
        }

        public ClientIntelligentCreature CreateClientIntelligentCreature()
        {
            return new ClientIntelligentCreature
            {
                PetType = PetType,
                Icon = Info.Icon,
                CustomName = CustomName,
                Fullness = Fullness,
                SlotIndex = SlotIndex,
                ExpireTime = ExpireTime,
                BlackstoneTime = BlackstoneTime,
                MaintainFoodTime = MaintainFoodTime,

                petMode = petMode,

                CreatureRules = new IntelligentCreatureRules
                {
                    MinimalFullness = Info.MinimalFullness,
                    MousePickupEnabled = Info.MousePickupEnabled,
                    MousePickupRange = Info.MousePickupRange,
                    AutoPickupEnabled = Info.AutoPickupEnabled,
                    AutoPickupRange = Info.AutoPickupRange,
                    SemiAutoPickupEnabled = Info.SemiAutoPickupEnabled,
                    SemiAutoPickupRange = Info.SemiAutoPickupRange,
                    CanProduceBlackStone = Info.CanProduceBlackStone,
                    Info = Info.Info,
                    Info1 = Info.Info1,
                    Info2 = Info.Info2
                },

                Filter = Filter
            };
        }
    }

    public class InventoryItem
    {
        public long id { get; set; }

        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        [ForeignKey("UserItem")]
        public long? ItemUniqueID { get; set; }
        public UserItem UserItem { get; set; }
    }

    public class EquipmentItem
    {
        public long id { get; set; }
        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        [ForeignKey("UserItem")]
        public long? ItemUniqueID { get; set; }
        public UserItem UserItem { get; set; }
    }

    public class QuestInventoryItem
    {
        public long id { get; set; }

        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        [ForeignKey("UserItem")]
        public long? ItemUniqueID { get; set; }
        public UserItem UserItem { get; set; }
    }

    public class CurrentRefineItem
    {
        public long id { get; set; }

        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        [ForeignKey("UserItem")]
        public long? ItemUniqueID { get; set; }
        public UserItem UserItem { get; set; }
    }

    public class UserBuff
    {
        [Key]
        public int Index { get; set; }

        public BuffType Type { get; set; }
        [NotMapped]
        public MapObject Caster { get; set; }
        public bool Visible { get; set; }
        [NotMapped]
        public uint ObjectID { get; set; }

        public long DBObjectID
        {
            get { return ObjectID; }
            set { ObjectID = (uint) value; }
        }
        public long ExpireTime { get; set; }
        public int[] Values;

        public string DbValues
        {
            get { return string.Join(",", Values); }
            set { Values = value.Split(',').Select(int.Parse).ToArray(); }
        }
        public bool Infinite { get; set; }

        public bool RealTime { get; set; }
        public DateTime? RealTimeExpire { get; set; } = SqlDateTime.MinValue.Value;

        public bool Paused { get; set; }
        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }
    }

    public class GameShopPurchase
    {
        public long id { get; set; }

        public int GameShopItemIndex { get; set; }
        public int GameShopItemQty { get; set; }

        [ForeignKey("CharacterInfo")]
        public int? CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }
    }
}
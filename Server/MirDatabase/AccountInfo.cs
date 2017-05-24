using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Server.MirNetwork;
using Server.MirEnvir;
using C = ClientPackets;


namespace Server.MirDatabase
{
    public class AccountInfo
    {
        public int Index;

        public string AccountID = string.Empty;
        public string Password = string.Empty;

        public string UserName = string.Empty;
        public DateTime BirthDate;
        public string SecretQuestion = string.Empty;
        public string SecretAnswer = string.Empty;
        public string EMailAddress = string.Empty;

        public string CreationIP = string.Empty;
        public DateTime CreationDate;

        public bool Banned;
        public string BanReason = string.Empty;
        public DateTime ExpiryDate;
        public int WrongPasswordCount;

        public string LastIP = string.Empty;
        public DateTime LastDate;

        public List<CharacterInfo> Characters = new List<CharacterInfo>();

        public UserItem[] Storage = new UserItem[80];
        public bool HasExpandedStorage;
        public DateTime ExpandedStorageExpiryDate;
        public uint Gold;
        public uint Credit;

        public ListViewItem ListItem;
        public MirConnection Connection;
        
        public LinkedList<AuctionInfo> Auctions = new LinkedList<AuctionInfo>();
        public bool AdminAccount;

        public AccountInfo()
        {

        }
        public AccountInfo(C.NewAccount p)
        {
            AccountID = p.AccountID;
            Password = p.Password;
            UserName = p.UserName;
            SecretQuestion = p.SecretQuestion;
            SecretAnswer = p.SecretAnswer;
            EMailAddress = p.EMailAddress;

            BirthDate = p.BirthDate;
            CreationDate = SMain.Envir.Now;
        }
        public AccountInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();

            AccountID = reader.ReadString();
            Password = reader.ReadString();

            UserName = reader.ReadString();
            BirthDate = DateTime.FromBinary(reader.ReadInt64());
            SecretQuestion = reader.ReadString();
            SecretAnswer = reader.ReadString();
            EMailAddress = reader.ReadString();

            CreationIP = reader.ReadString();
            CreationDate = DateTime.FromBinary(reader.ReadInt64());

            Banned = reader.ReadBoolean();
            BanReason = reader.ReadString();
            ExpiryDate = DateTime.FromBinary(reader.ReadInt64());

            LastIP = reader.ReadString();
            LastDate = DateTime.FromBinary(reader.ReadInt64());

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                Characters.Add(new CharacterInfo(reader) { AccountInfo = this });
            }

            if (Envir.LoadVersion > 75)
            {
                HasExpandedStorage = reader.ReadBoolean();
                ExpandedStorageExpiryDate = DateTime.FromBinary(reader.ReadInt64());
            }
            
            Gold = reader.ReadUInt32();
            if (Envir.LoadVersion >= 63) Credit = reader.ReadUInt32();

            count = reader.ReadInt32();

            Array.Resize(ref Storage, count);

            for (int i = 0; i < count; i++)
            {
                if (!reader.ReadBoolean()) continue;
                UserItem item = new UserItem(reader, Envir.LoadVersion, Envir.LoadCustomVersion);
                if (SMain.Envir.BindItem(item) && i < Storage.Length)
                    Storage[i] = item;
            }

            if (Envir.LoadVersion >= 10) AdminAccount = reader.ReadBoolean();
            if (!AdminAccount)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i] == null) continue;
                    if (Characters[i].Deleted) continue;
                    if ((DateTime.Now - Characters[i].LastDate).TotalDays > 13) continue;
                    if ((Characters[i].Level >= SMain.Envir.RankBottomLevel[0]) || (Characters[i].Level >= SMain.Envir.RankBottomLevel[(byte)Characters[i].Class + 1]))
                    {
                        SMain.Envir.CheckRankUpdate(Characters[i]);
                    }
                }
            }
        }


        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(AccountID);
            writer.Write(Password);

            writer.Write(UserName);
            writer.Write(BirthDate.ToBinary());
            writer.Write(SecretQuestion);
            writer.Write(SecretAnswer);
            writer.Write(EMailAddress);

            writer.Write(CreationIP);
            writer.Write(CreationDate.ToBinary());

            writer.Write(Banned);
            writer.Write(BanReason);
            writer.Write(ExpiryDate.ToBinary());

            writer.Write(LastIP);
            writer.Write(LastDate.ToBinary());

            writer.Write(Characters.Count);
            for (int i = 0; i < Characters.Count; i++)
                Characters[i].Save(writer);

            writer.Write(HasExpandedStorage);
            writer.Write(ExpandedStorageExpiryDate.ToBinary());
            writer.Write(Gold);
            writer.Write(Credit);
            writer.Write(Storage.Length);
            for (int i = 0; i < Storage.Length; i++)
            {
                writer.Write(Storage[i] != null);
                if (Storage[i] == null) continue;

                Storage[i].Save(writer);
            }
            writer.Write(AdminAccount);
        }

        public ListViewItem CreateListView()
        {
            if (ListItem != null)
                ListItem.Remove();


            ListItem = new ListViewItem(Index.ToString()) {Tag = this};

            ListItem.SubItems.Add(AccountID);
            ListItem.SubItems.Add(Password);
            ListItem.SubItems.Add(UserName);
            ListItem.SubItems.Add(AdminAccount.ToString());
            ListItem.SubItems.Add(Banned.ToString());
            ListItem.SubItems.Add(BanReason);
            ListItem.SubItems.Add(ExpiryDate.ToString());

            return ListItem;
        }

        public void Update()
        {
            if (ListItem == null) return;

            ListItem.SubItems[0].Text = Index.ToString();
            ListItem.SubItems[1].Text = AccountID;
            ListItem.SubItems[2].Text = Password;
            ListItem.SubItems[3].Text = UserName;
            ListItem.SubItems[4].Text = AdminAccount.ToString();
            ListItem.SubItems[5].Text = Banned.ToString();
            ListItem.SubItems[6].Text = BanReason;
            ListItem.SubItems[7].Text = ExpiryDate.ToString();
        }

        public List<SelectInfo> GetSelectInfo()
        {
            List<SelectInfo> list = new List<SelectInfo>();

            for (int i = 0; i < Characters.Count; i++)
            {
                if (Characters[i].Deleted) continue;
                list.Add(Characters[i].ToSelectInfo());
                if (list.Count >= Globals.MaxCharacterCount) break;
            }

            return list;
        }

        public int ExpandStorage()
        {
            if (Storage.Length == 80)
                Array.Resize(ref Storage, Storage.Length + 80);

            return Storage.Length;
        }
    }
}
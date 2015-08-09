using Server.MirDatabase;
using Server.MirObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.MirEnvir
{
    public class MailInfo
    {
        public ulong MailID;

        public string Sender;

        public int RecipientIndex;
        public CharacterInfo RecipientInfo;

        public string Message = string.Empty;
        public uint Gold = 0;
        public List<UserItem> Items = new List<UserItem>();

        public DateTime DateSent, DateOpened;

        public bool Sent
        {
            get { return DateSent > DateTime.MinValue; }
        }

        public bool Opened
        {
            get { return DateOpened > DateTime.MinValue; }
        }

        public bool Locked;

        public bool Collected;

        public bool Parcel //parcel if item contains gold or items.
        {
            get { return Gold > 0 || Items.Count > 0; }
        }

        public bool CanReply;

        public MailInfo(int recipientIndex, bool canReply = false)
        {
            MailID = ++SMain.Envir.NextMailID;
            RecipientIndex = recipientIndex;

            CanReply = canReply;
        }

        public MailInfo(BinaryReader reader, int version, int customversion)
        {
            MailID = reader.ReadUInt64();
            Sender = reader.ReadString();
            RecipientIndex = reader.ReadInt32();
            Message = reader.ReadString();
            Gold = reader.ReadUInt32();

            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                UserItem item = new UserItem(reader, version, customversion);
                if (SMain.Envir.BindItem(item))
                    Items.Add(item);
            }

            DateSent = DateTime.FromBinary(reader.ReadInt64());
            DateOpened = DateTime.FromBinary(reader.ReadInt64());

            Locked = reader.ReadBoolean();
            Collected = reader.ReadBoolean();
            CanReply = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(MailID);
            writer.Write(Sender);
            writer.Write(RecipientIndex);
            writer.Write(Message);
            writer.Write(Gold);

            writer.Write(Items.Count);
            for (int i = 0; i < Items.Count; i++)
                Items[i].Save(writer);

            writer.Write(DateSent.ToBinary());
            writer.Write(DateOpened.ToBinary());

            writer.Write(Locked);
            writer.Write(Collected);
            writer.Write(CanReply);
        }

        public void Send()
        {
            if (Sent) return;

            Collected = true;

            if (Parcel)
            {
                if(Items.Count > 0 && Gold > 0)
                {
                    if(!Settings.MailAutoSendGold || !Settings.MailAutoSendItems)
                    {
                        Collected = false;
                    }
                }
                if(Items.Count > 0)
                {
                    if (!Settings.MailAutoSendItems)
                    {
                        Collected = false;
                    }
                }
                else
                {
                    if (!Settings.MailAutoSendGold)
                    {
                        Collected = false;
                    }
                }
            }

            if (SMain.Envir.Mail.Contains(this)) return;

            SMain.Envir.Mail.Add(this); //add to postbox

            DateSent = DateTime.Now;
        }

        public bool Receive()
        {
            if (!Sent) return false; //mail not sent yet

            if (RecipientInfo == null)
            {
                RecipientInfo = SMain.Envir.GetCharacterInfo(RecipientIndex);

                if (RecipientInfo == null) return false;
            }

            RecipientInfo.Mail.Add(this); //add to players inbox
            
            if(RecipientInfo.Player != null)
            {
                RecipientInfo.Player.NewMail = true; //notify player of new mail  --check in player process
            }

            SMain.Envir.Mail.Remove(this); //remove from postbox

            return true;
        }

        public ClientMail CreateClientMail()
        {
            return new ClientMail
            {
                MailID = MailID,
                SenderName = Sender,
                Message = Message,
                Locked = Locked,
                CanReply = CanReply,
                Gold = Gold,
                Items = Items,
                Opened = Opened,
                Collected = Collected,
                DateSent = DateSent
            };
        }
    }

    // player bool NewMail (process in envir loop) - send all mail on login

    // Send mail from player (auto from player)
    // Send mail from Envir (mir administrator)
}

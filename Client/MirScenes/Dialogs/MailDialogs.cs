using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Client.MirSounds;
using Client.MirNetwork;
using C = ClientPackets;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public class MailListDialog : MirImageControl
    {
        MirButton HelpButton, CloseButton;
        //Send / Reply (Can only reply if index exists, button will disapear if not) / Read / Delete / Block List / Bug Report (new system??)

        MirLabel PageLabel;
        MirButton PreviousButton, NextButton;
        MirButton SendButton, ReplyButton, ReadButton, DeleteButton, BlockListButton, BugReportButton;

        public MailItemRow[] Rows = new MailItemRow[10];

        public ClientMail SelectedMail;
        public int SelectedIndex;

        private int StartIndex = 0;
        private int CurrentPage = 1, PageCount = 1;

        public MailListDialog()
        {
            Index = 670;
            Library = Libraries.Title;
            Size = new Size(312, 444);
            Movable = true;
            Sort = true;
            Location = new Point((Settings.ScreenWidth - Size.Width) - 150, 5);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            HelpButton = new MirButton
            {
                Index = 257,
                HoverIndex = 258,
                PressedIndex = 259,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(Size.Width - 50, 3),
                Sound = SoundList.ButtonA,
            };
            HelpButton.Click += (o, e) => GameScene.Scene.HelpDialog.DisplayPage("");

            PreviousButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(102, Size.Height - 55),
                Sound = SoundList.ButtonA,
            };
            PreviousButton.Click += (o, e) =>
            {
                if (CurrentPage <= 1) return;

                SelectedMail = null;

                CurrentPage--;

                StartIndex -= 10;

                UpdateInterface();
            };

            PageLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Location = new Point(120, Size.Height - 55),
                Size = new Size(67, 15),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(192, Size.Height - 55),
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                if (CurrentPage >= PageCount) return;

                SelectedMail = null;

                CurrentPage++;
                StartIndex += 10;

                UpdateInterface();
            };

            #region Action Buttons
            SendButton = new MirButton
            {
                Index = 563,
                HoverIndex = 564,
                PressedIndex = 565,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(75, 414),
                Sound = SoundList.ButtonA,
                Hint = "Send"
            };
            SendButton.Click += (o, e) =>
                {
                    MirInputBox inputBox = new MirInputBox("Please enter the name of the person you would like to mail.");

                    inputBox.OKButton.Click += (o1, e1) =>
                    {
                        //open letter dialog, pass in name
                        GameScene.Scene.MailComposeLetterDialog.ComposeMail(inputBox.InputTextBox.Text);
                        
                        inputBox.Dispose();
                    };

                    inputBox.Show();
                };

            ReplyButton = new MirButton
            {
                Index = 569,
                HoverIndex = 570,
                PressedIndex = 571,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(102, 414),
                Sound = SoundList.ButtonA,
                Hint = "Reply"
            };
            ReplyButton.Click += (o, e) =>
            {
                if (SelectedMail == null) return;

                GameScene.Scene.MailComposeLetterDialog.ComposeMail(SelectedMail.SenderName);
            };

            ReadButton = new MirButton
            {
                Index = 572,
                HoverIndex = 573,
                PressedIndex = 574,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(129, 414),
                Sound = SoundList.ButtonA,
                Hint = "Read"
            };
            ReadButton.Click += (o, e) =>
            {
                if (SelectedMail == null) return;

                if(SelectedMail.Gold > 0 || SelectedMail.Items.Count > 0)
                {
                    GameScene.Scene.MailReadParcelDialog.ReadMail(SelectedMail);
                }
                else
                {
                    GameScene.Scene.MailReadLetterDialog.ReadMail(SelectedMail);
                }
            };

            DeleteButton = new MirButton
            {
                Index = 557,
                HoverIndex = 558,
                PressedIndex = 559,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(156, 414),
                Sound = SoundList.ButtonA,
                Hint = "Delete"
            };
            DeleteButton.Click += (o, e) =>
            {
                if (SelectedMail == null || SelectedMail.Locked) return;

                if (SelectedMail.Items.Count > 0 || SelectedMail.Gold > 0)
                {
                    MirMessageBox messageBox = new MirMessageBox("This parcel contains items or gold. Are you sure you want to delete it?", MirMessageBoxButtons.YesNo);

                    messageBox.YesButton.Click += (o1, e1) =>
                    {
                        Network.Enqueue(new C.DeleteMail { MailID = SelectedMail.MailID });
                        SelectedMail = null;
                    };

                    messageBox.Show();
                }
                else
                {
                    Network.Enqueue(new C.DeleteMail { MailID = SelectedMail.MailID });
                    SelectedMail = null;
                }
            };

            BlockListButton = new MirButton
            {
                Index = 520,
                HoverIndex = 521,
                PressedIndex = 522,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(183, 414),
                Sound = SoundList.ButtonA,
                Hint = "Block List"
            };

            BugReportButton = new MirButton
            {
                Index = 523,
                HoverIndex = 524,
                PressedIndex = 525,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(210, 414),
                Sound = SoundList.ButtonA,
                Hint = "Report Bug"
            };
            #endregion

            
        }

        public void Reset()
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i] != null) Rows[i].Dispose();

                Rows[i] = null;
            }
        }

        public void UpdateInterface()
        {
            Reset();

            PageCount = (int)Math.Ceiling((double)GameScene.User.Mail.Count / 10);
            if (PageCount < 1) PageCount = 1;

            PageLabel.Text = string.Format("{0} / {1}", CurrentPage, PageCount);

            for (int i = 0; i < Rows.Length; i++)
            {
                if (i + StartIndex >= GameScene.User.Mail.Count) break;

                if (Rows[i] != null)
                    Rows[i].Dispose();

                Rows[i] = new MailItemRow
                {
                    Mail = GameScene.User.Mail[i + StartIndex],
                    Location = new Point(10, 55 + i * 33),
                    Parent = this
                };

                Rows[i].Click += (o, e) =>
                {
                    MailItemRow row = (MailItemRow)o;

                    if (row.Mail != SelectedMail)
                    {
                        SelectedMail = row.Mail;
                        SelectedIndex = FindSelectedIndex();
                        UpdateRows();
                    }
                    else
                    {
                        if (SelectedMail.Gold > 0 || SelectedMail.Items.Count > 0)
                        {
                            GameScene.Scene.MailReadParcelDialog.ReadMail(SelectedMail);
                        }
                        else
                        {
                            GameScene.Scene.MailReadLetterDialog.ReadMail(SelectedMail);
                        }
                    }
                };

                if (SelectedMail != null)
                {
                    if(SelectedMail.MailID == Rows[i].Mail.MailID)
                    {
                        SelectedMail = Rows[i].Mail;
                    }
                }
            }

            UpdateRows();
        }

        public int FindSelectedIndex()
        {
            int selectedIndex = 0;
            if (SelectedMail != null)
            {
                for (int i = 0; i < Rows.Length; i++)
                {
                    if (Rows[i] == null || SelectedMail != Rows[i].Mail) continue;

                    selectedIndex = i;
                }
            }

            return selectedIndex;
        }

        public void UpdateRows()
        {
            for (int i = 0; i < Rows.Length; i++)
            {
                if (Rows[i] == null) continue;

                Rows[i].Selected = false;

                if (Rows[i].Mail == SelectedMail)
                {
                    Rows[i].Selected = true;
                }

                Rows[i].UpdateInterface();
            }

            if(SelectedMail != null)
            {
                ReplyButton.Visible = SelectedMail.CanReply;
            }
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;

            UpdateInterface();
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;

            SelectedMail = null;
            SelectedIndex = -1;
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }

    public class MailItemRow : MirControl
    {
        public ClientMail Mail = null;

        public MirLabel SenderLabel, MessageLabel;
        public MirImageControl IconImage, UnreadImage, ParcelImage, LockedImage, SelectedImage;

        public bool Selected = false;

        Size IconArea = new Size(34, 32);

        public MailItemRow()
        {
            Sound = SoundList.ButtonA;
            Size = new Size(290, 33);

            BeforeDraw += QuestRow_BeforeDraw;

            IconImage = new MirImageControl
            {
                Index = 0,
                Library = Libraries.Prguse,
                Location = new Point(0, 0),
                Parent = this
            };

            UnreadImage = new MirImageControl
            {
                Index = 550,
                Library = Libraries.Prguse,
                Location = new Point(5, 17),
                Parent = this,
                Visible = false
            };

            LockedImage = new MirImageControl
            {
                Index = 551,
                Library = Libraries.Prguse,
                Location = new Point(5, 17),
                Parent = this,
                Visible = false
            };

            ParcelImage = new MirImageControl
            {
                Index = 552,
                Library = Libraries.Prguse,
                Location = new Point(5, 17), //20
                Parent = this,
                Visible = false
            };


            SelectedImage = new MirImageControl
            {
                Index = 545,
                Library = Libraries.Prguse,
                Location = new Point(-5, -3),
                Parent = this,
                Visible = false,
                NotControl = true
            };

            SenderLabel = new MirLabel
            {
                Location = new Point(35, 0),
                Size = new Size(130, 31),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
            };

            MessageLabel = new MirLabel
            {
                Location = new Point(170, 0),
                Size = new Size(115, 31),
                DrawFormat = TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
            };

            UpdateInterface();
        }

        void QuestRow_BeforeDraw(object sender, EventArgs e)
        {
            UpdateInterface();
        }

        public void UpdateInterface()
        {
            if (Mail == null) return;

            IconImage.Visible = true;

            if (Mail.Items.Count > 0)
            {
                IconImage.Index = Mail.Items[0].Info.Image;
                IconImage.Library = Libraries.Items;
            }
            else if (Mail.Gold > 0)
            {
                IconImage.Index = 541;
                IconImage.Library = Libraries.Prguse;
            }
            else
            {
                IconImage.Index = 540;
                IconImage.Library = Libraries.Prguse;
            }

            IconImage.Location = new Point((IconArea.Width - IconImage.Size.Width) /2, (IconArea.Height - IconImage.Size.Height) / 2);


            if (!Mail.Opened)
            {
                UnreadImage.Visible = true;
            }

            if (Mail.Locked)
            {
                LockedImage.Visible = true;
            }

            if (!Mail.Collected)
            {
                ParcelImage.Visible = true;

                if (!Mail.Opened)
                {
                    //move unread to second position if not collected parcel
                    UnreadImage.Location = new Point(20, 17);
                }
            }
            else
            {
                if (Mail.Locked)
                {
                    //move unread to second position if locked
                    UnreadImage.Location = new Point(20, 17);
                }
            }

            SenderLabel.Text = Mail.SenderName;
            MessageLabel.Text = Mail.Locked ? "[*] " + Mail.Message.Replace("\r\n", " ") : Mail.Message.Replace("\r\n", " ");

            SelectedImage.Visible = Selected;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Mail = null;
            SenderLabel = null;
            MessageLabel = null;
            SelectedImage = null;
            IconImage = null;

            Selected = false;
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            GameScene.Scene.CreateMailLabel(Mail);          
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeMailLabel();
        }
    }

    public class MailComposeLetterDialog : MirImageControl
    {
        MirLabel RecipientNameLabel;
        MirTextBox MessageTextBox;
        MirButton SendButton, CancelButton, CloseButton;

        public MailComposeLetterDialog()
        {
            Index = 671;
            Library = Libraries.Title;
            Size = new Size(236, 300);
            Movable = true;
            Sort = true;
            Location = new Point(100, 100);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();


            RecipientNameLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 35),
                Size = new Size(150, 15),
                NotControl = true,
            };

            MessageTextBox = new MirTextBox
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(15, 92),
                Size = new Size(202, 165),
            };

            MessageTextBox.MultiLine();

            SendButton = new MirButton
            {
                Index = 607,
                HoverIndex = 608,
                PressedIndex = 609,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(30, 265)
            };
            SendButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.SendMail { Name = RecipientNameLabel.Text, Message = MessageTextBox.Text });
                Hide();
            };

            CancelButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(135, 265)
            };
            CancelButton.Click += (o, e) => Hide();
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void ComposeMail(string recipientName)
        {
            if (string.IsNullOrEmpty(recipientName)) return;

            RecipientNameLabel.Text = recipientName;
            MessageTextBox.Text = string.Empty;

            MessageTextBox.SetFocus();

            Visible = true;
        }
    }
    public class MailComposeParcelDialog : MirImageControl
    {
        public MirLabel RecipientNameLabel, ParcelCostLabel, GoldSendLabel;
        MirTextBox MessageTextBox;
        MirButton StampButton, SendButton, CancelButton, CloseButton;
        MirImageControl ItemCover;

        private const uint _cellCount = 5;

        public MirItemCell[] Cells = new MirItemCell[_cellCount];

        public static UserItem[] Items = new UserItem[_cellCount];
        public static ulong[] ItemsIdx = new ulong[_cellCount];

        public uint GiftGoldAmount = 0;
        public bool Stamped = false;

        public MailComposeParcelDialog()
        {
            Index = 674;
            Library = Libraries.Title;
            Size = new Size(236, 384);
            Movable = true;
            Sort = true;
            Location = new Point(GameScene.Scene.InventoryDialog.Size.Width + 10, 0);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) =>
            {
                ResetLockedCells();
                Hide();
            };

            RecipientNameLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 35),
                Size = new Size(150, 15),
                NotControl = true,
            };

            MessageTextBox = new MirTextBox
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(15, 98),
                Size = new Size(202, 165),
            };

            MessageTextBox.MultiLine();

            StampButton = new MirButton
            {
                Index = 203,
                HoverIndex = 203,
                PressedIndex = 203,
                Location = new Point(73, 56),
                Size = new Size(20,20),
                Library = Libraries.Prguse2,
                Parent = this,             
                Sound = SoundList.ButtonA,
            };
            StampButton.Click += (o, e) =>
            {
                StampParcel();
            };

            ItemCover = new MirImageControl
            {
                Index = 676,
                Location = new Point(63, 310),
                Size = new Size(144, 33),
                Library = Libraries.Title,
                Parent = this
            };

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new MirItemCell
                {
                    BorderColour = Color.Lime,
                    Size = new Size(35, 31),
                    GridType = MirGridType.Mail,
                    Library = Libraries.Items,
                    Parent = this,
                    Location = new Point(27 + (i * 36), 311),
                    ItemSlot = i
                };
            }

            ParcelCostLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(63, 269),
                Parent = this,
                Size = new Size(143, 15),
            };

            GoldSendLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(63, 290),
                Parent = this,
                Size = new Size(143, 15),
                Sound = SoundList.Gold,
            };
            GoldSendLabel.Click += (o, e) =>
            {
                if (GameScene.SelectedCell == null && GameScene.Gold > 0)
                {
                    MirAmountBox amountBox = new MirAmountBox("Send Amount:", 116, GameScene.Gold);

                    amountBox.OKButton.Click += (c, a) =>
                    {
                        if (amountBox.Amount > 0)
                        {
                            GiftGoldAmount += amountBox.Amount;
                            GameScene.Gold -= amountBox.Amount;
                        }

                        GoldSendLabel.Text = GiftGoldAmount.ToString("###,###,##0");

                        CalculatePostage();
                    };

                    amountBox.Show();
                    GameScene.PickedUpGold = false;
                }
            };

            SendButton = new MirButton
            {
                Index = 607,
                HoverIndex = 608,
                PressedIndex = 609,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(30, 350)
            };
            SendButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.SendMail { Name = RecipientNameLabel.Text, Message = MessageTextBox.Text, Gold = GiftGoldAmount, ItemsIdx = ItemsIdx, Stamped = Stamped });
            };

            CancelButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(135, 350)
            };
            CancelButton.Click += (o, e) =>
            {
                ResetLockedCells();
                Hide();
            };
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;

            Reset();
        }

        public void Reset()
        {
            GameScene.Gold += GiftGoldAmount;
            GiftGoldAmount = 0;
            Stamped = false;

            ResetLockedCells();
        }

        public void ResetLockedCells()
        {
            for (int i = 0; i < _cellCount; i++)
            {
                MirItemCell cell = Cells[i];

                if (cell.Item != null)
                {
                    Network.Enqueue(new C.MailLockedItem { UniqueID = cell.Item.UniqueID, Locked = false });
                    cell.Item = null;
                }

                ItemsIdx[i] = 0;
            }
        }

        private void StampParcel()
        {
            if(!Stamped)
            {
                for (int i = 0; i < GameScene.User.Inventory.Length; i++)
                {
                    UserItem item = GameScene.User.Inventory[i];
                    if (item == null || item.Info.Type != ItemType.Nothing || item.Info.Shape != 1) continue;

                    Stamped = true;
                    break;
                }
            }
            else
                Stamped = false;

            CalculatePostage();
            UpdateParcel();
            ResetLockedCells();
        }

        private void UpdateParcel()
        {
            if (Stamped)
            {
                StampButton.Index = 204;
                StampButton.HoverIndex = 204;
                StampButton.PressedIndex = 204;

                for (int i = 1; i < Cells.Length; i++)
                {
                    Cells[i].Enabled = true;
                }

                ItemCover.Visible = false;
            }
            else
            {
                StampButton.Index = 203;
                StampButton.HoverIndex = 203;
                StampButton.PressedIndex = 203;

                for (int i = 1; i < Cells.Length; i++)
                {
                    Cells[i].Enabled = false;
                }

                ItemCover.Visible = true;
            }
        }


        public void CalculatePostage()
        {
            Network.Enqueue(new C.MailCost { Gold = GiftGoldAmount, ItemsIdx = ItemsIdx, Stamped = Stamped });
        }

        public void ComposeMail(string recipientName)
        {
            if (string.IsNullOrEmpty(recipientName)) return;

            RecipientNameLabel.Text = recipientName;
            MessageTextBox.Text = string.Empty;
            MessageTextBox.SetFocus();

            UpdateParcel();

            //Disable last 4 item slots
            for (int i = 1; i < Cells.Length; i++)
            {
                Cells[i].Enabled = false;
            }

            ParcelCostLabel.Text = "0";
            GoldSendLabel.Text = "0";

            ResetLockedCells();

            Visible = true;
        }
    }

    public class MailReadLetterDialog : MirImageControl
    {
        MirLabel SenderNameLabel, DateSentLabel, MessageLabel;

        MirButton DeleteButton, LockButton, CancelButton, CloseButton;

        public ClientMail Mail;

        public MailReadLetterDialog()
        {
            Index = 672;
            Library = Libraries.Title;
            Size = new Size(236, 300);
            Movable = true;
            Sort = true;
            Location = new Point(100, 100);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            SenderNameLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 35),
                Size = new Size(150, 15),
                NotControl = true,
            };

            DateSentLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 56),
                Size = new Size(150, 15),
                NotControl = true,
            };

            MessageLabel = new MirLabel
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(15, 92),
                Size = new Size(202, 165),
            };

            DeleteButton = new MirButton
            {
                Index = 540,
                HoverIndex = 541,
                PressedIndex = 542,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(12, 265)
            };
            DeleteButton.Click += (o, e) =>
            {
                if (Mail.Locked) return;

                Network.Enqueue(new C.DeleteMail { MailID = Mail.MailID });

                Mail = null;

                Hide();
            };

            LockButton = new MirButton
            {
                Index = 686,
                HoverIndex = 687,
                PressedIndex = 688,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(81, 265)
            };
            LockButton.Click += (o, e) =>
            {
                Mail.Locked = !Mail.Locked;

                //GameScene.Scene.MailListDialog.SelectedMail = null;

                Network.Enqueue(new C.LockMail { MailID = Mail.MailID, Lock = Mail.Locked });
            };

            CancelButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(154, 265)
            };
            CancelButton.Click += (o, e) => Hide();
        }

        public void ReadMail(ClientMail mail)
        {
            if (mail == null) return;

            Mail = mail;

            if (!Mail.Opened)
            {
                Network.Enqueue(new C.ReadMail { MailID = Mail.MailID });
            }

            SenderNameLabel.Text = Mail.SenderName;
            DateSentLabel.Text = Mail.DateSent.ToString("dd/MM/yy H:mm:ss");
            MessageLabel.Text = Mail.Message.Replace("\\r\\n", "\r\n");

            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
    }
    public class MailReadParcelDialog : MirImageControl
    {
        MirLabel SenderNameLabel, DateSentLabel, MessageLabel, GoldSendLabel;

        MirButton CollectButton, CancelButton, CloseButton;

        public MirItemCell[] Cells = new MirItemCell[5];

        public ClientMail Mail;

        public MailReadParcelDialog()
        {
            Index = 675;
            Library = Libraries.Title;
            Size = new Size(236, 300);
            Movable = true;
            Sort = true;
            Location = new Point(100, 100);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 27, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            SenderNameLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 35),
                Size = new Size(150, 15),
                NotControl = true,
            };

            DateSentLabel = new MirLabel
            {
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Location = new Point(70, 56),
                Size = new Size(150, 15),
                NotControl = true,
            };

            MessageLabel = new MirLabel
            {
                ForeColour = Color.White,
                Parent = this,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(15, 98),
                Size = new Size(202, 165),
            };

            GoldSendLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(63, 290),
                Parent = this,
                Size = new Size(143, 15),
            };

            CollectButton = new MirButton
            {
                Index = 370,
                HoverIndex = 371,
                PressedIndex = 372,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(30, 350)
            };
            CollectButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.CollectParcel { MailID = Mail.MailID }); 
            };

            CancelButton = new MirButton
            {
                Index = 193,
                HoverIndex = 194,
                PressedIndex = 195,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(135, 350)
            };
            CancelButton.Click += (o, e) => Hide();
        }

        public void ReadMail(ClientMail mail)
        {
            if (mail == null) return;

            Mail = mail;

            if (!Mail.Opened)
            {
                Network.Enqueue(new C.ReadMail { MailID = Mail.MailID });
            }

            ResetCells();

            SenderNameLabel.Text = Mail.SenderName;
            DateSentLabel.Text = Mail.DateSent.ToString("dd/MM/yy H:mm:ss");
            MessageLabel.Text = Mail.Message.Replace("\\r\\n", "\r\n");
            GoldSendLabel.Text = Mail.Gold.ToString("###,###,##0");

            if (Mail.Items.Count > 0)
            {
                for (int i = 0; i < Cells.Length; i++)
                {
                    if (i >= Mail.Items.Count) break;

                    Cells[i] = new MirItemCell
                    {
                        BorderColour = Color.Lime,
                        Size = new Size(35, 31),
                        GridType = MirGridType.Mail,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(27 + (i * 36), 311),
                        ItemSlot = i,
                        Item = Mail.Items[i]
                    };
                }
            }

            if (!Mail.Collected)
            {
                CollectButton.Index = 683;
                CollectButton.HoverIndex = 684;
                CollectButton.PressedIndex = 685;
                CollectButton.Enabled = false;
            }
            else
            {
                CollectButton.Index = 680;
                CollectButton.HoverIndex = 681;
                CollectButton.PressedIndex = 682;
                CollectButton.Enabled = true;
            }

            Visible = true;
        }

        public void ResetCells()
        {
            foreach (var item in Cells)
            {
                if (item == null) continue;

                if (item.Item != null)
                {
                    item.Item = null;
                }
            }
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
    }
}

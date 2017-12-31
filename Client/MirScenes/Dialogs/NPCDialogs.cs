using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using Microsoft.DirectX.Direct3D;
using Font = System.Drawing.Font;
using S = ServerPackets;
using C = ClientPackets;
using Effect = Client.MirObjects.Effect;

using Client.MirScenes.Dialogs;
using System.Drawing.Imaging;

namespace Client.MirScenes.Dialogs
{
    public sealed class NPCDialog : MirImageControl
    {
        public static Regex R = new Regex(@"<(.*?/\@.*?)>");
        public static Regex C = new Regex(@"{(.*?/.*?)}");

        public MirButton CloseButton, UpButton, DownButton, PositionBar, QuestButton;
        public MirLabel[] TextLabel;
        public List<MirLabel> TextButtons;

        public MirLabel NameLabel;

        Font font = new Font(Settings.FontName, 9F);

        public List<string> CurrentLines = new List<string>();
        private int _index = 0;
        public int MaximumLines = 8;

        public NPCDialog()
        {
            Index = 995;
            Library = Libraries.Prguse;

            TextLabel = new MirLabel[30];
            TextButtons = new List<MirLabel>();

            MouseWheel += NPCDialog_MouseWheel;

            Sort = true;

            NameLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                ForeColour = Color.BurlyWood,
                Location = new Point(30, 6),
                AutoSize = true
            };

            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(417, 34),
                Sound = SoundList.ButtonA,
                Visible = false
            };
            UpButton.Click += (o, e) =>
            {
                if (_index <= 0) return;

                _index--;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            DownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(417, 175),
                Sound = SoundList.ButtonA,
                Visible = false
            };
            DownButton.Click += (o, e) =>
            {
                if (_index + MaximumLines >= CurrentLines.Count) return;

                _index++;

                NewText(CurrentLines, false);
                UpdatePositionBar();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(417, 47),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = false
            };
            PositionBar.OnMoving += PositionBar_OnMoving;

            QuestButton = new MirAnimatedButton()
            {
                Animated = true,
                AnimationCount = 10,
                Loop = true,
                AnimationDelay = 130,

                Index = 530,
                HoverIndex = 284,
                PressedIndex = 286,
                Library = Libraries.Title,
                Parent = this,
                Size = new Size(96, 25),
                Location = new Point((440 - 96) / 2, 224 - 30),
                Sound = SoundList.ButtonA,
                Visible = false
            };

            QuestButton.Click += (o, e) => GameScene.Scene.QuestListDialog.Toggle();

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(413, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            MirButton helpButton = new MirButton
            {
                Index = 257,
                HoverIndex = 258,
                PressedIndex = 259,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(390, 3),
                Sound = SoundList.ButtonA,
            };
            helpButton.Click += (o, e) => GameScene.Scene.HelpDialog.DisplayPage("Purchasing");
        }

        void NPCDialog_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (_index == 0 && count >= 0) return;
            if (_index == CurrentLines.Count - 1 && count <= 0) return;
            if (CurrentLines.Count <= MaximumLines) return;

            _index -= count;

            if (_index < 0) _index = 0;
            if (_index + MaximumLines > CurrentLines.Count - 1) _index = CurrentLines.Count - MaximumLines;

            NewText(CurrentLines, false);

            UpdatePositionBar();
        }

        void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 417;
            int y = PositionBar.Location.Y;

            if (y >= 155) y = 155;
            if (y <= 47) y = 47;

            int location = y - 47;
            int interval = 108 / (CurrentLines.Count - MaximumLines);

            double yPoint = location / interval;

            _index = Convert.ToInt16(Math.Floor(yPoint));

            NewText(CurrentLines, false);

            PositionBar.Location = new Point(x, y);
        }

        private void UpdatePositionBar()
        {
            if (CurrentLines.Count <= MaximumLines) return;

            int interval = 108 / (CurrentLines.Count - MaximumLines);

            int x = 417;
            int y = 48 + (_index * interval);

            if (y >= 155) y = 155;
            if (y <= 47) y = 47;

            PositionBar.Location = new Point(x, y);
        }


        public void NewText(List<string> lines, bool resetIndex = true)
        {
            if (resetIndex)
            {
                _index = 0;
                CurrentLines = lines;
                UpdatePositionBar();
            }

            if (lines.Count > MaximumLines)
            {
                Index = 385;
                UpButton.Visible = true;
                DownButton.Visible = true;
                PositionBar.Visible = true;
            }
            else
            {
                Index = 384;
                UpButton.Visible = false;
                DownButton.Visible = false;
                PositionBar.Visible = false;
            }

            for (int i = 0; i < TextButtons.Count; i++)
                TextButtons[i].Dispose();

            for (int i = 0; i < TextLabel.Length; i++)
            {
                if (TextLabel[i] != null) TextLabel[i].Text = "";
            }

            TextButtons.Clear();

            int lastLine = lines.Count > MaximumLines ? ((MaximumLines + _index) > lines.Count ? lines.Count : (MaximumLines + _index)) : lines.Count;

            for (int i = _index; i < lastLine; i++)
            {
                TextLabel[i] = new MirLabel
                {
                    Font = font,
                    DrawFormat = TextFormatFlags.WordBreak,
                    Visible = true,
                    Parent = this,
                    Size = new Size(420, 20),
                    Location = new Point(20, 34 + (i - _index) * 20),
                    NotControl = true
                };

                if (i >= lines.Count)
                {
                    TextLabel[i].Text = string.Empty;
                    continue;
                }

                string currentLine = lines[i];

                List<Match> matchList = R.Matches(currentLine).Cast<Match>().ToList();
                matchList.AddRange(C.Matches(currentLine).Cast<Match>());

                int oldLength = currentLine.Length;

                foreach (Match match in matchList.OrderBy(o => o.Index).ToList())
                {
                    int offSet = oldLength - currentLine.Length;

                    Capture capture = match.Groups[1].Captures[0];
                    string[] values = capture.Value.Split('/');
                    currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, values[0]);
                    string text = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                    Size size = TextRenderer.MeasureText(CMain.Graphics, text, TextLabel[i].Font, TextLabel[i].Size, TextFormatFlags.TextBoxControl);

                    if (R.Match(match.Value).Success)
                        NewButton(values[0], values[1], TextLabel[i].Location.Add(new Point(size.Width - 10, 0)));

                    if (C.Match(match.Value).Success)
                        NewColour(values[0], values[1], TextLabel[i].Location.Add(new Point(size.Width - 10, 0)));
                }

                TextLabel[i].Text = currentLine;
                TextLabel[i].MouseWheel += NPCDialog_MouseWheel;

            }
        }

        private void NewButton(string text, string key, Point p)
        {
            key = string.Format("[{0}]", key);

            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = Color.Yellow,
                Sound = SoundList.ButtonC,
                Font = font
            };
            //Fontstyle.Underline;

            temp.MouseEnter += (o, e) => temp.ForeColour = Color.Red;
            temp.MouseLeave += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseDown += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseUp += (o, e) => temp.ForeColour = Color.Red;

            temp.Click += (o, e) =>
            {
                if (key == "[@Exit]")
                {
                    Hide();
                    return;
                }

                if (CMain.Time <= GameScene.NPCTime) return;

                GameScene.NPCTime = CMain.Time + 5000;
                Network.Enqueue(new C.CallNPC { ObjectID = GameScene.NPCID, Key = key });
            };
            temp.MouseWheel += NPCDialog_MouseWheel;

            TextButtons.Add(temp);
        }

        private void NewColour(string text, string colour, Point p)
        {
            Color textColour = Color.FromName(colour);

            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = textColour,
                Font = font
            };
            temp.MouseWheel += NPCDialog_MouseWheel;

            TextButtons.Add(temp);
        }

        public void CheckQuestButtonDisplay()
        {
            NameLabel.Text = string.Empty;

            QuestButton.Visible = false;

            NPCObject npc = (NPCObject)MapControl.GetObject(GameScene.NPCID);
            if (npc != null)
            {
                string[] nameSplit = npc.Name.Split('_');
                NameLabel.Text = nameSplit[0];

                if (npc.GetAvailableQuests().Any())
                    QuestButton.Visible = true;
            }
        }

        public void Hide()
        {
            Visible = false;
            GameScene.Scene.NPCGoodsDialog.Hide();
            GameScene.Scene.NPCDropDialog.Hide();
            GameScene.Scene.NPCAwakeDialog.Hide();
            GameScene.Scene.RefineDialog.Hide();
            GameScene.Scene.StorageDialog.Hide();
            GameScene.Scene.TrustMerchantDialog.Hide();
            GameScene.Scene.InventoryDialog.Location = new Point(0, 0);
        }

        public void Show()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, 0);
            Visible = true;

            CheckQuestButtonDisplay();
        }
    }
    public sealed class NPCGoodsDialog : MirImageControl
    {
        public int StartIndex;
        public UserItem SelectedItem;

        public List<UserItem> Goods = new List<UserItem>();
        public MirGoodsCell[] Cells;
        public MirButton BuyButton, CloseButton;
        public MirImageControl BuyLabel;

        public MirButton UpButton, DownButton, PositionBar;

        public bool usePearls = false;//pearl currency

        public PanelType PType;

        public NPCGoodsDialog()
        {
            Index = 1000;
            Library = Libraries.Prguse;
            Location = new Point(0, 224);
            Cells = new MirGoodsCell[8];
            Sort = true;

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new MirGoodsCell
                {
                    Parent = this,
                    Location = new Point(10, 34 + i * 33),
                    Sound = SoundList.ButtonC,
                };
                Cells[i].Click += (o, e) =>
                {
                    SelectedItem = ((MirGoodsCell)o).Item;
                    Update();

                    if (PType == PanelType.Craft)
                    {
                        GameScene.Scene.CraftDialog.ResetCells();
                        GameScene.Scene.CraftDialog.RefreshCraftCells(SelectedItem);

                        if (GameScene.Scene.CraftDialog.Visible) ;
                        else
                            GameScene.Scene.CraftDialog.Show();
                    }
                };
                Cells[i].MouseWheel += NPCGoodsPanel_MouseWheel;
                Cells[i].DoubleClick += (o, e) => BuyItem();
            }

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(216, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            BuyButton = new MirButton
            {
                HoverIndex = 313,
                Index = 312,
                Location = new Point(77, 304),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 314,
                Sound = SoundList.ButtonA,
            };
            BuyButton.Click += (o, e) => BuyItem();

            BuyLabel = new MirImageControl
            {
                Index = 27,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(20, 9),
            };


            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                Library = Libraries.Prguse2,
                Location = new Point(218, 35),
                Parent = this,
                PressedIndex = 199,
                Sound = SoundList.ButtonA
            };
            UpButton.Click += (o, e) =>
            {
                if (StartIndex == 0) return;
                StartIndex--;
                Update();
            };

            DownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                Location = new Point(218, 284),
                Parent = this,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            DownButton.Click += (o, e) =>
            {
                if (Goods.Count <= 8) return;

                if (StartIndex == Goods.Count - 8) return;
                StartIndex++;
                Update();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(218, 49),
                Parent = this,
                PressedIndex = 206,
                Movable = true,
                Sound = SoundList.None
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
            PositionBar.MouseUp += (o, e) => Update();
        }

        private void BuyItem()
        {
            if (SelectedItem == null) return;

            if (SelectedItem.Info.StackSize > 1)
            {
                uint tempCount = SelectedItem.Count;
                uint maxQuantity = SelectedItem.Info.StackSize;

                SelectedItem.Count = maxQuantity;

                if (usePearls)//pearl currency
                {
                    if (SelectedItem.Price() > GameScene.User.PearlCount)
                    {
                        maxQuantity = GameScene.Gold / (SelectedItem.Price() / SelectedItem.Count);
                        if (maxQuantity == 0)
                        {
                            GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Pearls.", ChatType.System);
                            return;
                        }
                    }
                }

                else if (SelectedItem.Price() > GameScene.Gold)
                {
                    maxQuantity = GameScene.Gold / (SelectedItem.Price() / SelectedItem.Count);
                    if (maxQuantity == 0)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Gold.", ChatType.System);
                        return;
                    }
                }

                MapObject.User.GetMaxGain(SelectedItem);

                if (SelectedItem.Count > tempCount)
                {
                    SelectedItem.Count = tempCount;
                }

                if (SelectedItem.Count == 0) return;

                MirAmountBox amountBox = new MirAmountBox("Purchase Amount:", SelectedItem.Image, maxQuantity, 0, SelectedItem.Count);

                amountBox.OKButton.Click += (o, e) =>
                {
                    if (amountBox.Amount > 0)
                    {
                        Network.Enqueue(new C.BuyItem { ItemIndex = SelectedItem.UniqueID, Count = amountBox.Amount, Type = PanelType.Buy });
                    }
                };

                amountBox.Show();
            }
            else
            {
                if (SelectedItem.Info.Price > GameScene.Gold)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough gold.", ChatType.System);
                    return;
                }

                if (SelectedItem.Weight > (MapObject.User.MaxBagWeight - MapObject.User.CurrentBagWeight))
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough weight.", ChatType.System);
                    return;
                }

                for (int i = 0; i < MapObject.User.Inventory.Length; i++)
                {
                    if (MapObject.User.Inventory[i] == null) break;
                    if (i == MapObject.User.Inventory.Length - 1)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You cannot purchase any more items.", ChatType.System);
                        return;
                    }
                }


                Network.Enqueue(new C.BuyItem { ItemIndex = SelectedItem.UniqueID, Count = 1, Type = PanelType.Buy });
            }
        }

        private void NPCGoodsPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (StartIndex == 0 && count >= 0) return;
            if (StartIndex == Goods.Count - 1 && count <= 0) return;

            StartIndex -= count;
            Update();
        }
        private void Update()
        {
            if (StartIndex > Goods.Count - 8) StartIndex = Goods.Count - 8;
            if (StartIndex <= 0) StartIndex = 0;

            if (Goods.Count > 8)
            {
                PositionBar.Visible = true;
                int h = 233 - PositionBar.Size.Height;
                h = (int)((h / (float)(Goods.Count - 8)) * StartIndex);
                PositionBar.Location = new Point(218, 49 + h);
            }
            else
                PositionBar.Visible = false;


            for (int i = 0; i < 8; i++)
            {
                if (i + StartIndex >= Goods.Count)
                {
                    Cells[i].Visible = false;
                    continue;
                }
                Cells[i].Visible = true;

                Cells[i].Item = Goods[i + StartIndex];
                Cells[i].Border = SelectedItem != null && Cells[i].Item == SelectedItem;
                Cells[i].usePearls = usePearls;//pearl currency
            }




        }
        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            const int x = 218;
            int y = PositionBar.Location.Y;
            if (y >= 282 - PositionBar.Size.Height) y = 282 - PositionBar.Size.Height;
            if (y < 49) y = 49;

            int h = 233 - PositionBar.Size.Height;
            h = (int)Math.Round(((y - 49) / (h / (float)(Goods.Count - 8))));

            PositionBar.Location = new Point(x, y);

            if (h == StartIndex) return;
            StartIndex = h;
            Update();
        }

        public void UpdatePanelType(PanelType type)
        {
            PType = type;

            if (PType == PanelType.Buy)
            {
                BuyButton.Index = 312;
                BuyButton.HoverIndex = 313;
                BuyButton.PressedIndex = 314;

                BuyLabel.Index = 27;
                BuyButton.Visible = true;
            }
            else if (PType == PanelType.Craft)
            {
                BuyLabel.Index = 12;
                BuyButton.Visible = false;

                GameScene.Scene.CraftDialog.Show();
            }
        }

        public void NewGoods(List<UserItem> list)
        {
            Goods.Clear();
            StartIndex = 0;
            SelectedItem = null;

            foreach (UserItem item in list)
            {
                //item.CurrentDura = item.Info.Durability;
                //item.MaxDura = item.Info.Durability;
                Goods.Add(item);
            }

            Update();
        }



        public void Hide()
        {
            Visible = false;
            if (GameScene.Scene.CraftDialog.Visible)
                GameScene.Scene.CraftDialog.Hide();
        }
        public void Show()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].Recipe = PType == PanelType.Craft;
            }

            Visible = true;

            GameScene.Scene.InventoryDialog.Show();
        }
    }
    public sealed class NPCDropDialog : MirImageControl
    {

        public readonly MirButton ConfirmButton, HoldButton;
        public readonly MirItemCell ItemCell;
        public MirItemCell OldCell;
        public readonly MirLabel InfoLabel;
        public PanelType PType;

        public static UserItem TargetItem;
        public bool Hold;


        public NPCDropDialog()
        {
            Index = 392;
            Library = Libraries.Prguse;
            Location = new Point(264, 224);
            Sort = true;

            Click += NPCDropPanel_Click;

            HoldButton = new MirButton
            {
                HoverIndex = 294,
                Index = 293,
                Location = new Point(114, 36),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 295,
                Sound = SoundList.ButtonA,
            };
            HoldButton.Click += (o, e) => Hold = !Hold;

            ConfirmButton = new MirButton
            {
                HoverIndex = 291,
                Index = 290,
                Location = new Point(114, 62),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 292,
                Sound = SoundList.ButtonA,
            };
            ConfirmButton.Click += (o, e) => Confirm();

            InfoLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(30, 10),
                Parent = this,
                NotControl = true,
            };

            ItemCell = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.DropPanel,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(38, 72),
            };
            ItemCell.Click += (o, e) => ItemCell_Click();

            BeforeDraw += NPCDropPanel_BeforeDraw;
            AfterDraw += NPCDropPanel_AfterDraw;
        }

        private void NPCDropPanel_AfterDraw(object sender, EventArgs e)
        {
            if (Hold)
                Libraries.Title.Draw(295, 114 + DisplayLocation.X, 36 + DisplayLocation.Y);
        }

        private void NPCDropPanel_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;

            if (me == null) return;
            int x = me.X - DisplayLocation.X;
            int y = me.Y - DisplayLocation.Y;

            if (new Rectangle(20, 55, 75, 75).Contains(x, y))
                ItemCell_Click();
        }

        private void Confirm()
        {
            if (TargetItem == null) return;

            switch (PType)
            {
                case PanelType.Sell:
                    if (TargetItem.Info.Bind.HasFlag(BindMode.DontSell))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Cannot sell this item.", ChatType.System);
                        return;
                    }
                    if (GameScene.Gold + TargetItem.Price() / 2 <= uint.MaxValue)
                    {
                        Network.Enqueue(new C.SellItem { UniqueID = TargetItem.UniqueID, Count = TargetItem.Count });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat("Cannot carry anymore gold.", ChatType.System);
                    break;
                case PanelType.Repair:
                    if (TargetItem.Info.Bind.HasFlag(BindMode.DontRepair))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Cannot repair this item.", ChatType.System);
                        return;
                    }
                    if (GameScene.Gold >= TargetItem.RepairPrice() * GameScene.NPCRate)
                    {
                        Network.Enqueue(new C.RepairItem { UniqueID = TargetItem.UniqueID });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough gold.", ChatType.System);
                    break;
                case PanelType.SpecialRepair:
                    if ((TargetItem.Info.Bind.HasFlag(BindMode.DontRepair)) || (TargetItem.Info.Bind.HasFlag(BindMode.NoSRepair)))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Cannot repair this item.", ChatType.System);
                        return;
                    }
                    if (GameScene.Gold >= (TargetItem.RepairPrice() * 3) * GameScene.NPCRate)
                    {
                        Network.Enqueue(new C.SRepairItem { UniqueID = TargetItem.UniqueID });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough gold.", ChatType.System);
                    break;
                case PanelType.Consign:
                    if (TargetItem.Info.Bind.HasFlag(BindMode.DontStore))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Cannot store this item.", ChatType.System);
                        return;
                    }
                    MirAmountBox box = new MirAmountBox("Consignment Price:", TargetItem.Image, Globals.MaxConsignment, Globals.MinConsignment)
                    {
                        InputTextBox = { Text = string.Empty },
                        Amount = 0
                    };

                    box.Show();
                    box.OKButton.Click += (o, e) =>
                    {
                        Network.Enqueue(new C.ConsignItem { UniqueID = TargetItem.UniqueID, Price = box.Amount });
                        TargetItem = null;
                    };
                    return;
                case PanelType.Disassemble:
                    Network.Enqueue(new C.DisassembleItem { UniqueID = TargetItem.UniqueID });
                    break;
                case PanelType.Downgrade:
                    Network.Enqueue(new C.DowngradeAwakening { UniqueID = TargetItem.UniqueID });
                    break;
                case PanelType.Reset:
                    if (TargetItem.Info.NeedIdentify == false)
                    {
                        Network.Enqueue(new C.ResetAddedItem { UniqueID = TargetItem.UniqueID });
                    }
                    break;
                case PanelType.Refine:

                    for (int i = 0; i < GameScene.Scene.RefineDialog.Grid.Length; i++)
                    {
                        if (GameScene.Scene.RefineDialog.Grid[i].Item != null)
                        {
                            if (GameScene.Gold >= ((TargetItem.Info.RequiredAmount * 10) * GameScene.NPCRate))
                            {
                                Network.Enqueue(new C.RefineItem { UniqueID = TargetItem.UniqueID });
                                TargetItem = null;
                                return;
                            }
                            GameScene.Scene.ChatDialog.ReceiveChat(String.Format("You don't have enough gold to refine your {0}.", TargetItem.FriendlyName), ChatType.System);
                            return;
                        }

                    }
                    GameScene.Scene.ChatDialog.ReceiveChat(String.Format("You haven't deposited any items to refine your {0} with.", TargetItem.FriendlyName), ChatType.System);
                    break;
                case PanelType.CheckRefine:

                    if (TargetItem.RefineAdded == 0)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(String.Format("Your {0} hasn't been refined so it doesn't need checking.", TargetItem.FriendlyName), ChatType.System);
                        return;
                    }
                    Network.Enqueue(new C.CheckRefine { UniqueID = TargetItem.UniqueID });
                    break;

                case PanelType.ReplaceWedRing:

                    if (TargetItem.Info.Type != ItemType.Ring)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(String.Format("{0} isn't a ring.", TargetItem.FriendlyName), ChatType.System);
                        return;
                    }

                    Network.Enqueue(new C.ReplaceWedRing { UniqueID = TargetItem.UniqueID });
                    break;
            }


            TargetItem = null;
            OldCell.Locked = false;
            OldCell = null;
        }

        private void ItemCell_Click()
        {
            if (OldCell != null)
            {
                OldCell.Locked = false;
                TargetItem = null;
                OldCell = null;
            }

            if (GameScene.SelectedCell != null && PType == PanelType.Disassemble)
            {
                if (GameScene.SelectedCell.Item.Info.Grade != ItemGrade.None &&
                    GameScene.SelectedCell.Item.Info.Type != ItemType.Awakening)
                {
                    TargetItem = GameScene.SelectedCell.Item;
                    OldCell = GameScene.SelectedCell;
                    OldCell.Locked = true;
                    GameScene.SelectedCell = null;
                    return;
                }
            }

            if (GameScene.SelectedCell != null && PType == PanelType.Downgrade)
            {
                if (GameScene.SelectedCell.Item.Awake.getAwakeLevel() != 0)
                {
                    TargetItem = GameScene.SelectedCell.Item;
                    OldCell = GameScene.SelectedCell;
                    OldCell.Locked = true;
                    GameScene.SelectedCell = null;
                    return;
                }
            }

            if (GameScene.SelectedCell != null && PType == PanelType.Reset)
            {
                if (GameScene.SelectedCell.Item.IsAdded)
                {
                    TargetItem = GameScene.SelectedCell.Item;
                    OldCell = GameScene.SelectedCell;
                    OldCell.Locked = true;
                    GameScene.SelectedCell = null;
                    return;
                }
            }

            if (GameScene.SelectedCell != null && (PType == PanelType.Disassemble || PType == PanelType.Downgrade || PType == PanelType.Reset))
            {
                GameScene.SelectedCell.Locked = false;
                GameScene.SelectedCell = null;
                return;
            }

            //////////////////////////////////////

            if (GameScene.SelectedCell == null || GameScene.SelectedCell.GridType != MirGridType.Inventory ||
                (PType != PanelType.Sell && PType != PanelType.Consign && GameScene.SelectedCell.Item != null && GameScene.SelectedCell.Item.Info.Durability == 0))
                return;
            /*
            if (GameScene.SelectedCell.Item != null && (GameScene.SelectedCell.Item.Info.StackSize > 1 && GameScene.SelectedCell.Item.Count > 1))
            {
                MirAmountBox amountBox = new MirAmountBox("Sell Amount:", GameScene.SelectedCell.Item.Image, GameScene.SelectedCell.Item.Count);

                amountBox.OKButton.Click += (o, a) =>
                {
                    TargetItem = GameScene.SelectedCell.Item.Clone();
                    TargetItem.Count = amountBox.Amount;

                    OldCell = GameScene.SelectedCell;
                    OldCell.Locked = true;
                    GameScene.SelectedCell = null;
                    if (Hold) Confirm();
                };

                amountBox.Show();
            }
            else
            {
                TargetItem = GameScene.SelectedCell.Item;
                OldCell = GameScene.SelectedCell;
                OldCell.Locked = true;
                GameScene.SelectedCell = null;
                if (Hold) Confirm();
            }
            */
            TargetItem = GameScene.SelectedCell.Item;
            OldCell = GameScene.SelectedCell;
            OldCell.Locked = true;
            GameScene.SelectedCell = null;
            if (Hold) Confirm();
        }

        private void NPCDropPanel_BeforeDraw(object sender, EventArgs e)
        {
            string text;

            HoldButton.Visible = true;

            Index = 351;
            Library = Libraries.Prguse2;
            Location = new Point(264, 224);

            ConfirmButton.HoverIndex = 291;
            ConfirmButton.Index = 290;
            ConfirmButton.PressedIndex = 292;
            ConfirmButton.Location = new Point(114, 62);

            InfoLabel.Location = new Point(30, 10);

            ItemCell.Location = new Point(38, 72);

            switch (PType)
            {
                case PanelType.Sell:
                    text = "Sale: ";
                    break;
                case PanelType.Repair:
                    text = "Repair: ";
                    break;
                case PanelType.SpecialRepair:
                    text = "S. Repair: ";
                    break;
                case PanelType.Consign:
                    InfoLabel.Text = "Consignment: ";
                    return;
                case PanelType.Disassemble:
                    text = "Item will be Destroyed\n\n\n\n\n\n\n\n         ";
                    HoldButton.Visible = false;
                    Index = 711;
                    Library = Libraries.Title;
                    Location = new Point(234, 224);
                    ConfirmButton.HoverIndex = 716;
                    ConfirmButton.Index = 715;
                    ConfirmButton.PressedIndex = 717;
                    ConfirmButton.Location = new Point(62, 190);
                    InfoLabel.Location = new Point(44, 60);
                    ItemCell.Location = new Point(83, 94);
                    break;
                case PanelType.Downgrade:
                    text = "Downgrade: ";
                    HoldButton.Visible = false;
                    break;
                case PanelType.Reset:
                    text = "Reset: ";
                    HoldButton.Visible = false;
                    break;
                case PanelType.Refine:
                    text = "Refine: ";
                    HoldButton.Visible = false;
                    ConfirmButton.Visible = true;
                    GameScene.Scene.RefineDialog.Show();
                    break;
                case PanelType.CheckRefine:
                    text = "Check Refine";
                    HoldButton.Visible = false;
                    ConfirmButton.Visible = true;
                    break;
                case PanelType.ReplaceWedRing:
                    text = "Replace: ";
                    HoldButton.Visible = false;
                    ConfirmButton.Visible = true;
                    break;

                default: return;

            }

            if (TargetItem != null)
            {

                switch (PType)
                {
                    case PanelType.Sell:
                        text += (TargetItem.Price() / 2).ToString();
                        break;
                    case PanelType.Repair:
                        text += (TargetItem.RepairPrice() * GameScene.NPCRate).ToString();
                        break;
                    case PanelType.SpecialRepair:
                        text += ((TargetItem.RepairPrice() * 3) * GameScene.NPCRate).ToString();
                        break;
                    case PanelType.Disassemble:
                        text += TargetItem.DisassemblePrice().ToString();
                        break;
                    case PanelType.Downgrade:
                        text += TargetItem.DowngradePrice().ToString();
                        break;
                    case PanelType.Reset:
                        text += TargetItem.ResetPrice().ToString();
                        break;
                    case PanelType.Refine:
                        text += ((TargetItem.Info.RequiredAmount * 10) * GameScene.NPCRate).ToString();
                        break;
                    case PanelType.ReplaceWedRing:
                        text += ((TargetItem.Info.RequiredAmount * 10) * GameScene.NPCRate).ToString();
                        break;
                    default: return;
                }

                text += " Gold";
            }

            InfoLabel.Text = text;
        }

        public void Hide()
        {
            if (OldCell != null)
            {
                OldCell.Locked = false;
                TargetItem = null;
                OldCell = null;
            }
            Visible = false;
        }
        public void Show()
        {
            Hold = false;
            GameScene.Scene.InventoryDialog.Show();
            Visible = true;
        }
    }
    public sealed class NPCAwakeDialog : MirImageControl
    {

        public MirButton UpgradeButton, CloseButton;
        public MirItemCell[] ItemCells = new MirItemCell[7];
        public MirDropDownBox SelectAwakeType;
        public AwakeType CurrentAwakeType = AwakeType.None;
        public MirLabel GoldLabel, NeedItemLabel1, NeedItemLabel2;

        public static UserItem[] Items = new UserItem[7];
        public static int[] ItemsIdx = new int[7];

        public NPCAwakeDialog()
        {
            Index = 710;
            Library = Libraries.Title;
            Location = new Point(0, 0);
            Sort = true;
            Movable = true;

            GoldLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(112, 354),
                Parent = this,
                NotControl = true,
            };

            NeedItemLabel1 = new MirLabel
            {
                AutoSize = true,
                Location = new Point(67, 317),//
                Parent = this,
                NotControl = true,
            };

            NeedItemLabel2 = new MirLabel
            {
                AutoSize = true,
                Location = new Point(192, 317),//(155, 316),
                Parent = this,
                NotControl = true,
            };

            UpgradeButton = new MirButton
            {
                HoverIndex = 713,
                Index = 712,
                Location = new Point(115, 391), //new Point(181, 135),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 714,
                Sound = SoundList.ButtonA,
            };
            UpgradeButton.Click += (o, e) => Awakening();

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(284, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            ItemCells[0] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(202, 91),
                ItemSlot = 0,
            };
            //ItemCells[0].AfterDraw += (o, e) => ItemCell_AfterDraw();
            //ItemCells[0].Click += (o, e) => ItemCell_Click();

            ItemCells[1] = new MirItemCell //Required
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(31, 316),
                ItemSlot = 1,
                Enabled = false,

            };

            ItemCells[2] = new MirItemCell //Required
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(155, 316),
                ItemSlot = 2,
                Enabled = false,
            };

            ItemCells[3] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(175, 199),
                ItemSlot = 3,
            };

            ItemCells[4] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(230, 199),
                ItemSlot = 4,
            };

            ItemCells[5] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(175, 256),
                ItemSlot = 5,
            };

            ItemCells[6] = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.AwakenItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(230, 256),
                ItemSlot = 6,
            };

            SelectAwakeType = new MirDropDownBox()
            {
                Parent = this,
                Location = new Point(35, 141),
                Size = new Size(109, 14),
                ForeColour = Color.White,
                Visible = true,
                Enabled = true,
            };
            SelectAwakeType.ValueChanged += (o, e) => OnAwakeTypeSelect(SelectAwakeType._WantedIndex);
        }

        public void ItemCellClear()
        {
            if (ItemCells[1].Item != null)
            {
                ItemCells[1].Item = null;
            }
            if (ItemCells[2].Item != null)
            {
                ItemCells[2].Item = null;
            }


            NeedItemLabel2.Text = "";
            NeedItemLabel1.Text = "";
            GoldLabel.Text = "";
        }

        public void ItemCell_Click()
        {
            ItemCellClear();
            SelectAwakeType.Items.Clear();

            if (Items[0] == null)
            {
                SelectAwakeType.Items.Add("Select Upgrade Item.");
                SelectAwakeType.SelectedIndex = SelectAwakeType.Items.Count - 1;
                CurrentAwakeType = AwakeType.None;
            }
            else
            {
                if (Items[0].Awake.getAwakeLevel() == 0)
                {
                    SelectAwakeType.Items.Add("Select Upgrade Type.");
                    if (Items[0].Info.Type == ItemType.Weapon)
                    {
                        SelectAwakeType.Items.Add("Bravery Glyph");
                        SelectAwakeType.Items.Add("Magic Glyph");
                        SelectAwakeType.Items.Add("Soul Glyph");
                    }
                    else if (Items[0].Info.Type == ItemType.Helmet)
                    {
                        SelectAwakeType.Items.Add("Protection Glyph");
                        SelectAwakeType.Items.Add("EvilSlayer Glyph");
                    }
                    else
                    {
                        SelectAwakeType.Items.Add("Body Glyph");
                    }
                }
                else
                {
                    SelectAwakeType.Items.Add(getAwakeTypeText(Items[0].Awake.type));
                    if (CurrentAwakeType != Items[0].Awake.type)
                    {
                        CurrentAwakeType = Items[0].Awake.type;
                        OnAwakeTypeSelect(0);
                    }
                }
            }
        }

        public string getAwakeTypeText(AwakeType type)
        {
            string typeName = "";
            switch (type)
            {
                case AwakeType.DC:
                    typeName = "Bravery Glyph";
                    break;
                case AwakeType.MC:
                    typeName = "Magic Glyph";
                    break;
                case AwakeType.SC:
                    typeName = "Soul Glyph";
                    break;
                case AwakeType.AC:
                    typeName = "Protection Glyph";
                    break;
                case AwakeType.MAC:
                    typeName = "EvilSlayer Glyph";
                    break;
                case AwakeType.HPMP:
                    typeName = "Body Glyph";
                    break;
                default:
                    typeName = "Select Upgrade Item.";
                    break;
            }
            return typeName;
        }

        public AwakeType getAwakeType(string typeName)
        {
            AwakeType type = AwakeType.None;
            switch (typeName)
            {
                case "Bravery Glyph":
                    type = AwakeType.DC;
                    break;
                case "Magic Glyph":
                    type = AwakeType.MC;
                    break;
                case "Soul Glyph":
                    type = AwakeType.SC;
                    break;
                case "Protection Glyph":
                    type = AwakeType.AC;
                    break;
                case "EvilSlayer Glyph":
                    type = AwakeType.MAC;
                    break;
                case "Body Glyph":
                    type = AwakeType.HPMP;
                    break;
                default:
                    type = AwakeType.None;
                    break;
            }

            return type;
        }

        public void OnAwakeTypeSelect(int Index)
        {
            SelectAwakeType.SelectedIndex = Index;

            AwakeType type = getAwakeType(SelectAwakeType.Items[SelectAwakeType.SelectedIndex]);
            CurrentAwakeType = type;
            if (type != AwakeType.None)
            {
                Network.Enqueue(new C.AwakeningNeedMaterials { UniqueID = Items[0].UniqueID, Type = type });
            }
        }

        public void setNeedItems(ItemInfo[] Materials, byte[] MaterialsCount)
        {
            if (MaterialsCount[0] != 0)
            {
                ItemCells[1].Item = new UserItem(Materials[0]);
                ItemCells[1].Item.Count = MaterialsCount[0];
                NeedItemLabel1.Text = Regex.Replace(ItemCells[1].Item.Info.Name, @"[\d-]", string.Empty) + "\nQuantity: " + MaterialsCount[0].ToString();
            }
            else
            {
                ItemCells[1].Item = null;
                NeedItemLabel1.Text = "";
            }

            if (MaterialsCount[1] != 0)
            {
                ItemCells[2].Item = new UserItem(Materials[1]);
                ItemCells[2].Item.Count = MaterialsCount[1];
                NeedItemLabel2.Text = Regex.Replace(ItemCells[2].Item.Info.Name, @"[\d-]", string.Empty) + "\nQuantity:" + MaterialsCount[1].ToString();
            }
            else
            {
                ItemCells[2].Item = null;
                NeedItemLabel2.Text = "";
            }

            if (ItemCells[0].Item != null)
                GoldLabel.Text = ItemCells[0].Item.AwakeningPrice().ToString();
        }

        public bool CheckNeedMaterials()
        {
            int maxEqual = (Items[1] == null || Items[2] == null) ? 1 : 2;
            int equal = 0;
            for (int i = 1; i < 3; i++)
            {
                if (Items[i] == null) continue;
                for (int j = 3; j < 5; j++)
                {
                    if (Items[j] == null) continue;
                    if (Items[i].Info.Name == Items[j].Info.Name &&
                        Items[i].Count <= Items[j].Count)
                        equal++;
                }
            }
            return equal >= maxEqual;
        }

        public void Awakening()
        {
            if (CheckNeedMaterials())
            {
                AwakeType type = getAwakeType(SelectAwakeType.Items[SelectAwakeType.SelectedIndex]);

                if (type != AwakeType.None)
                {
                    Network.Enqueue(new C.Awakening { UniqueID = Items[0].UniqueID, Type = type });
                    MapControl.AwakeningAction = true;
                }
            }
        }

        public void Hide()
        {
            foreach (var item in ItemCells)
            {
                if (item.Item != null)
                {
                    Network.Enqueue(new C.AwakeningLockedItem { UniqueID = item.Item.UniqueID, Locked = false });
                    item.Item = null;
                }
            }

            for (int i = 0; i < ItemsIdx.Length; i++)
            {
                ItemsIdx[i] = 0;
            }

            ItemCell_Click();
            Visible = false;
        }

        public void Show()
        {
            Visible = true;

            GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, 0);
            GameScene.Scene.InventoryDialog.Show();
        }
    }


    public sealed class CraftDialog : MirImageControl
    {
        public static UserItem RecipeItem;
        public static UserItem[] IngredientSlots = new UserItem[16];

        public List<KeyValuePair<MirItemCell, ulong>> Selected = new List<KeyValuePair<MirItemCell, ulong>>();

        public MirItemCell[] Grid;
        public static UserItem[] ShadowItems = new UserItem[16];

        public MirButton CraftButton, CloseButton;

        public CraftDialog()
        {
            Index = 1002;
            Library = Libraries.Prguse;
            Location = new Point(0, 0);
            Sort = true;
            BeforeDraw += CraftDialog_BeforeDraw;
            Movable = true;

            Grid = new MirItemCell[4 * 4];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    int idx = 4 * y + x;
                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = idx,
                        GridType = MirGridType.Craft,
                        Library = Libraries.Items,
                        Parent = this,
                        Size = new Size(34, 32),
                        Location = new Point(x * 34 + 12 + x, y * 32 + 37 + y),
                        Border = true,
                        BorderColour = Color.Lime
                    };
                    Grid[idx].Click += Grid_Click;
                }
            }

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(139, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            CraftButton = new MirButton
            {
                HoverIndex = 337,
                Index = 336,
                Location = new Point(41, 177),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 338,
                Sound = SoundList.ButtonA,
                GrayScale = true,
                Enabled = false
            };
            CraftButton.Click += (o, e) => CraftItem();
        }

        void CraftDialog_BeforeDraw(object sender, EventArgs e)
        {
            if (!GameScene.Scene.InventoryDialog.Visible)
            {
                Hide();
                return;
            }
        }


        private void Grid_Click(object sender, EventArgs e)
        {
            MirItemCell cell = (MirItemCell)sender;

            if (cell == null || cell.ShadowItem == null)
                return;

            if (GameScene.SelectedCell == null || GameScene.SelectedCell.GridType != MirGridType.Inventory || GameScene.SelectedCell.Locked)
                return;

            if (GameScene.SelectedCell.Item.Info != cell.ShadowItem.Info || cell.Item != null)
                return;


            cell.Item = GameScene.SelectedCell.Item;

            Selected.Add(new KeyValuePair<MirItemCell, ulong>(GameScene.SelectedCell, GameScene.SelectedCell.Item.UniqueID));

            GameScene.SelectedCell.Locked = true;
            GameScene.SelectedCell = null;

            RefreshCraftCells(RecipeItem);
        }

        public void Hide()
        {
            if (!Visible) return;

            Visible = false;

            ResetCells();
        }

        public void Show()
        {
            Visible = true;

            Location = new Point(GameScene.Scene.InventoryDialog.Location.X, GameScene.Scene.InventoryDialog.Location.Y + 236);
        }

        private void CraftItem()
        {
            if (RecipeItem == null) return;

            uint max = 99;
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i] == null || Grid[i].Item == null) continue;

                uint temp = Grid[i].Item.Count / Grid[i].ShadowItem.Count;

                if (temp < max) max = temp;
            }

            if (max > RecipeItem.Info.StackSize)
                max = RecipeItem.Info.StackSize;

            //TODO - Check Max slots spare against slots to be used (stacksize/quantity)
            //TODO - GetMaxItemGain

            if (RecipeItem.Weight > (MapObject.User.MaxBagWeight - MapObject.User.CurrentBagWeight))
            {
                GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough weight.", ChatType.System);
                return;
            }

            if (max > 1)
            {
                MirAmountBox amountBox = new MirAmountBox("Craft Amount:", RecipeItem.Info.Image, max, 0, max);

                amountBox.OKButton.Click += (o, e) =>
                {
                    if (amountBox.Amount > 0)
                    {
                        if (!HasCraftItems(RecipeItem, amountBox.Amount))
                        {
                            GameScene.Scene.ChatDialog.ReceiveChat("You do not have the required ingredients.", ChatType.System);
                            return;
                        }

                        Network.Enqueue(new C.CraftItem { UniqueID = RecipeItem.UniqueID, Count = amountBox.Amount, Slots = Selected.Select(x => x.Key.ItemSlot).ToArray() });
                    }
                };

                amountBox.Show();
            }
            else
            {
                Network.Enqueue(new C.CraftItem { UniqueID = RecipeItem.UniqueID, Count = 1, Slots = Selected.Select(x => x.Key.ItemSlot).ToArray() });
            }
        }

        private bool HasCraftItems(UserItem item, uint count)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].ShadowItem == null) continue;

                if (Grid[i].Item == null || Grid[i].Item.Count < (Grid[i].ShadowItem.Count * count)) return false;
            }

            return true;
        }

        public void ResetCells()
        {
            RecipeItem = null;
            for (int j = 0; j < Grid.Length; j++)
            {
                IngredientSlots[j] = null;
                ShadowItems[j] = null;
            }

            for (int i = 0; i < Selected.Count; i++)
            {
                Selected[i].Key.Locked = false;
            }

            Selected.Clear();
        }

        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }

        public void RefreshCraftCells(UserItem selectedItem)
        {
            RecipeItem = selectedItem;

            CraftButton.Enabled = true;
            CraftButton.GrayScale = false;

            ClientRecipeInfo recipe = GameScene.RecipeInfoList.SingleOrDefault(x => x.Item.ItemIndex == selectedItem.ItemIndex);

            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                if (i >= IngredientSlots.Length) break;

                ShadowItems[i] = recipe.Ingredients[i];

                bool needItem = Grid[i].Item == null || Grid[i].Item.Count < Grid[i].ShadowItem.Count;

                if (needItem)
                {
                    CraftButton.Enabled = false;
                    CraftButton.GrayScale = true;
                }
            }
        }
    }


    public sealed class RefineDialog : MirImageControl
    {
        public MirItemCell[] Grid;
        public MirButton RefineButton;

        public RefineDialog()
        {
            Index = 1002;
            Library = Libraries.Prguse;
            Location = new Point(0, 225);
            Sort = true;

            MirImageControl TitleLabel = new MirImageControl
            {
                Index = 18,
                Library = Libraries.Title,
                Location = new Point(28, 8),
                Parent = this
            };


            Grid = new MirItemCell[4 * 4];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    int idx = 4 * y + x;
                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = idx,
                        GridType = MirGridType.Refine,
                        Library = Libraries.Items,
                        Parent = this,
                        Size = new Size(34, 32),
                        Location = new Point(x * 34 + 12 + x, y * 32 + 37 + y),
                    };
                }
            }
        }

        public void Hide()
        {
            Visible = false;
            RefineCancel();
        }

        public void Show()
        {
            Visible = true;
        }

        public void RefineCancel()
        {
            Network.Enqueue(new C.RefineCancel());
        }

        public void RefineReset()
        {
            for (int i = 0; i < Grid.Length; i++)
                Grid[i].Item = null;
        }



        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }

    }
    public sealed class StorageDialog : MirImageControl
    {
        public MirItemCell[] Grid;
        public MirButton Storage1Button, Storage2Button, RentButton, ProtectButton, CloseButton;
        public MirImageControl LockedPage;
        public MirLabel RentalLabel;

        public StorageDialog()
        {
            Index = 586;
            Library = Libraries.Prguse;
            Location = new Point(0, 0);
            Sort = true;

            MirImageControl TitleLabel = new MirImageControl
            {
                Index = 0,
                Library = Libraries.Title,
                Location = new Point(18, 8),
                Parent = this
            };

            LockedPage = new MirImageControl
            {
                Index = 2443,
                Library = Libraries.Prguse,
                Location = new Point(8, 59),
                Parent = this,
                Visible = false
            };

            Storage1Button = new MirButton
            {
                HoverIndex = 743,
                Index = 743,
                Location = new Point(8, 36),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 744,
                Sound = SoundList.ButtonA,
            };
            Storage1Button.Click += (o, e) =>
            {
                RefreshStorage1();
            };

            Storage2Button = new MirButton
            {
                HoverIndex = 746,
                Index = 746,
                Location = new Point(80, 36),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 746,
                Sound = SoundList.ButtonA,
                Visible = true
            };
            Storage2Button.Click += (o, e) =>
            {
                RefreshStorage2();
            };
            RentButton = new MirButton
            {
                Index = 483,
                HoverIndex = 484,
                PressedIndex = 485,
                Library = Libraries.Title,
                Location = new Point(283, 33),
                Parent = this,
                Sound = SoundList.ButtonA,
                Visible = true,
            };
            RentButton.Click += (o, e) =>
            {
                MirMessageBox messageBox;

                if (GameScene.User.HasExpandedStorage)
                    messageBox = new MirMessageBox("Would you like to extend your rental period for 10 days at a cost of 1,000,000 gold?", MirMessageBoxButtons.OKCancel);
                else
                    messageBox = new MirMessageBox("Would you like to rent extra storage for 10 days at a cost of 1,000,000 gold?", MirMessageBoxButtons.OKCancel);

                messageBox.OKButton.Click += (o1, a) =>
                {
                    Network.Enqueue(new C.Chat { Message = "@ADDSTORAGE" });
                };
                messageBox.Show();
            };

            ProtectButton = new MirButton
            {
                HoverIndex = 114,
                Index = 113,
                Location = new Point(328, 33),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 115,
                Sound = SoundList.ButtonA,
                Visible = true
            };
            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(363, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            RentalLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(40, 322),
                AutoSize = true,
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                NotControl = true,
                Text = "Expanded Storage Locked",
                ForeColour = Color.Red
            };

            Grid = new MirItemCell[10 * 16];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    int idx = 10 * y + x;

                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = idx,
                        GridType = MirGridType.Storage,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(x * 36 + 9 + x, y % 8 * 32 + 60 + y % 8),
                    };

                    if (idx >= 80)
                        Grid[idx].Visible = false;
                }
            }
        }

        public void Hide()
        {
            Visible = false;
        }

        public void Show()
        {
            GameScene.Scene.InventoryDialog.Show();
            RefreshStorage1();

            Visible = true;
        }

        public void RefreshStorage1()
        {
            if (GameScene.User == null) return;

            Storage1Button.Index = 743;
            Storage1Button.HoverIndex = 743;
            Storage2Button.Index = 746;
            Storage2Button.HoverIndex = 746;

            foreach (var grid in Grid)
            {
                if (grid.ItemSlot < 80)
                    grid.Visible = true;
                else
                    grid.Visible = false;
            }

            RentButton.Visible = false;
            LockedPage.Visible = false;
            RentalLabel.Visible = false;
        }

        public void RefreshStorage2()
        {
            if (GameScene.User == null) return;

            Storage1Button.Index = 744;
            Storage1Button.HoverIndex = 744;
            Storage2Button.Index = 745;
            Storage2Button.HoverIndex = 745;

            RentalLabel.Visible = true;

            if (GameScene.User.HasExpandedStorage)
            {
                RentButton.Visible = true;
                LockedPage.Visible = false;
                RentalLabel.Text = "Expanded Storage Expires On: " + GameScene.User.ExpandedStorageExpiryTime.ToString();
                RentalLabel.ForeColour = Color.White;
            }
            else
            {
                RentalLabel.Text = "Expanded Storage Locked";
                RentalLabel.ForeColour = Color.Red;
                RentButton.Visible = true;
                LockedPage.Visible = true;
            }

            foreach (var grid in Grid)
            {
                if (grid.ItemSlot < 80 || !GameScene.User.HasExpandedStorage)
                    grid.Visible = false;
                else
                    grid.Visible = true;
            }
        }

        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }
    }
}

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using Font = System.Drawing.Font;
using C = ClientPackets;
using System.Diagnostics;

namespace Client.MirScenes.Dialogs
{
    public sealed class NPCDialog : MirImageControl
    {
        public static Regex R = new Regex(@"<((.*?)\/(\@.*?))>");
        public static Regex C = new Regex(@"{((.*?)\/(.*?))}");
        public static Regex L = new Regex(@"\(((.*?)\/(.*?))\)");
        public static Regex B = new Regex(@"<<((.*?)\/(\@.*?))>>");

        // New regex patterns for NPC/Monster/Item linking (using IDX)
        public static Regex MonsterLink = new Regex(@"\[MONSTER:(?<idx>\d+)(\|(?<name>[^\]]+))?\]|<\$MONSTER:(?<idx>\d+)>", RegexOptions.IgnoreCase);
        public static Regex NPCLink = new Regex(@"\[NPC:(?<idx>\d+)(\|(?<name>[^\]]+))?\]|<\$NPC:(?<idx>\d+)>", RegexOptions.IgnoreCase);
        public static Regex ItemLink = new Regex(@"\[ITEM:(?<idx>\d+)(\|(?<name>[^\]]+))?\]|<\$ITEM:(?<idx>\d+)>", RegexOptions.IgnoreCase);

        public MirButton CloseButton, UpButton, DownButton, PositionBar, QuestButton, HelpButton;
        public MirLabel[] TextLabel;
        public List<MirLabel> TextButtons;
        public List<BigButton> BigButtons;
        public BigButtonDialog BigButtonDialog;

        public MirLabel NameLabel;

        Font font = new Font(Settings.FontName, 9F);

        private static readonly HashSet<int> PendingMapInfoRequests = new HashSet<int>();

        public List<string> CurrentLines = new List<string>();
        private int _index = 0;
        public int MaximumLines = 8;

        // Tooltip control for linked objects
        private MirControl _tooltipPanel;
        private MirImageControl _tooltipImage;
        private MirLabel _tooltipTitle;
        private MirLabel _tooltipContent;
        
        public NPCDialog()
        {
            Index = 995;
            Library = Libraries.Prguse;

            // Hide tooltip when dialog is clicked
            this.MouseDown += (o, e) => HideTooltip();
            
            TextLabel = new MirLabel[30];
            TextButtons = new List<MirLabel>();
            BigButtons = new List<BigButton>();
            Size = Size;
            AutoSize = false;

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

            HelpButton = new MirButton
            {
                Index = 257,
                HoverIndex = 258,
                PressedIndex = 259,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(390, 3),
                Sound = SoundList.ButtonA,
            };
            HelpButton.Click += (o, e) => GameScene.Scene.HelpDialog.DisplayPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Purchasing));

            BigButtonDialog = new BigButtonDialog()
            {
                Parent = this,               
            };

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
                Sound = SoundList.ButtonA,
                Visible = false
            };

            QuestButton.Click += (o, e) => GameScene.Scene.QuestListDialog.Toggle();
            
            // Initialize tooltip panel
            InitializeTooltipPanel();
        }

        private void InitializeTooltipPanel()
        {
            // Use simple background like item tooltips
            _tooltipPanel = new MirControl
            {
                Parent = GameScene.Scene,
                Visible = false,
                NotControl = true,
                Sort = true,
                BackColour = Color.FromArgb(240, 80, 60, 40), // Darker brownish color
                Border = true,
                BorderColour = Color.FromArgb(255, 120, 100, 80), // Brownish border
                DrawControlTexture = true,
                Modal = false
            };

            _tooltipImage = new MirImageControl
            {
                Parent = _tooltipPanel,
                Visible = false,
                NotControl = true,
                Size = new Size(48, 48),
                Location = new Point(10, 10),
                DrawImage = true
            };

            _tooltipTitle = new MirLabel
            {
                Parent = _tooltipPanel,
                AutoSize = true,
                ForeColour = Color.Yellow, // Title in yellow like KR
                Location = new Point(70, 5),
                NotControl = true,
                OutLine = true
            };

            _tooltipContent = new MirLabel
            {
                Parent = _tooltipPanel,
                AutoSize = true,
                ForeColour = Color.White, // Content in white
                Location = new Point(70, 25),
                NotControl = true,
                DrawFormat = TextFormatFlags.WordBreak | TextFormatFlags.ExpandTabs,
                OutLine = true
            };
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

        private void ButtonClicked(string action)
        {
            if (action == "@Exit")
            {
                Hide();
                return;
            }

            if (CMain.Time <= GameScene.NPCTime) return;

            GameScene.NPCTime = CMain.Time + 5000;
            Network.Enqueue(new C.CallNPC { ObjectID = GameScene.NPCID, Key = $"[{action}]" });
        }


        public void NewText(List<string> lines, bool resetIndex = true)
        {
            Size = TrueSize;

            if (resetIndex)
            {
                _index = 0;
                CurrentLines = lines;
                UpdatePositionBar();
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    string currentLine = lines[i];

                    List<Match> bigButtonSortedList = B.Matches(currentLine).Cast<Match>().OrderBy(o => o.Index).ToList();

                    for (int j = 0; j < bigButtonSortedList.Count; j++)
                    {
                        Match match = bigButtonSortedList[j];
                        Capture capture = match.Groups[1].Captures[0];
                        string txt = match.Groups[2].Captures[0].Value;
                        string action = match.Groups[3].Captures[0].Value;
                        string colourString = "RoyalBlue";

                        string[] actionSplit = action.Split('/');

                        action = actionSplit[0];
                        if (actionSplit.Length > 1)
                            colourString = actionSplit[1];

                        Color color = Color.FromName(colourString);

                        BigButton button = new BigButton
                        {
                            Index = 841,
                            HoverIndex = 842,
                            PressedIndex = 843,
                            Library = Libraries.Title,
                            Sound = SoundList.ButtonA,
                            Text = txt,
                            FontColour = Color.White,
                            ForeColour = color
                        };

                        button.Click += (o, e) =>
                        {
                            ButtonClicked(action);
                        };
                        BigButtons.Insert(0, button);
                    }

                    var bigButtonString = B.ToString();

                    var oldCurrentLine = currentLine;

                    currentLine = Regex.Replace(currentLine, bigButtonString, "");

                    if (string.IsNullOrWhiteSpace(currentLine) && oldCurrentLine != currentLine)
                        lines.RemoveAt(i);
                }

                if (BigButtons.Count > 0)
                {
                    int minimumButtons = 0;
                    if (string.IsNullOrWhiteSpace(string.Concat(lines)))
                    {
                        BigButtonDialog.Location = new Point(1, 27);
                        minimumButtons = 4;
                    }
                    else
                        BigButtonDialog.Location = new Point(1, Size.Height - 33);

                    BigButtonDialog.Show(BigButtons, minimumButtons);
                    Size = new Size(Size.Width, BigButtonDialog.Location.Y + BigButtonDialog.Size.Height);
                }
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

            QuestButton.Location = new Point(172, Size.Height - 30);

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
                    Location = new Point(8, 34 + (i - _index) * 18),
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
                matchList.AddRange(L.Matches(currentLine).Cast<Match>());

                matchList.AddRange(MonsterLink.Matches(currentLine).Cast<Match>());
                matchList.AddRange(NPCLink.Matches(currentLine).Cast<Match>());
                matchList.AddRange(ItemLink.Matches(currentLine).Cast<Match>());

                int oldLength = currentLine.Length;

                foreach (Match match in matchList.OrderBy(o => o.Index).ToList())
                {
                    int offSet = oldLength - currentLine.Length;

                    // Handle new link types FIRST (Monster, NPC, Item)
                    bool isMonsterLink = MonsterLink.Match(match.Value).Success;
                    bool isNPCLink = NPCLink.Match(match.Value).Success;
                    bool isItemLink = ItemLink.Match(match.Value).Success;

                    if (isMonsterLink || isNPCLink || isItemLink)
                    {
                        // Extract the index from the link
                        string linkIdx = match.Groups["idx"].Captures.Count > 0 ? match.Groups["idx"].Captures[0].Value : match.Groups["idx"].Value;
                        string providedName = match.Groups["name"].Success ? match.Groups["name"].Captures[0].Value : null;

                        string linkType = "ITEM";
                        switch (true)
                        {
                            case true when isMonsterLink:
                                linkType = "MONSTER";
                                break;
                            case true when isNPCLink:
                                linkType = "NPC";
                                break;
                        }

                        string displayName = GetDisplayNameForLink(linkType, linkIdx, providedName);
                        if (string.IsNullOrEmpty(displayName)) displayName = $"LINK_{linkIdx}";

                        int matchStart = match.Index - offSet;
                        int matchLength = match.Length;
                        currentLine = currentLine.Remove(matchStart, matchLength).Insert(matchStart, displayName);

                        string textUpToLink = currentLine.Substring(0, matchStart);
                        Point offset = CalculateLinkOffset(textUpToLink, TextLabel[i]);

                        NewLink(displayName, linkType, linkIdx, TextLabel[i].Location.Add(offset));

                        continue;
                    }

                    bool hasMultipleGroups = match.Groups.Count > 3 && match.Groups[2].Captures.Count > 0 && match.Groups[3].Captures.Count > 0;

                    if (hasMultipleGroups)
                    {
                        Capture capture = match.Groups[1].Captures[0];
                        string txt = match.Groups[2].Captures[0].Value;
                        string action = match.Groups[3].Captures[0].Value;

                        currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, txt);
                        string text2 = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                        Size size2 = TextRenderer.MeasureText(CMain.Graphics, text2, TextLabel[i].Font, TextLabel[i].Size, TextFormatFlags.TextBoxControl);

                        if (R.Match(match.Value).Success)
                            NewButton(txt, action, TextLabel[i].Location.Add(new Point(size2.Width - 10, 0)));

                        if (C.Match(match.Value).Success)
                            NewColour(txt, action, TextLabel[i].Location.Add(new Point(size2.Width - 10, 0)));

                        if (L.Match(match.Value).Success)
                            NewButton(txt, null, TextLabel[i].Location.Add(new Point(size2.Width - 10, 0)), action);
                    }
                }
                TextLabel[i].Text = currentLine;
                TextLabel[i].MouseWheel += NPCDialog_MouseWheel;
            }
        }

        private void NewButton(string text, string key, Point p, string link = "")
        {
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

            temp.MouseEnter += (o, e) => temp.ForeColour = Color.Red;
            temp.MouseLeave += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseDown += (o, e) => temp.ForeColour = Color.Yellow;
            temp.MouseUp += (o, e) => temp.ForeColour = Color.Red;

            if (!string.IsNullOrEmpty(link))
            {
                temp.Click += (o, e) =>
                {
                    if (link.StartsWith("http://", true, CultureInfo.InvariantCulture))
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = link,
                            UseShellExecute = true
                        });
                    }
                };
            }
            else
            {
                temp.Click += (o, e) =>
                {
                    ButtonClicked(key);
                };
            }

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

        private void NewLink(string text, string linkType, string linkName, Point p)
        {
            MirLabel temp = new MirLabel
            {
                AutoSize = true,
                Visible = true,
                Parent = this,
                Location = p,
                Text = text,
                ForeColour = Color.Cyan,
                Font = font,
                NotControl = false // Make sure it can receive mouse events
            };

            temp.MouseEnter += (o, e) =>
            {
                temp.ForeColour = Color.Orange;

                // All link types (MONSTER, NPC, ITEM) now use the custom ShowTooltip method
                ShowTooltip(linkType, linkName, temp.DisplayLocation);
            };

            temp.MouseLeave += (o, e) =>
            {
                temp.ForeColour = Color.Cyan;
                HideTooltip();
            };

            temp.Click += (o, e) =>
            {
                HideTooltip();
            };

            temp.MouseWheel += NPCDialog_MouseWheel;
            TextButtons.Add(temp);
        }

        private void ShowTooltip(string linkType, string linkName, Point anchor)
        {
            if (_tooltipPanel == null) return;

            string title = "";
            string content = "";
            bool showImage = false;

            switch (linkType)
            {
                case "MONSTER":
                    // Get monster name instead of index
                    if (int.TryParse(linkName, out int monsterIdx))
                    {
                        var monsterInfo = GameScene.MonsterInfoList.FirstOrDefault(x => x.Index == monsterIdx);
                        if (monsterInfo != null)
                        {
                            string monsterName = string.IsNullOrEmpty(monsterInfo.GameName) ? monsterInfo.Name : monsterInfo.GameName;
                            title = monsterName;
                            content = GetMonsterInfo(linkName);

                            // Show monster image
                            if ((ushort)monsterInfo.Image < Libraries.Monsters.Length)
                            {
                                _tooltipImage.Library = Libraries.Monsters[(ushort)monsterInfo.Image];
                                _tooltipImage.Index = 20;
                                _tooltipImage.Location = new Point(10, 10);
                                showImage = true;
                            }
                        }
                        else
                        {
                            GameScene.RequestMonsterInfo(monsterIdx);
                            title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterIndexTitle, linkName);
                            content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterInfoLoading);
                        }
                    }
                    else
                    {
                        title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterIndexTitle, linkName);
                        content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidLinkIndex);
                    }
                    break;
                case "NPC":
                    // Get NPC name instead of index
                    if (int.TryParse(linkName, out int npcIdx))
                    {
                        var npcInfo = GameScene.NPCInfoList.FirstOrDefault(x => x.Index == npcIdx);
                        if (npcInfo != null)
                        {
                            title = npcInfo.Name;
                            content = GetNPCInfo(linkName);

                            // Show NPC image
                            if (npcInfo.Image < Libraries.NPCs.Length)
                            {
                                _tooltipImage.Library = Libraries.NPCs[npcInfo.Image];
                                _tooltipImage.Index = 0;
                                _tooltipImage.Location = new Point(10, 10);
                                showImage = true;
                            }
                        }
                        else
                        {
                            GameScene.RequestNPCInfo(npcIdx);
                            title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCIndexTitle, linkName);
                            content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCInfoLoading);
                        }
                    }
                    else
                    {
                        title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCIndexTitle, linkName);
                        content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidLinkIndex);
                    }
                    break;
                case "ITEM":
                    // Get item name instead of index
                    if (int.TryParse(linkName, out int itemIdx))
                    {
                        var item = GameScene.ItemInfoList.FirstOrDefault(x => x.Index == itemIdx);
                        if (item != null)
                        {
                            title = item.FriendlyName ?? item.Name;
                            content = GetItemInfo(linkName);

                            // Show item image using Items_Tooltip_32bit library for KR style
                            _tooltipImage.Library = Libraries.Items_Tooltip_32bit;
                            _tooltipImage.Index = item.Image;
                            _tooltipImage.Location = new Point(10, 10);
                            showImage = true;
                        }
                        else
                        {
                            GameScene.RequestItemInfo(itemIdx);
                            title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemIndexTitle, linkName);
                            content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemInfoLoading);
                        }
                    }
                    else
                    {
                        title = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemIndexTitle, linkName);
                        content = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidLinkIndex);
                    }
                    break;
            }

            // Set image size BEFORE setting text and calculating panel size
            int imageWidth = 0;
            int imageHeight = 0;

            if (showImage && _tooltipImage.Library != null)
            {
                // Get actual size from library
                Size libSize = _tooltipImage.Library.GetSize(_tooltipImage.Index);
                if (!libSize.IsEmpty)
                {
                    imageWidth = libSize.Width;
                    imageHeight = libSize.Height;
                    _tooltipImage.Size = new Size(imageWidth, imageHeight);
                }
                else
                {
                    // Fallback to Items library if Items_Tooltip_32bit image not found
                    if (_tooltipImage.Library == Libraries.Items_Tooltip_32bit && linkType == "ITEM")
                    {
                        Size fallbackSize = Libraries.Items.GetSize(_tooltipImage.Index);
                        if (!fallbackSize.IsEmpty)
                        {
                            _tooltipImage.Library = Libraries.Items;
                            imageWidth = fallbackSize.Width;
                            imageHeight = fallbackSize.Height;
                            _tooltipImage.Size = new Size(imageWidth, imageHeight);
                        }
                    }
                }
            }

            // Now set text and position
            _tooltipTitle.Text = title;
            _tooltipContent.Text = content;

            if (showImage)
            {
                _tooltipTitle.Location = new Point(imageWidth + 20, 5);
                _tooltipContent.Location = new Point(imageWidth + 20, 27);
            }
            else
            {
                _tooltipTitle.Location = new Point(10, 5);
                _tooltipContent.Location = new Point(10, 27);
            }

            _tooltipImage.Visible = showImage;

            // Calculate panel size based on content
            int imageOffset = showImage ? imageWidth + 20 : 10;
            int contentHeight = _tooltipTitle.Size.Height + _tooltipContent.Size.Height + 30;

            // Panel height should be at least as tall as the image + padding at top and bottom
            int minPanelHeight = showImage ? imageHeight + 20 : contentHeight;
            int panelHeight = Math.Max(minPanelHeight, contentHeight);

            _tooltipPanel.Size = new Size(Math.Max(200, Math.Max(_tooltipTitle.Size.Width, _tooltipContent.Size.Width) + imageOffset + 20),
                                         panelHeight);

            _tooltipPanel.Location = new Point(anchor.X + 20, anchor.Y + 20);

            _tooltipPanel.BringToFront();
            _tooltipPanel.Sort = true;

            if (_tooltipPanel.Parent != null)
            {
                var parent = _tooltipPanel.Parent as MirControl;
                if (parent != null)
                {
                    parent.InsertControl(parent.Controls.Count, _tooltipPanel);
                }
            }

            _tooltipPanel.Visible = true;
        }

        private void HideTooltip()
        {
            if (_tooltipPanel != null)
                _tooltipPanel.Visible = false;
        }

        private string GetMonsterInfo(string monsterIdx)
        {
            if (int.TryParse(monsterIdx, out int idx))
            {
                var monster = GameScene.MonsterInfoList.FirstOrDefault(x => x.Index == idx);
                if (monster != null)
                {
                    return string.Join("\n", new[]
                    {
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipLevel, monster.Level),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipHealth, monster.Stats[Stat.HP]),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipExperience, monster.Experience),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipDC, monster.Stats[Stat.MinDC], monster.Stats[Stat.MaxDC]),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipMC, monster.Stats[Stat.MinMC], monster.Stats[Stat.MaxMC]),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipAC, monster.Stats[Stat.MinAC], monster.Stats[Stat.MaxAC]),
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterTooltipMAC, monster.Stats[Stat.MinMAC], monster.Stats[Stat.MaxMAC])
                    });
                }
                GameScene.RequestMonsterInfo(idx);
                return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MonsterIndexLoading, idx);
            }
            return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidMonsterIndex);
        }

        private string GetNPCInfo(string npcIdx)
        {
            if (int.TryParse(npcIdx, out int idx))
            {
                var npc = GameScene.NPCInfoList.FirstOrDefault(x => x.Index == idx);
                if (npc != null)
                {
                    string mapName = string.Empty;

                    // Try to get from current map if we're on that map
                    if (GameScene.Scene.MapControl != null && GameScene.Scene.MapControl.Index == npc.MapIndex)
                    {
                        mapName = GameScene.Scene.MapControl.Title;
                    }
                    // Otherwise try MapInfoList
                    else if (npc.MapIndex > 0 && GameScene.MapInfoList.TryGetValue(npc.MapIndex, out var mapInfo))
                    {
                        if (mapInfo?.MapInfo != null && !string.IsNullOrEmpty(mapInfo.MapInfo.Title))
                        {
                            mapName = mapInfo.MapInfo.Title;
                        }
                    }
                    // Otherwise no map info yet; we will request it below if needed

                    List<string> npcInfoLines = new List<string>
                    {
                        GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipName, npc.Name)
                    };

                    if (!string.IsNullOrWhiteSpace(mapName))
                    {
                        npcInfoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipLocation, mapName));
                    }
                    else if (npc.MapIndex > 0)
                    {
                        if (!PendingMapInfoRequests.Contains(npc.MapIndex) && !GameScene.MapInfoList.ContainsKey(npc.MapIndex))
                        {
                            PendingMapInfoRequests.Add(npc.MapIndex);
                            Network.Enqueue(new C.RequestMapInfo() { MapIndex = npc.MapIndex });
                        }

                        // Show a loading placeholder while we wait for the map title to arrive.
                        npcInfoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipLocation, GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipMapLoading)));
                    }
                    else
                    {
                        npcInfoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipLocation, GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipUnknownMap)));
                    }

                    npcInfoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCTooltipCoordinates, npc.Location.X, npc.Location.Y));

                    return string.Join("\n", npcInfoLines);
                }
                GameScene.RequestNPCInfo(idx);
                return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NPCIndexLoading, idx);
            }
            return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidNPCIndex);
        }

        private string GetItemInfo(string itemIdx)
        {
            if (int.TryParse(itemIdx, out int idx))
            {
                var item = GameScene.ItemInfoList.FirstOrDefault(x => x.Index == idx);

                if (item != null)
                {
                    List<string> infoLines = new List<string>();

                    string localizedType = item.Type.ToLocalizedString();
                    if (!string.IsNullOrEmpty(localizedType))
                    {
                        infoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTooltipType, localizedType));
                    }

                    if (item.RequiredType == RequiredType.Level && item.RequiredAmount > 0)
                    {
                        infoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.RequiredLevel, item.RequiredAmount));
                    }

                    if (item.ClassBased)
                    {
                        infoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ClassRequired, item.RequiredClass.ToLocalizedString()));
                    }

                    if (item.Weight > 0)
                    {
                        infoLines.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemTooltipWeight, item.Weight));
                    }

                    return string.Join("\n", infoLines);
                }

                GameScene.RequestItemInfo(idx);
                return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemIndexLoading, idx);
            }

            return GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InvalidItemIndex);
        }

        public static string GetDisplayNameForLink(string linkType, string linkIdx, string providedName = null)
        {
            if (!string.IsNullOrEmpty(providedName))
                return providedName;

            if (int.TryParse(linkIdx, out int idx))
            {
                switch (linkType)
                {
                    case "ITEM":
                        var item = GameScene.ItemInfoList.FirstOrDefault(x => x.Index == idx);
                        if (item != null)
                        return item.FriendlyName ?? item.Name ?? $"Item {idx}";

                        GameScene.RequestItemInfo(idx);
                        return $"Item {idx}";
                    case "MONSTER":
                        var monster = GameScene.MonsterInfoList.FirstOrDefault(x => x.Index == idx);
                        if (monster != null)
                        return string.IsNullOrEmpty(monster.GameName) ? monster.Name : monster.GameName;

                        GameScene.RequestMonsterInfo(idx);
                        return $"Monster {idx}";
                    case "NPC":
                        var npc = GameScene.NPCInfoList.FirstOrDefault(x => x.Index == idx);
                        if (npc != null)
                        return npc.Name;

                        GameScene.RequestNPCInfo(idx);
                        return $"NPC {idx}";
                    default:
                        return linkIdx;
                }
            }
            return linkIdx;
        }

        public static void ShowTooltipForLink(string linkType, string linkIdx, Point anchor)
        {
            if (GameScene.Scene.NPCDialog == null) return;
            GameScene.Scene.NPCDialog.ShowTooltip(linkType, linkIdx, anchor);
        }

        public static void HideTooltipForLink()
        {
            if (GameScene.Scene.NPCDialog == null) return;
            GameScene.Scene.NPCDialog.HideTooltip();
        }

        private static readonly TextFormatFlags LinkMeasureFlags = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;

        internal static Point CalculateLinkOffset(string text, MirLabel label)
        {
            if (label == null || string.IsNullOrEmpty(text))
                return Point.Empty;

            int maxWidth = Math.Max(1, label.Size.Width);
            int x = 0;
            int y = 0;

            foreach (char ch in text)
            {
                if (ch == '\r') continue;

                if (ch == '\n')
                {
                    x = 0;
                    y += label.Font.Height;
                    continue;
                }

                string character = ch.ToString();
                Size size = TextRenderer.MeasureText(CMain.Graphics, character, label.Font, new Size(int.MaxValue, int.MaxValue), LinkMeasureFlags);

                if (x + size.Width > maxWidth && x > 0)
                {
                    x = 0;
                    y += label.Font.Height;
                }

                x += size.Width;
            }

            return new Point(x, y);
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

        public override void Hide()
        {     
            Visible = false;
            GameScene.Scene.NPCGoodsDialog.Hide();
            GameScene.Scene.NPCSubGoodsDialog.Hide();
            GameScene.Scene.NPCCraftGoodsDialog.Hide();
            GameScene.Scene.NPCDropDialog.Hide();
            GameScene.Scene.NPCAwakeDialog.Hide();
            GameScene.Scene.RefineDialog.Hide();
            GameScene.Scene.StorageDialog.Hide();
            GameScene.Scene.TrustMerchantDialog.Hide();
            GameScene.Scene.QuestListDialog.Hide();
            GameScene.Scene.InventoryDialog.Location = new Point(0, 0);
            GameScene.Scene.RollControl.Hide();
            GameScene.Scene.GuildTerritoryDialog.Hide();
            BigButtonDialog.Hide();
            HideTooltip();
        }

        public override void Show()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, 0);
            Visible = true;

            CheckQuestButtonDisplay();
        }

    }
    public sealed class NPCGoodsDialog : MirImageControl
    {
        public PanelType PType;
        public bool UsePearls;

        public int StartIndex;
        public UserItem SelectedItem;

        public List<UserItem> Goods = new List<UserItem>();
        public List<UserItem> DisplayGoods = new List<UserItem>();
        public MirGoodsCell[] Cells;
        public MirButton BuyButton, CloseButton;
        public MirImageControl BuyLabel;

        public MirButton UpButton, DownButton, PositionBar;

        public NPCGoodsDialog(PanelType type)
        {
            PType = type;

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
                    Sound = SoundList.ButtonC
                };
                Cells[i].Click += (o, e) =>
                {
                    SelectedItem = ((MirGoodsCell)o).Item;
                    Update();

                    if (PType == PanelType.Craft)
                    {
                        GameScene.Scene.CraftDialog.ResetCells();
                        GameScene.Scene.CraftDialog.RefreshCraftCells(SelectedItem);

                        if (!GameScene.Scene.CraftDialog.Visible)
                        {
                            GameScene.Scene.CraftDialog.Show();
                        }
                    }
                };
                Cells[i].MouseWheel += NPCGoodsPanel_MouseWheel;
                Cells[i].DoubleClick += (o, e) =>
                {
                    if (PType == PanelType.Craft) return;

                    BuyItem();
                };
            }

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(217, 3),
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

            if (PType == PanelType.Craft)
            {
                BuyLabel.Index = 12;
                BuyButton.Visible = false;
            }

            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                Library = Libraries.Prguse2,
                Location = new Point(219, 35),
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
                Location = new Point(219, 284),
                Parent = this,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            DownButton.Click += (o, e) =>
            {
                if (DisplayGoods.Count <= 8) return;

                if (StartIndex == DisplayGoods.Count - 8) return;
                StartIndex++;
                Update();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(219, 49),
                Parent = this,
                PressedIndex = 206,
                Movable = true,
                Sound = SoundList.None
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
            PositionBar.MouseUp += (o, e) => Update();
        }

        private bool CheckSubGoods()
        {
            if (SelectedItem == null) return false;

            if (PType == PanelType.Buy && !UsePearls)
            {
                var list = Goods.Where(x => x.Info.Index == SelectedItem.Info.Index).ToList();

                if (list.Count > 1 || GameScene.Scene.NPCSubGoodsDialog.Visible)
                {
                    GameScene.Scene.NPCSubGoodsDialog.NewGoods(list);
                    GameScene.Scene.NPCSubGoodsDialog.Show();
                    return true;
                }
            }

            return false;
        }

        private void BuyItem()
        {
            if (SelectedItem == null) return;

            if (CheckSubGoods())
            {
                return;
            }

            if (SelectedItem.Info.StackSize > 1)
            {
                ushort tempCount = SelectedItem.Count;
                ushort maxQuantity = SelectedItem.Info.StackSize;

                SelectedItem.Count = maxQuantity;

                if (UsePearls)
                {
                    if (SelectedItem.Price() > GameScene.User.PearlCount)
                    {
                        maxQuantity = Math.Min(ushort.MaxValue, (ushort)(GameScene.Gold / (SelectedItem.Price() / SelectedItem.Count)));
                        if (maxQuantity == 0)
                        {
                            GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.NotEnoughPearls), ChatType.System);
                            return;
                        }
                    }
                }

                else if (SelectedItem.Price() > GameScene.Gold)
                {
                    maxQuantity = Math.Min(ushort.MaxValue, (ushort)(GameScene.Gold / (SelectedItem.Price() / SelectedItem.Count)));
                    if (maxQuantity == 0)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold), ChatType.System);
                        return;
                    }
                }

                MapObject.User.GetMaxGain(SelectedItem);

                if (SelectedItem.Count == 0)
                {
                    SelectedItem.Count = tempCount;
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouNoBagSpace), ChatType.System);
                    return;
                }

                if (SelectedItem.Count < maxQuantity)
                {
                    maxQuantity = SelectedItem.Count;
                }

                if (SelectedItem.Count > tempCount)
                {
                    SelectedItem.Count = tempCount;
                }

                MirAmountBox amountBox = new(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PurchaseAmount), SelectedItem.Image, maxQuantity, 0, SelectedItem.Count);

                amountBox.OKButton.Click += (o, e) =>
                {
                    if (amountBox.Amount > 0)
                    {
                        Network.Enqueue(new C.BuyItem { ItemIndex = SelectedItem.UniqueID, Count = (ushort)amountBox.Amount, Type = PanelType.Buy });
                    }
                };

                amountBox.Show();
            }
            else
            {
                if (SelectedItem.Info.Price > GameScene.Gold)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold), ChatType.System);
                    return;
                }

                for (int i = 0; i < MapObject.User.Inventory.Length; i++)
                {
                    if (MapObject.User.Inventory[i] == null) break;
                    if (i == MapObject.User.Inventory.Length - 1)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouNoBagSpace), ChatType.System);
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
            if (StartIndex == DisplayGoods.Count - 1 && count <= 0) return;

            StartIndex -= count;
            Update();
        }
        private void Update()
        {
            if (StartIndex > DisplayGoods.Count - 8) StartIndex = DisplayGoods.Count - 8;
            if (StartIndex <= 0) StartIndex = 0;

            if (DisplayGoods.Count > 8)
            {
                PositionBar.Visible = true;
                int h = 233 - PositionBar.Size.Height;
                h = (int)((h / (float)(DisplayGoods.Count - 8)) * StartIndex);
                PositionBar.Location = new Point(219, 49 + h);
            }
            else
                PositionBar.Visible = false;


            for (int i = 0; i < 8; i++)
            {
                if (i + StartIndex >= DisplayGoods.Count)
                {
                    Cells[i].Visible = false;
                    continue;
                }
                Cells[i].Visible = true;

                var matchingGoods = Goods.Where(x => x.Info.Index == Cells[i].Item.Info.Index);

                Cells[i].Item = DisplayGoods[i + StartIndex];
                Cells[i].MultipleAvailable = matchingGoods.Count() > 1 && matchingGoods.Any(x => x.IsShopItem == false);
                Cells[i].Border = SelectedItem != null && Cells[i].Item == SelectedItem;
                Cells[i].UsePearls = UsePearls;
            }
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            const int x = 219;
            int y = PositionBar.Location.Y;
            if (y >= 282 - PositionBar.Size.Height) y = 282 - PositionBar.Size.Height;
            if (y < 49) y = 49;

            int h = 233 - PositionBar.Size.Height;
            h = (int)Math.Round(((y - 49) / (h / (float)(DisplayGoods.Count - 8))));

            PositionBar.Location = new Point(x, y);

            if (h == StartIndex) return;
            StartIndex = h;
            Update();
        }

        public void NewGoods(IEnumerable<UserItem> list)
        {
            Goods.Clear();
            DisplayGoods.Clear();

            AddGoods(list);
        }

        public void AddGoods(IEnumerable<UserItem> list)
        {
            if (PType == PanelType.BuySub)
            {
                StartIndex = 0;
                SelectedItem = null;

                list = list.OrderBy(x => x.Price());
            }

            foreach (UserItem item in list)
            {
                //Normal shops just want to show one of each item type
                if (PType == PanelType.Buy && !UsePearls)
                {
                    Goods.Add(item);

                    if (DisplayGoods.Any(x => x.Info.Index == item.Info.Index)) continue;
                }

                DisplayGoods.Add(item);
            }

            if (GameScene.Scene.NPCSubGoodsDialog.Visible)
            {
                CheckSubGoods();
            }

            Update();
        }

        public override void Hide()
        {
            Visible = false;

            if (GameScene.Scene.CraftDialog.Visible)
            {
                GameScene.Scene.CraftDialog.Hide();
            }
        }

        public override void Show()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].Recipe = PType == PanelType.Craft;
            }

            Location = new Point(Location.X, GameScene.Scene.NPCDialog.Size.Height);
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
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotSellItem), ChatType.System);
                        return;
                    }
                    if (GameScene.Gold + TargetItem.Price() / 2 <= uint.MaxValue)
                    {
                        Network.Enqueue(new C.SellItem { UniqueID = TargetItem.UniqueID, Count = TargetItem.Count });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotCarryMoreGold), ChatType.System);
                    break;
                case PanelType.Repair:
                    if (TargetItem.Info.Bind.HasFlag(BindMode.DontRepair))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotRepairItem), ChatType.System);
                        return;
                    }
                    if (GameScene.Gold >= TargetItem.RepairPrice() * GameScene.NPCRate)
                    {
                        Network.Enqueue(new C.RepairItem { UniqueID = TargetItem.UniqueID });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold), ChatType.System);
                    break;
                case PanelType.SpecialRepair:
                    if ((TargetItem.Info.Bind.HasFlag(BindMode.DontRepair)) || (TargetItem.Info.Bind.HasFlag(BindMode.NoSRepair)))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotRepairItem), ChatType.System);
                        return;
                    }
                    if (GameScene.Gold >= (TargetItem.RepairPrice() * 3) * GameScene.NPCRate)
                    {
                        Network.Enqueue(new C.SRepairItem { UniqueID = TargetItem.UniqueID });
                        TargetItem = null;
                        return;
                    }
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LowGold), ChatType.System);
                    break;
                case PanelType.Consign:
                    if (TargetItem.Info.Bind.HasFlag(BindMode.DontStore) || TargetItem.Info.Bind.HasFlag(BindMode.DontSell))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CannotConsignItem), ChatType.System);
                        return;
                    }
                    MirAmountBox box = new MirAmountBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ConsignmentPrice), TargetItem.Image, Globals.MaxConsignment, Globals.MinConsignment)
                    {
                        InputTextBox = { Text = string.Empty },
                        Amount = 0
                    };

                    box.Show();
                    box.OKButton.Click += (o, e) =>
                    {
                        Network.Enqueue(new C.ConsignItem { UniqueID = TargetItem.UniqueID, Price = box.Amount, Type = MarketPanelType.Consign });
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
                            GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouDontHaveEnoughGoldToRefine), TargetItem.FriendlyName), ChatType.System);
                            return;
                        }

                    }
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.YouHaventDepositedItemsToRefine), TargetItem.FriendlyName), ChatType.System);
                    break;
                case PanelType.CheckRefine:

                    if (TargetItem.RefineAdded == 0)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ItemHasntBeenRefinedNoChecking), TargetItem.FriendlyName), ChatType.System);
                        return;
                    }
                    Network.Enqueue(new C.CheckRefine { UniqueID = TargetItem.UniqueID });
                    break;

                case PanelType.ReplaceWedRing:

                    if (TargetItem.Info.Type != ItemType.Ring)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.ItemIsNotRing), TargetItem.FriendlyName), ChatType.System);
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
                if (GameScene.SelectedCell.Item.Awake.GetAwakeLevel() != 0)
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
            Location = new Point(264, GameScene.Scene.NPCDialog.Size.Height);

            ConfirmButton.HoverIndex = 291;
            ConfirmButton.Index = 290;
            ConfirmButton.PressedIndex = 292;
            ConfirmButton.Location = new Point(114, 62);

            InfoLabel.Location = new Point(30, 10);

            ItemCell.Location = new Point(38, 72);

            switch (PType)
            {
                case PanelType.Sell:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Sale);
                    break;
                case PanelType.Repair:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Repair);
                    break;
                case PanelType.SpecialRepair:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SpecialRepair);
                    break;
                case PanelType.Consign:
                    InfoLabel.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Consignment);
                    return;
                case PanelType.Disassemble:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ItemWillBeDestroyed);
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
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Downgrade);
                    HoldButton.Visible = false;
                    break;
                case PanelType.Reset:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Reset);
                    HoldButton.Visible = false;
                    break;
                case PanelType.Refine:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Refine);
                    HoldButton.Visible = false;
                    ConfirmButton.Visible = true;
                    GameScene.Scene.RefineDialog.Show();
                    break;
                case PanelType.CheckRefine:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CheckRefine);
                    HoldButton.Visible = false;
                    ConfirmButton.Visible = true;
                    break;
                case PanelType.ReplaceWedRing:
                    text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ReplaceWedRing);
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

                text += GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Gold2);
            }

            InfoLabel.Text = text;
        }

        public override void Hide()
        {
            if (OldCell != null)
            {
                OldCell.Locked = false;
                TargetItem = null;
                OldCell = null;
            }
            Visible = false;
        }
        public override void Show()
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
                SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SelectUpgradeItem));
                SelectAwakeType.SelectedIndex = SelectAwakeType.Items.Count - 1;
                CurrentAwakeType = AwakeType.None;
            }
            else
            {
                if (Items[0].Awake.GetAwakeLevel() == 0)
                {
                    SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SelectUpgradeType));
                    if (Items[0].Info.Type == ItemType.Weapon)
                    {
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BraveryGlyph));
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MagicGlyph));
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SoulGlyph));
                    }
                    else if (Items[0].Info.Type == ItemType.Helmet)
                    {
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ProtectionGlyph));
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EvilSlayerGlyph));
                    }
                    else
                    {
                        SelectAwakeType.Items.Add(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BodyGlyph));
                    }
                }
                else
                {
                    SelectAwakeType.Items.Add(getAwakeTypeText(Items[0].Awake.Type));
                    if (CurrentAwakeType != Items[0].Awake.Type)
                    {
                        CurrentAwakeType = Items[0].Awake.Type;
                        OnAwakeTypeSelect(0);
                    }
                }
            }
        }

        public string getAwakeTypeText(AwakeType type)
        {
            string typeName = "";
            if (type == AwakeType.DC)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BraveryGlyph);
            }
            else if (type == AwakeType.MC)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MagicGlyph);
            }
            else if (type == AwakeType.SC)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SoulGlyph);
            }
            else if (type == AwakeType.AC)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ProtectionGlyph);
            }
            else if (type == AwakeType.MAC)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EvilSlayerGlyph);
            }
            else if (type == AwakeType.HPMP)
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BodyGlyph);
            }
            else
            {
                typeName = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SelectUpgradeItem);
            }

            return typeName;
        }

        public AwakeType getAwakeType(string typeName)
        {
            AwakeType type = AwakeType.None;
            if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BraveryGlyph))
            {
                type = AwakeType.DC;
            }
            else if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MagicGlyph))
            {
                type = AwakeType.MC;
            }
            else if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SoulGlyph))
            {
                type = AwakeType.SC;
            }
            else if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ProtectionGlyph))
            {
                type = AwakeType.AC;
            }
            else if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EvilSlayerGlyph))
            {
                type = AwakeType.MAC;
            }
            else if (typeName == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BodyGlyph))
            {
                type = AwakeType.HPMP;
            }
            else
            {
                type = AwakeType.None;
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
                NeedItemLabel1.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.NeedItemQuantity), Regex.Replace(ItemCells[1].Item.Info.Name, @"[\d-]", string.Empty), MaterialsCount[0].ToString());
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
                NeedItemLabel2.Text = GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.NeedItemQuantity), Regex.Replace(ItemCells[2].Item.Info.Name, @"[\d-]", string.Empty), MaterialsCount[1].ToString());
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

        public override void Hide()
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

        public override void Show()
        {
            Visible = true;

            //GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, 0);
            GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, Location.Y);
            GameScene.Scene.InventoryDialog.Show();
        }
    }
    public sealed class CraftDialog : MirImageControl
    {
        public UserItem RecipeItem;
        public ClientRecipeInfo Recipe;

        private const int _toolCount = 3;
        private const int _ingredientCount = 6;
        private static int _totalCount { get { return _toolCount + _ingredientCount; } }

        public static UserItem[] Slots = new UserItem[_totalCount];
        public static UserItem[] ShadowItems = new UserItem[_totalCount];

        public Dictionary<MirItemCell, ulong> Selected = new Dictionary<MirItemCell, ulong>();

        public MirItemCell[] Grid;

        public MirButton CraftButton, AutoFillButton, CloseButton;

        public MirLabel RecipeLabel;
        public MirLabel PossibilityLabel;
        public MirLabel GoldLabel;

        public CraftDialog()
        {
            Index = 1109;
            Library = Libraries.Prguse;
            Location = new Point(0, 0);
            Sort = true;
            BeforeDraw += CraftDialog_BeforeDraw;
            Movable = true;

            RecipeLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(22, 5),
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                ForeColour = Color.BurlyWood,
                Visible = true,
                NotControl = true
            };

            PossibilityLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Parent = this,
                Location = new Point(10, 135),
                Font = new Font(Settings.FontName, 8F),
                Visible = true,
                NotControl = true
            };

            GoldLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Parent = this,
                Location = new Point(30, 190),
                Font = new Font(Settings.FontName, 8F),
                Visible = true,
                NotControl = true
            };

            Grid = new MirItemCell[_totalCount];
            for (int x = 0; x < _totalCount; x++)
            {
                if (x >= _toolCount)
                {
                    Grid[x] = new MirItemCell
                    {
                        ItemSlot = x,
                        GridType = MirGridType.Craft,
                        Library = Libraries.Items,
                        Parent = this,
                        Size = new Size(35, 32),
                        Location = new Point(((x - _toolCount) * 40) + 52, 86),
                        Border = true,
                        BorderColour = Color.Lime
                    };
                }
                else
                {
                    Grid[x] = new MirItemCell
                    {
                        ItemSlot = x,
                        GridType = MirGridType.Craft,
                        Library = Libraries.Items,
                        Parent = this,
                        Size = new Size(35, 32),
                        Location = new Point((x * 44) + 108, 44),
                        Border = true,
                        BorderColour = Color.Lime
                    };
                }

                Grid[x].Click += Grid_Click;
            }

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(312, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            AutoFillButton = new MirButton
            {
                HoverIndex = 181,
                Index = 180,
                Location = new Point(165, 185),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 182,
                Sound = SoundList.ButtonA
            };
            AutoFillButton.Click += (o, e) => AutoFill();

            CraftButton = new MirButton
            {
                HoverIndex = 337,
                Index = 336,
                Location = new Point(215, 185),
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

            if (cell.ItemSlot >= _toolCount)
            {
                if (GameScene.SelectedCell.Item.Count < cell.ShadowItem.Count)
                    return;

                if (cell.ShadowItem.CurrentDura < cell.ShadowItem.MaxDura && GameScene.SelectedCell.Item.CurrentDura < cell.ShadowItem.CurrentDura)
                    return;
            }
            else
            {
                if (GameScene.SelectedCell.Item.CurrentDura < 1000M)
                    return;
            }

            cell.Item = GameScene.SelectedCell.Item;

            Selected.Add(GameScene.SelectedCell, GameScene.SelectedCell.Item.UniqueID);
            GameScene.SelectedCell.Locked = true;
            GameScene.SelectedCell = null;

            RefreshCraftCells(RecipeItem);
        }

        public override void Hide()
        {
            if (!Visible) return;

            Visible = false;

            ResetCells();
        }

        public override void Show()
        {
            Visible = true;

            Location = new Point(GameScene.Scene.InventoryDialog.Location.X - 12, GameScene.Scene.InventoryDialog.Location.Y + 236);
        }

        private void AutoFill()
        {
            ResetCells(false);

            if (RecipeItem == null) return;

            List<int> usedSlots = new List<int>();

            int j = 0;
            foreach (var tool in Recipe.Tools)
            {
                for (int i = 0; i < UserObject.User.Inventory.Length; i++)
                {
                    if (usedSlots.Contains(i)) continue;

                    var slot = UserObject.User.Inventory[i];

                    if (slot == null || tool.Info.Index != slot.Info.Index || slot.CurrentDura < 1000M) continue;

                    var cell = GameScene.Scene.InventoryDialog.GetCell(slot.UniqueID) ?? GameScene.Scene.BeltDialog.GetCell(slot.UniqueID);

                    if (cell.Locked) continue;

                    Selected.Add(cell, cell.Item.UniqueID);
                    cell.Locked = true;

                    Grid[j].Item = cell.Item;
                    break;
                }

                j++;
            }

            j = 3;
            foreach (var ingredient in Recipe.Ingredients)
            {
                for (int i = 0; i < UserObject.User.Inventory.Length; i++)
                {
                    if (usedSlots.Contains(i)) continue;

                    var slot = UserObject.User.Inventory[i];

                    if (slot == null || ingredient.Info.Index != slot.Info.Index) continue;
                    if (slot.Count < ingredient.Count) continue;
                    if (ingredient.CurrentDura < ingredient.MaxDura && slot.CurrentDura < ingredient.CurrentDura) continue;

                    var cell = GameScene.Scene.InventoryDialog.GetCell(slot.UniqueID) ?? GameScene.Scene.BeltDialog.GetCell(slot.UniqueID);

                    if (cell.Locked) continue;

                    Selected.Add(cell, cell.Item.UniqueID);
                    cell.Locked = true;

                    Grid[j].Item = cell.Item;
                    break;
                }

                j++;
            }

            RefreshCraftCells(RecipeItem);
        }

        private void CraftItem()
        {
            if (RecipeItem == null) return;

            if (Selected.Count < Recipe.Tools.Count + Recipe.Ingredients.Count) return;

            ushort max = 99;

            //Max quantity based on available ingredients/tools
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i] == null || Grid[i].Item == null) continue;

                ushort temp = 0;
                if (i >= _toolCount)
                {
                    temp = (ushort)(Grid[i].Item.Count / Grid[i].ShadowItem.Count);
                }
                else
                {
                    temp = (ushort)Math.Floor(Grid[i].Item.CurrentDura / 1000M);
                }

                if (temp < max) max = temp;
            }

            if (max > (RecipeItem.Info.StackSize / RecipeItem.Count))
            {
                max = (ushort)(RecipeItem.Info.StackSize / RecipeItem.Count);
            }

            //TODO - Check Max slots spare against slots to be used (stacksize/quantity)
            //TODO - GetMaxItemGain

            if (max == 1)
            {
                if (Recipe.Gold > GameScene.Gold)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouDoNotHaveEnoughGold), ChatType.System);
                    return;
                }

            }

            if (max > 1)
            {
                MirAmountBox amountBox = new MirAmountBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CraftAmount), RecipeItem.Info.Image, max, 0, max);

                amountBox.OKButton.Click += (o, e) =>
                {
                    if (amountBox.Amount > 0)
                    {
                        if (!HasCraftItems((ushort)amountBox.Amount))
                        {
                            GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouDoNotHaveRequiredToolsOrIngredients), ChatType.System);
                            return;
                        }
                        
                        if ((Recipe.Gold * amountBox.Amount) > GameScene.Gold)
                        {
                            GameScene.Scene.ChatDialog.ReceiveChat(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.YouDoNotHaveEnoughGold), ChatType.System);
                            return;
                        }

                        Network.Enqueue(new C.CraftItem
                        {
                            UniqueID = RecipeItem.UniqueID,
                            Count = (ushort)amountBox.Amount,
                            Slots = Selected.Select(x => x.Key.ItemSlot).ToArray()
                        });
                    }
                };

                amountBox.Show();
            }
            else
            {
                Network.Enqueue(new C.CraftItem
                {
                    UniqueID = RecipeItem.UniqueID,
                    Count = 1,
                    Slots = Selected.Select(x => x.Key.ItemSlot).ToArray()
                });
            }
        }

        private bool HasCraftItems(ushort count)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].ShadowItem == null) continue;

                if (i >= _toolCount)
                {
                    if (Grid[i].Item == null || Grid[i].Item.Count < (Grid[i].ShadowItem.Count * count)) return false;
                }
                else
                {
                    if (Grid[i].Item == null || (uint)Math.Floor(Grid[i].Item.CurrentDura / 1000M) < count) return false;
                }
            }

            return true;
        }

        public void ResetCells(bool clearRecipe = true)
        {
            if (clearRecipe)
            {
                RecipeItem = null;
            }

            for (int j = 0; j < Grid.Length; j++)
            {
                Slots[j] = null;
                ShadowItems[j] = null;
            }

            foreach (var key in Selected.Keys)
            {
                key.Locked = false;
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

        public void UpdateCraftCells()
        {
            List<MirItemCell> invalidCells = new List<MirItemCell>();

            foreach (var key in Selected.Keys)
            {
                MirItemCell cell = key;
                ulong oldItem = Selected[key];

                if (cell.Item == null || cell.Item.UniqueID != oldItem || (cell.Item.MaxDura > 1000M && cell.Item.CurrentDura < 1000M))
                {
                    MirItemCell gridCell = GetCell(oldItem);

                    if (gridCell != null)
                    {
                        gridCell.Item = null;
                    }
                    cell.Locked = false;

                    invalidCells.Add(key);
                }
            }

            foreach (var cell in invalidCells)
            {
                Selected.Remove(cell);
            }

            RefreshCraftCells(RecipeItem);
        }

        public void RefreshCraftCells(UserItem selectedItem)
        {
            RecipeItem = selectedItem;

            CraftButton.Enabled = true;
            CraftButton.GrayScale = false;

            Recipe = GameScene.RecipeInfoList.SingleOrDefault(x => x.Item.ItemIndex == selectedItem.ItemIndex);

            RecipeLabel.Text = Recipe.Item.FriendlyName;
            PossibilityLabel.Text = (UserObject.User.Stats[Stat.CraftRatePercent] > 0 ? $"{Math.Min(100, Recipe.Chance + UserObject.User.Stats[Stat.CraftRatePercent])}% (+{UserObject.User.Stats[Stat.CraftRatePercent]}%)" : $"{Recipe.Chance}%") + " Chance of Success";
            GoldLabel.Text = Recipe.Gold.ToString("###,###,##0");

            for (int i = 0; i < Slots.Length; i++)
            {
                bool need;

                if (i >= _toolCount)
                {
                    if ((i - _toolCount) >= Recipe.Ingredients.Count) continue;

                    ShadowItems[i] = Recipe.Ingredients[i - _toolCount];
                    need = Grid[i].Item == null || Grid[i].Item.Count < Grid[i].ShadowItem.Count;
                }
                else
                {
                    if (i >= Recipe.Tools.Count) continue;

                    ShadowItems[i] = Recipe.Tools[i];
                    need = Grid[i].Item == null || Grid[i].Item.Count < Grid[i].ShadowItem.Count;
                }

                if (need)
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

        public override void Hide()
        {
            if (!Visible) return;

            Visible = false;
            RefineCancel();
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
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExtendYourRentalPeriod), MirMessageBoxButtons.OKCancel);
                else
                    messageBox = new MirMessageBox(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExtraStorage), MirMessageBoxButtons.OKCancel);

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
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExpandedStorageLocked),
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

                    if (idx >= Globals.StorageGridSize)
                        Grid[idx].Visible = false;
                }
            }
        }

        public override void Show()
        {
            GameScene.Scene.InventoryDialog.Location = new Point(Size.Width + 5, Location.Y);
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
                if (grid.ItemSlot < Globals.StorageGridSize)
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
                RentalLabel.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExpandedStorageExpiresOn) + GameScene.User.ExpandedStorageExpiryTime.ToString();
                RentalLabel.ForeColour = Color.White;
            }
            else
            {
                RentalLabel.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExpandedStorageLocked);
                RentalLabel.ForeColour = Color.Red;
                RentButton.Visible = true;
                LockedPage.Visible = true;
            }

            foreach (var grid in Grid)
            {
                if (grid.ItemSlot < Globals.StorageGridSize || !GameScene.User.HasExpandedStorage)
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
    public sealed class BigButtonDialog : MirImageControl
    {
        const int MaximumRows = 8;
        private List<BigButton> CurrentButtons;
        private int ScrollOffset = 0;
        public BigButtonDialog()
        {
            Visible = false;
        }

        public void Show(List<BigButton> buttons, int minimumButtons)
        {
            if (Visible) return;
            CurrentButtons = buttons;

            for (int i = 0; i < Controls.Count; i++)
                Controls[i].Dispose();
            Controls.Clear();
            Size = Size.Empty;
            ScrollOffset = 0;

            CurrentButtons.ToList().ForEach(b => b.MouseWheel += (o, e) => BigButtonDialog_MouseWheel(o, e));
            int count = Math.Max(minimumButtons, buttons.Count);
            for (int i = 0; i < Math.Min(count, MaximumRows); i++)
            {
                MirImageControl background = new MirImageControl()
                {
                    Parent = this,
                    Library = Libraries.Title,
                    Location = new Point(buttons.Count == 1 ? -1 : 0, Size.Height),
                    Index = count == 1 ? 836 : (i == 0 ? 838 : (i == count - 1 ? 840 : 839)),
                    NotControl = false,
                    Visible = true,
                };
                background.MouseWheel += (o, e) => BigButtonDialog_MouseWheel(o, e);
                Size = new Size(background.Size.Width, Size.Height + background.Size.Height);
            }

            RefreshButtons();

            MirImageControl footer = new MirImageControl()
            {
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(-1, Size.Height),
                Index = 837,
                NotControl = false,
                Visible = true,
            };
            Size = new Size(Size.Width, Size.Height + footer.Size.Height);

            if (buttons.Count > MaximumRows)
            {
                MirButton upButton = new MirButton
                {
                    Index = 197,
                    HoverIndex = 198,
                    PressedIndex = 199,
                    Library = Libraries.Prguse2,
                    Parent = this,
                    Size = new Size(16, 14),
                    Sound = SoundList.ButtonA,
                    Location = new Point(Size.Width - 26, 17)
                };
                upButton.Click += (o, e) =>
                {
                    ScrollUp();
                };

                MirButton downButton = new MirButton
                {
                    Index = 207,
                    HoverIndex = 208,
                    Library = Libraries.Prguse2,
                    PressedIndex = 209,
                    Parent = this,
                    Size = new Size(16, 14),
                    Sound = SoundList.ButtonA,
                    Location = new Point(Size.Width - 26, Size.Height - 57)
                };
                downButton.Click += (o, e) =>
                {
                    ScrollDown();
                };
            }

            Visible = true;
        }

        public override void Hide()
        {
            Size = Size.Empty;
            Visible = false;
        }

        private void RefreshButtons()
        {
            CurrentButtons.ToList().ForEach(b => b.Visible = false);

            for (int i = 0; i < Math.Min(CurrentButtons.Count, MaximumRows); i++)
            {
                CurrentButtons[i + ScrollOffset].Parent = this;
                CurrentButtons[i + ScrollOffset].Visible = true;
                CurrentButtons[i + ScrollOffset].Location = new Point(97, 7 + i * 40);
            }            
        }

        private void BigButtonDialog_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (count > 0)
                ScrollUp();
            else if (count < 0)
                ScrollDown();
        }

        private void ScrollUp()
        {
            if (ScrollOffset <= 0) return;

            ScrollOffset--;
            RefreshButtons();
        }

        private void ScrollDown()
        {
            if (ScrollOffset + MaximumRows >= CurrentButtons.Count) return;

            ScrollOffset++;
            RefreshButtons();
        }
    }
    public sealed class BigButton : MirButton
    {
        #region Label
        private MirLabel _shadowLabel;
        #endregion

        #region CenterText
        public override bool CenterText
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
                if (_center)
                {
                    _label.Size = Size;
                    _label.DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    _shadowLabel.Size = Size;
                    _shadowLabel.DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                }
                else
                {
                    _label.AutoSize = true;
                    _shadowLabel.AutoSize = true;
                }
            }
        }
        #endregion

        #region Font Colour
        public override Color FontColour
        {
            get
            {
                if (_label != null && !_label.IsDisposed)
                    return _label.ForeColour;
                return Color.Empty;
            }
            set
            {
                if (_label != null && !_label.IsDisposed)
                    _label.ForeColour = value;
            }
        }
        #endregion

        #region Size
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            if (_shadowLabel != null && !_shadowLabel.IsDisposed)
                _shadowLabel.Size = Size;
        }
        #endregion

        #region Text
        public override string Text
        {
            set
            {
                if (_label != null && !_label.IsDisposed)
                {
                    _label.Text = value;
                    _label.Visible = !string.IsNullOrEmpty(value);
                }

                if (_shadowLabel != null && !_shadowLabel.IsDisposed)
                {
                    _shadowLabel.Text = value;
                    _shadowLabel.Visible = !string.IsNullOrEmpty(value);
                }
            }
        }
        #endregion
        public BigButton()
        {
            HoverIndex = -1;
            PressedIndex = -1;
            DisabledIndex = -1;
            Sound = SoundList.ButtonB;

            _shadowLabel = new MirLabel
            {
                NotControl = true,
                Parent = this,
                Location = new Point(2, 7),
                AutoSize = false,
                Size = new Size(237, 20),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                ForeColour = Color.Black,
                Font = ScaleFont(new Font(Settings.FontName, 12F, FontStyle.Bold))
            };

            _label = new MirLabel
            {
                NotControl = true,
                Parent = this,
                Location = new Point(0, 5),
                AutoSize = false,
                Size = new Size(237, 20),
                DrawFormat = TextFormatFlags.HorizontalCenter,                
                Font = ScaleFont(new Font(Settings.FontName, 12F, FontStyle.Bold))
            };
        }

        protected internal override void DrawControl()
        {
            base.DrawControl();

            if (DrawImage && Library != null)
            {
                bool oldGray = DXManager.GrayScale;

                if (GrayScale)
                {
                    DXManager.SetGrayscale(true);
                }

                if (Blending)
                    Library.DrawBlend(Index + 3, DisplayLocation, Color.White, false, BlendingRate);
                else
                    Library.Draw(Index + 3, DisplayLocation, Color.White, false, Opacity);

                if (GrayScale) DXManager.SetGrayscale(oldGray);
            }
        }

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            if (_shadowLabel != null && !_shadowLabel.IsDisposed)
                _shadowLabel.Dispose();
            _shadowLabel = null;
        }
        #endregion
    }
}

using Client.MirControls;
using Client.MirGraphics;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Client.MirSounds;
using Client.MirNetwork;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{

    public class GTRow : MirImageControl
    {
        public MirLabel Idx;
        public MirLabel GuildOwner;
        public string Owner1, Owner2;
        public MirLabel OwnerName;
        public MirLabel Status;
        public MirLabel Price;

        public GTRow()
        {
            Size = new Size(550, 17);

            Idx = new MirLabel
            {
                Location = new Point(15, 0),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.White,
                NotControl = true,
                BackColour = Color.Transparent,
            };

            GuildOwner = new MirLabel
            {
                Location = new Point(45, 0),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.White,
                NotControl = true,
                BackColour = Color.Transparent,
            };

            OwnerName = new MirLabel
            {
                Location = new Point(150, 0),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.White,
                NotControl = true,
                BackColour = Color.Transparent,
            };

            Status = new MirLabel
            {
                Location = new Point(365, 0),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.White,
                NotControl = true,
                BackColour = Color.Transparent,
            };

            Price = new MirLabel
            {
                Location = new Point(460, 0),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.White,
                NotControl = true,
                BackColour = Color.Transparent,
            };
        }
    }

    public sealed class GuildTerritoryDialog : MirImageControl
    {

        public List<ClientGTMap> GTMapList = new List<ClientGTMap>();
        public List<GTRow> GTRowList = new List<GTRow>();
        public int Page = 0, Lenght = 0;
        public MirButton BuyButton;

        public int selectedIndex = -1;
        public GuildTerritoryDialog()
        {
            Index = 680;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;

            #region Buttons.

            var closeButton = new MirButton
            {
                Parent = this,
                Index = 361,
                PressedIndex = 363,
                HoverIndex = 362,
                Library = Libraries.Prguse,
                Location = new Point(544, 8),
                Hint = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExitButton)
            };
            closeButton.Click += (o, e) => Hide();

            var prevButton = new MirButton
            {
                HoverIndex = 241,
                Index = 240,
                Location = new Point(214, 213),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 242,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PageBack)
            };
            prevButton.Click += (o, e) =>
            {
                if ((Page - 1) >= 0)
                {
                    Page--;
                    Network.Enqueue(new C.GuildTerritoryPage { Page = Page });
                    Reset();
                }
            };

            var nextButton = new MirButton
            {
                HoverIndex = 244,
                Index = 243,
                Location = new Point(317, 213),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 245,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PageForward)
            };
            nextButton.Click += (o, e) =>
            {
                if (((Page + 1) * 7) < Lenght)
                {
                    Page++;
                    Network.Enqueue(new C.GuildTerritoryPage { Page = Page });
                    Reset();
                }
            };


            var mailButton = new MirButton
            {
                Index = 437,
                HoverIndex = 438,
                PressedIndex = 439,
                Library = Libraries.Prguse,
                Location = new Point(262, 208),
                Sound = SoundList.ButtonA,
                Parent = this,
                Hint = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MailGuildLeader)
            };
            mailButton.Click += (o, e) =>
            {
                var GT = GTRowList.FirstOrDefault(x => x.Idx.Text == selectedIndex.ToString());

                if (GT == null || GT.OwnerName.Text == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.None)) return;

                GameScene.Scene.MailComposeLetterDialog.ComposeMail(GT.Owner1);
            };

            BuyButton = new MirButton
            {
                Index = 437,
                HoverIndex = 438,
                PressedIndex = 439,
                Library = Libraries.Prguse,
                Location = new Point(292, 208),
                Sound = SoundList.ButtonA,
                Parent = this,
                Hint = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Purchase),
                Visible = false
            };
            BuyButton.Click += (o, e) =>
            {
                var GT = GTRowList.FirstOrDefault(x => x.Idx.Text == selectedIndex.ToString());

                if (GT == null || GT.OwnerName.Text == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.None)) return;
                if (GT.Status.Text != GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ForSale)) return;

                Network.Enqueue(new C.PurchaseGuildTerritory { Owner = GT.GuildOwner.Text });
            };

            #endregion

            #region Labels.

            var _titleLabel = new MirImageControl
            {
                Index = 54,
                Library = Libraries.Title,
                Location = new Point(217, 11),
                Parent = this
            };

            var __gtnumberLabel = new MirLabel
            {
                Location = new Point(15, 38),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.Goldenrod,
                Font = new Font(Settings.FontName, 8F),
                NotControl = true,
                BackColour = Color.Transparent,
                Text = "GT #"
            };

            var __gtguildLabel = new MirLabel
            {
                Location = new Point(60, 38),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.Goldenrod,
                Font = new Font(Settings.FontName, 8F),
                NotControl = true,
                BackColour = Color.Transparent,
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.OwningGuild)
            };

            var __gtguildownersLabel = new MirLabel
            {
                Location = new Point(230, 38),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.Goldenrod,
                Font = new Font(Settings.FontName, 8F),
                NotControl = true,
                BackColour = Color.Transparent,
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildLeaders)
            };

            var __gtstatusLabel = new MirLabel
            {
                Location = new Point(380, 38),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.Goldenrod,
                Font = new Font(Settings.FontName, 8F),
                NotControl = true,
                BackColour = Color.Transparent,
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GTStatus)
            };

            var __gtpriceLabel = new MirLabel
            {
                Location = new Point(480, 38),
                AutoSize = true,
                Parent = this,
                ForeColour = Color.Goldenrod,
                Font = new Font(Settings.FontName, 8F),
                NotControl = true,
                BackColour = Color.Transparent,
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GTPrice)
            };

            for (int i = 0; i < 7; i++)
            {
                GTRow gt = new GTRow
                {
                    Parent = this,
                    Location = new Point(5, 60 + 20 * i),
                    Border = false,
                    BorderColour = Color.Lime,
                };
                gt.Idx.Text = (i + 1).ToString();
                gt.GuildOwner.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.None);
                gt.OwnerName.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.None);
                gt.Status.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Available);
                gt.Price.Text = "10,000,000";

                gt.Click += (e, o) =>
                {
                    var obj = ((GTRow)e);

                    Reset();

                    if (obj.GuildOwner.Text != string.Empty)
                    {
                        int.TryParse(obj.Idx.Text, out selectedIndex);
                        obj.Border = true;
                        BuyButton.Visible = obj.Status.Text == GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ForSale);
                    }

                };

                GTRowList.Add(gt);
            }

            #endregion
        }

        public void Reset()
        {
            foreach (var r in GameScene.Scene.GuildTerritoryDialog.GTRowList)
            {
                r.Border = false;
            }
            selectedIndex = -1;
        }

        public override void Show()
        {
            Visible = true;
        }

        public override void Hide()
        {
            Visible = false;
        }

        public void UpdateInterface()
        {
            for (int i = 0; i < GTRowList.Count; i++)
            {
                if (GTMapList.Count > i)
                {
                    var gtMap = GTMapList[i];
                    var gtRow = GTRowList[i];

                    gtRow.Idx.Text = ((i + 1) + Page * 7).ToString();
                    gtRow.GuildOwner.Text = gtMap.Owner;
                    gtRow.Owner1 = gtMap.Leader;
                    gtRow.Owner2 = gtMap.Leader2;
                    gtRow.OwnerName.Text = gtMap.Leader;
                    if (gtMap.Leader2 != string.Empty)
                        gtRow.OwnerName.Text += GameLanguage.ClientTextMap.GetLocalization((ClientTextKeys.AndPlaceholder), gtMap.Leader2);

                    if (gtMap.Owner != GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.None))
                        if (gtMap.begin > 0)
                            gtRow.Status.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SalePending);
                    else
                        gtRow.Status.Text = gtMap.price > 0 ? GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ForSale) : GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Unavailable);
                    else
                        gtRow.Status.Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Available);

                    gtRow.Price.Text = gtMap.price.ToString("###,###,##0");
                }
                else
                {
                    var gtRow = GTRowList[i];

                    gtRow.Idx.Text = string.Empty;
                    gtRow.GuildOwner.Text = string.Empty;
                    gtRow.OwnerName.Text = string.Empty;
                    gtRow.Status.Text = string.Empty;
                    gtRow.Price.Text = string.Empty;
                }

            }
        }
    }
}

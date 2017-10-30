using System;
using System.Drawing;
using Client.MirGraphics;
using Client.MirScenes;
using Microsoft.DirectX;
using System.Text.RegularExpressions;

namespace Client.MirControls
{
    public sealed class MirGoodsCell : MirControl
    {
        //public ItemInfo Item;
        public UserItem Item;
        public MirLabel NameLabel, PriceLabel, CountLabel;
        public bool usePearls = false;//pearl currency
        public bool Recipe = false;

        public MirGoodsCell()
        {
            Size = new Size(205, 32);
            BorderColour = Color.Lime;

            NameLabel = new MirLabel
                {
                    AutoSize = true,
                    Parent = this,
                    NotControl = true,
                    Location = new Point(44, 0),
                };

            CountLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                NotControl = true,
                DrawControlTexture = true,
                Location = new Point(23, 17),
                ForeColour = Color.Yellow,
            };

            PriceLabel = new MirLabel
                {
                    AutoSize = true,
                    Parent = this,
                    NotControl = true,
                    Location = new Point(44, 14),
                };

            BeforeDraw += (o, e) => Update();
            AfterDraw += (o, e) => DrawItem();
        }

        private void Update()
        {
            if (Item == null || Item.Info == null) return;
            NameLabel.Text = Item.Info.FriendlyName;
            CountLabel.Text = (Item.Count <= 1) ? "" : Item.Count.ToString();

            if (usePearls)//pearl currency
            {
                if (Item.Price() > 1)
                    PriceLabel.Text = string.Format("Price: {0} pearls", (uint)(Item.Price() * GameScene.NPCRate));
                else
                    PriceLabel.Text = string.Format("Price: {0} pearl", (uint)(Item.Price() * GameScene.NPCRate));
            }
            else
            {
                if (Recipe)
                {
                    PriceLabel.Text = string.Format("Craftable Item");
                }
                else
                {
                    PriceLabel.Text = string.Format("Price: {0} gold", (uint)(Item.Price() * GameScene.NPCRate));
                }
            }
        }

        protected override Vector2[] BorderInfo
        {
            get
            {
                if (Size == Size.Empty) return null;

                if (BorderRectangle != DisplayRectangle)
                {

                    _borderInfo = new[]
                        {
                            new Vector2(DisplayRectangle.Left - 1, DisplayRectangle.Top - 1),
                            new Vector2(DisplayRectangle.Right, DisplayRectangle.Top - 1),

                            new Vector2(DisplayRectangle.Left - 1, DisplayRectangle.Top - 1),
                            new Vector2(DisplayRectangle.Left - 1, DisplayRectangle.Bottom),

                            new Vector2(DisplayRectangle.Left - 1, DisplayRectangle.Bottom),
                            new Vector2(DisplayRectangle.Right, DisplayRectangle.Bottom),

                            new Vector2(DisplayRectangle.Right, DisplayRectangle.Top - 1),
                            new Vector2(DisplayRectangle.Right, DisplayRectangle.Bottom),

                            new Vector2(DisplayRectangle.Left + 40, DisplayRectangle.Bottom),
                            new Vector2(DisplayRectangle.Left + 40, DisplayRectangle.Top - 1)
                        };

                    BorderRectangle = DisplayRectangle;
                }
                return _borderInfo;
            }
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();

            //if (ShowItem == null) ShowItem = new UserItem(Item) {MaxDura = Item.Durability, CurrentDura = Item.Durability};

            GameScene.Scene.CreateItemLabel(Item);
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeItemLabel();
            GameScene.HoverItem = null;
            //ShowItem = null;
        }

        private void DrawItem()
        {
            if (Item == null || Item.Info == null) return;

            Size size = Libraries.Items.GetTrueSize(Item.Image);
            Point offSet = new Point((40 - size.Width)/2, (32 - size.Height)/2);
            Libraries.Items.Draw(Item.Image, offSet.X + DisplayLocation.X, offSet.Y + DisplayLocation.Y);

            CountLabel.Draw();
        }
    }
}

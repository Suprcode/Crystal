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
        public ItemInfo Item;
        public UserItem ShowItem;
        public MirLabel NameLabel, PriceLabel;

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
            if (Item == null) return;
            NameLabel.Text = Item.FriendlyName;
            PriceLabel.Text = string.Format("Price: {0} gold", Item.Price*GameScene.NPCRate);
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

            if (ShowItem == null) ShowItem = new UserItem(Item) {MaxDura = Item.Durability, CurrentDura = Item.Durability};

            GameScene.Scene.CreateItemLabel(ShowItem);
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeItemLabel();
            GameScene.HoverItem = null;
            ShowItem = null;
        }

        private void DrawItem()
        {
            if (Item == null) return;

            Size size = Libraries.Items.GetTrueSize(Item.Image);
            Point offSet = new Point((40 - size.Width)/2, (32 - size.Height)/2);
            Libraries.Items.Draw(Item.Image, offSet.X + DisplayLocation.X, offSet.Y + DisplayLocation.Y);
        }
    }
}

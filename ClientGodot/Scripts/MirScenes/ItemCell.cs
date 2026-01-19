using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class ItemCell : Panel
    {
        private TextureRect _icon;
        private Label _countLabel;

        public UserItem Item;

        public override void _Ready()
        {
            _icon = GetNode<TextureRect>("Icon");
            _countLabel = GetNode<Label>("Count");
        }

        public void UpdateItem(UserItem item)
        {
            Item = item;
            if (item == null)
            {
                _icon.Texture = null;
                _countLabel.Text = "";
                return;
            }

            // Load Icon from Libraries.Items
            if (ClientGodot.Scripts.MirGraphics.Libraries.Items != null)
            {
                var img = ClientGodot.Scripts.MirGraphics.Libraries.Items.GetImage(item.Image);
                if (img != null)
                {
                    _icon.Texture = img.CreateTexture();
                }
            }

            if (item.Count > 1)
                _countLabel.Text = item.Count.ToString();
            else
                _countLabel.Text = "";
        }
    }
}

using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class MagicCell : Panel
    {
        private TextureRect _icon;
        private Label _nameLabel;
        private Label _keyLabel;

        public ClientMagic Magic;

        public override void _Ready()
        {
            _icon = GetNode<TextureRect>("Icon");
            _nameLabel = GetNode<Label>("Name");
            _keyLabel = GetNode<Label>("Key");
        }

        public void UpdateMagic(ClientMagic magic)
        {
            Magic = magic;
            if (magic == null)
            {
                Visible = false;
                return;
            }
            Visible = true;
            _nameLabel.Text = magic.Name + $" (Lv{magic.Level})";
            _keyLabel.Text = magic.Key > 0 ? "F" + magic.Key : "";

            // Icon
            // MagIcon.lib
            // Standard: Base offset 0?
            // Usually MagIcon has (Icon) + (Inactive Icon)?
            // Assuming Icon index = magic.Icon * 2? Or just magic.Icon.
            // Let's assume direct index.

            if (ClientGodot.Scripts.MirGraphics.Libraries.MagIcon != null)
            {
                var img = ClientGodot.Scripts.MirGraphics.Libraries.MagIcon.GetImage(magic.Icon); // Or magic.Icon * 2
                if (img != null)
                {
                    _icon.Texture = img.CreateTexture();
                }
            }
        }
    }
}

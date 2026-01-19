using Godot;
using ClientGodot.Scripts.MirGraphics;

namespace ClientGodot.Scripts.MirControls
{
    public partial class MirButton : TextureButton
    {
        [Export]
        public int ImageIndex = -1;

        [Export]
        public bool UsePressedImage = true;

        public override void _Ready()
        {
            if (ImageIndex >= 0)
            {
                // Standard Mir Button Layout:
                // Index: Normal
                // Index+1: Hover
                // Index+2: Pressed

                TextureNormal = Libraries.GetUITexture(ImageIndex);
                TextureHover = Libraries.GetUITexture(ImageIndex + 1);
                if (UsePressedImage)
                    TexturePressed = Libraries.GetUITexture(ImageIndex + 2);
            }
        }
    }
}

using Godot;
using ClientPackets;
using ServerPackets;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class HUD : CanvasLayer
    {
        public static HUD Instance;

        private TextureProgressBar _hpBar;
        private TextureProgressBar _mpBar;
        private Label _coordLabel;

        // Chat
        private RichTextLabel _chatOutput;
        private LineEdit _chatInput;

        // MiniMap
        private MiniMap _miniMap;

        public override void _Ready()
        {
            Instance = this;

            // Find Nodes (Assuming structure)
            _hpBar = GetNode<TextureProgressBar>("Control/Bars/HPBar");
            _mpBar = GetNode<TextureProgressBar>("Control/Bars/MPBar");
            _coordLabel = GetNode<Label>("Control/Bars/CoordLabel");

            _chatOutput = GetNode<RichTextLabel>("Control/ChatPanel/Output");
            _chatInput = GetNode<LineEdit>("Control/ChatPanel/Input");

            // Assuming MiniMap is added to Control in Scene
            _miniMap = GetNodeOrNull<MiniMap>("Control/MiniMap");

            _chatInput.TextSubmitted += OnChatSubmitted;

            // Load Textures if available
            // Standard Mir: HP Sphere? Or Bars?
            // Often Prguse.Lib or UI.Lib has the orbs.
            // Placeholder: Use a white square if no texture set, but Godot TextureProgress needs texture.
            // We can generate one or load from Library if we knew index.

            // Demo: Generate simple gradient texture
            if (_hpBar.TextureProgress == null)
                _hpBar.TextureProgress = GenerateBarTexture(Colors.Red);
            if (_mpBar.TextureProgress == null)
                _mpBar.TextureProgress = GenerateBarTexture(Colors.Blue);
        }

        private ImageTexture GenerateBarTexture(Color color)
        {
            var img = Image.Create(100, 20, false, Image.Format.Rgba8);
            img.Fill(color);
            return ImageTexture.CreateFromImage(img);
        }

        public override void _Process(double delta)
        {
            _miniMap?.Process();
        }

        public void UpdateBars(int hp, int maxHp, int mp, int maxMp)
        {
            if (maxHp > 0) _hpBar.Value = (double)hp / maxHp * 100;
            if (maxMp > 0) _mpBar.Value = (double)mp / maxMp * 100;
        }

        public void UpdateCoordinates(string text)
        {
            _coordLabel.Text = text;
        }

        public void AddChatMessage(string msg, ChatType type)
        {
            Color color = Colors.White;
            switch (type)
            {
                case ChatType.System: color = Colors.Red; break;
                case ChatType.WhisperIn: color = Colors.Pink; break;
                case ChatType.Shout: color = Colors.Yellow; break;
            }

            _chatOutput.PushColor(color);
            _chatOutput.AddText(msg + "\n");
            _chatOutput.Pop();
        }

        private void OnChatSubmitted(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            NetworkManager.Enqueue(new ClientPackets.Chat { Message = text });
            _chatInput.Clear();
            _chatInput.ReleaseFocus(); // Or keep focus?
        }
    }
}

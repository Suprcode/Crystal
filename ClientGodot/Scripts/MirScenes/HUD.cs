using Godot;
using ClientPackets;
using ServerPackets;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class HUD : CanvasLayer
    {
        public static HUD Instance;

        private ProgressBar _hpBar;
        private ProgressBar _mpBar;
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
            _hpBar = GetNode<ProgressBar>("Control/Bars/HPBar");
            _mpBar = GetNode<ProgressBar>("Control/Bars/MPBar");
            _coordLabel = GetNode<Label>("Control/Bars/CoordLabel");

            _chatOutput = GetNode<RichTextLabel>("Control/ChatPanel/Output");
            _chatInput = GetNode<LineEdit>("Control/ChatPanel/Input");

            // Assuming MiniMap is added to Control in Scene
            _miniMap = GetNodeOrNull<MiniMap>("Control/MiniMap");

            _chatInput.TextSubmitted += OnChatSubmitted;
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

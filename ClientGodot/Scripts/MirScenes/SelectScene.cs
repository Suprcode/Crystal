using System.Collections.Generic;
using Godot;
using ClientPackets;
using ServerPackets;
using ClientGodot.Scripts.MirScenes;
using ClientGodot.Scripts.MirGraphics;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class SelectScene : MirScene
    {
        private List<SelectInfo> _characters = new List<SelectInfo>();

        // UI References
        private VBoxContainer _charListContainer;
        private Button _startGameButton;
        private Button _newCharButton;
        private Label _statusLabel;

        // New Character UI
        private Control _newCharPanel;
        private LineEdit _newCharName;
        private OptionButton _classOption;
        private OptionButton _genderOption;
        private Button _createCharButton;
        private Button _cancelCharButton;

        public override void _Ready()
        {
            // Find Nodes
            _charListContainer = GetNode<VBoxContainer>("UI/HBox/CharListPanel/ScrollContainer/CharList");
            _startGameButton = GetNode<Button>("UI/HBox/ControlPanel/StartGameButton");
            _newCharButton = GetNode<Button>("UI/HBox/ControlPanel/NewCharButton");
            _statusLabel = GetNode<Label>("UI/StatusLabel");

            _newCharPanel = GetNode<Control>("UI/NewCharDialog");
            _newCharName = _newCharPanel.GetNode<LineEdit>("VBox/NameEdit");
            _classOption = _newCharPanel.GetNode<OptionButton>("VBox/ClassOption");
            _genderOption = _newCharPanel.GetNode<OptionButton>("VBox/GenderOption");
            _createCharButton = _newCharPanel.GetNode<Button>("VBox/HBox/CreateButton");
            _cancelCharButton = _newCharPanel.GetNode<Button>("VBox/HBox/CancelButton");

            // Connect Signals
            _startGameButton.Pressed += OnStartGame;
            _newCharButton.Pressed += () => _newCharPanel.Visible = true;
            _createCharButton.Pressed += OnCreateChar;
            _cancelCharButton.Pressed += () => _newCharPanel.Visible = false;

            // Initialize Options
            _classOption.AddItem("Warrior", 0);
            _classOption.AddItem("Wizard", 1);
            _classOption.AddItem("Taoist", 2);
            _classOption.AddItem("Assassin", 3);
            _classOption.AddItem("Archer", 4);

            _genderOption.AddItem("Male", 0);
            _genderOption.AddItem("Female", 1);

            _newCharPanel.Visible = false;

            // Start loading libraries if not already
            if (!Libraries.Loaded)
                Libraries.Load();
        }

        public void SetCharacters(List<SelectInfo> chars)
        {
            _characters = chars;
            RefreshCharList();
        }

        private void RefreshCharList()
        {
            // Clear existing
            foreach (Node child in _charListContainer.GetChildren())
                child.QueueFree();

            foreach (var info in _characters)
            {
                Button btn = new Button();
                btn.Text = $"{info.Name} (Lv.{info.Level} {info.Class})";
                btn.ToggleMode = true;
                btn.ButtonGroup = new ButtonGroup(); // Make them radio buttons ideally, but simple for now
                btn.Pressed += () => SelectCharacter(info);
                _charListContainer.AddChild(btn);
            }
        }

        private SelectInfo _selectedChar;
        private void SelectCharacter(SelectInfo info)
        {
            _selectedChar = info;
            _statusLabel.Text = $"Selected: {info.Name}";
        }

        private void OnStartGame()
        {
            if (_selectedChar == null)
            {
                _statusLabel.Text = "Please select a character.";
                return;
            }
            NetworkManager.Enqueue(new ClientPackets.StartGame { CharacterIndex = _selectedChar.Index });
        }

        private void OnCreateChar()
        {
            string name = _newCharName.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                _statusLabel.Text = "Enter a name.";
                return;
            }

            MirClass c = (MirClass)_classOption.Selected;
            MirGender g = (MirGender)_genderOption.Selected;

            NetworkManager.Enqueue(new ClientPackets.NewCharacter
            {
                Name = name,
                Class = c,
                Gender = g
            });

            _newCharPanel.Visible = false;
            _statusLabel.Text = "Creating character...";
        }

        public override void Process()
        {
        }

        public override void ProcessPacket(Packet p)
        {
            switch (p)
            {
                case ServerPackets.NewCharacterSuccess success:
                    _statusLabel.Text = "Character Created.";
                    // Server usually sends UserInformation or similar afterwards, or we re-request?
                    // Original client: NewCharacterSuccess -> Insert into list
                    SelectInfo newChar = new SelectInfo
                    {
                        Index = success.CharInfo.Index,
                        Name = success.CharInfo.Name,
                        Level = success.CharInfo.Level,
                        Class = success.CharInfo.Class,
                        Gender = success.CharInfo.Gender
                    };
                    _characters.Add(newChar);
                    RefreshCharList();
                    break;
                case ServerPackets.NewCharacter fail:
                    _statusLabel.Text = "Creation Failed: " + fail.Result;
                    break;
                case ServerPackets.StartGame start:
                    if (start.Result == 0) return; // 0 usually means Fail in some Mir implementations? Or StartGame packet has simple result?
                    // Actually StartGame packet (Server) usually implies success if it contains map info.
                    // Wait, ServerPackets.StartGame is: public byte Result, Resolution;
                    // If result is 4, it's success? Need to check Enums.
                    // Let's assume > 0 is success or check handling.
                    _statusLabel.Text = $"Game Start! Result: {start.Result}";
                    // TODO: Switch to GameScene
                    break;
            }
        }
    }
}

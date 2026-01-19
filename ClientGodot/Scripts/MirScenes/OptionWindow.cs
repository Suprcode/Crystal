using Godot;
using ClientGodot.Scripts;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class OptionWindow : Panel
    {
        private HSlider _musicSlider;
        private HSlider _soundSlider;
        private CheckBox _fullscreenCheck;
        private Button _exitButton;
        private Button _closeButton;
        private Button _devButton;

        public override void _Ready()
        {
            _musicSlider = GetNode<HSlider>("VBox/MusicRow/Slider");
            _soundSlider = GetNode<HSlider>("VBox/SoundRow/Slider");
            _fullscreenCheck = GetNode<CheckBox>("VBox/FullscreenCheck");
            _exitButton = GetNode<Button>("VBox/ExitButton");
            _closeButton = GetNode<Button>("CloseButton");
            _devButton = GetNode<Button>("VBox/DevButton");

            _musicSlider.ValueChanged += OnMusicVolChanged;
            _soundSlider.ValueChanged += OnSoundVolChanged;
            _fullscreenCheck.Toggled += OnFullscreenToggled;
            _exitButton.Pressed += OnExitPressed;
            _closeButton.Pressed += () => Visible = false;

            _devButton.Pressed += () =>
            {
                var viewer = GD.Load<PackedScene>("res://Scenes/Dev/LibraryViewer.tscn").Instantiate();
                GetTree().Root.AddChild(viewer);
                Visible = false;
            };

            // Init Values
            _musicSlider.Value = 100; // Db to linear? AudioServer uses Db.
            _soundSlider.Value = 100;
        }

        private void OnMusicVolChanged(double value)
        {
            // Convert 0-100 to Db (-80 to 0)
            float db = (float)Mathf.LinearToDb(value / 100.0);
            int bus = AudioServer.GetBusIndex("Music");
            AudioServer.SetBusVolumeDb(bus, db);
        }

        private void OnSoundVolChanged(double value)
        {
            float db = (float)Mathf.LinearToDb(value / 100.0);
            int bus = AudioServer.GetBusIndex("SFX");
            AudioServer.SetBusVolumeDb(bus, db);
        }

        private void OnFullscreenToggled(bool pressed)
        {
            if (pressed)
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
            else
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        private void OnExitPressed()
        {
            GetTree().Quit();
        }
    }
}

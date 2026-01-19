using Godot;
using ClientGodot.Scripts.MirGraphics;

namespace ClientGodot.Scripts.Dev
{
    public partial class LibraryViewer : Control
    {
        private TextureRect _display;
        private Label _infoLabel;
        private SpinBox _indexSpinner;
        private OptionButton _libSelector;

        private MLibrary _currentLib;

        public override void _Ready()
        {
            _display = GetNode<TextureRect>("VBox/Panel/TextureRect");
            _infoLabel = GetNode<Label>("VBox/InfoLabel");
            _indexSpinner = GetNode<SpinBox>("VBox/Controls/IndexSpinner");
            _libSelector = GetNode<OptionButton>("VBox/Controls/LibSelector");

            _indexSpinner.ValueChanged += OnIndexChanged;
            _libSelector.ItemSelected += OnLibSelected;

            // Populate Libs
            _libSelector.AddItem("UI", 0);
            _libSelector.AddItem("Items", 1);
            _libSelector.AddItem("ChrSel", 2);
            _libSelector.AddItem("Prguse", 3);
            _libSelector.AddItem("MagIcon", 4);
            // ... add more as needed

            // Load libs if not loaded (Standalone mode)
            if (!Libraries.Loaded)
            {
                Libraries.Load();
                // Wait for load?
                // Simple hack: Timer check or just hope it's fast enough for dev tool
            }

            // Default
            _currentLib = Libraries.UI_32bit;
            UpdateDisplay();
        }

        private void OnLibSelected(long index)
        {
            switch(index)
            {
                case 0: _currentLib = Libraries.UI_32bit; break;
                case 1: _currentLib = Libraries.Items; break;
                case 2: _currentLib = Libraries.ChrSel; break;
                case 3: _currentLib = Libraries.Prguse; break;
                case 4: _currentLib = Libraries.MagIcon; break;
            }
            UpdateDisplay();
        }

        private void OnIndexChanged(double value)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_currentLib == null)
            {
                _infoLabel.Text = "Library not loaded.";
                return;
            }

            int idx = (int)_indexSpinner.Value;
            var img = _currentLib.GetImage(idx);

            if (img != null)
            {
                var tex = img.CreateTexture();
                _display.Texture = tex;
                _infoLabel.Text = $"Index: {idx}\nSize: {img.Width}x{img.Height}\nOffset: {img.X}, {img.Y}";
            }
            else
            {
                _display.Texture = null;
                _infoLabel.Text = $"Index {idx}: Not Found / Empty";
            }
        }
    }
}

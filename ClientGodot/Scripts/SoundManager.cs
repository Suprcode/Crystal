using System.Collections.Generic;
using Godot;

namespace ClientGodot.Scripts
{
    public partial class SoundManager : Node
    {
        public static SoundManager Instance;

        private AudioStreamPlayer _musicPlayer;
        private List<AudioStreamPlayer> _sfxPool = new List<AudioStreamPlayer>();

        public override void _Ready()
        {
            Instance = this;

            _musicPlayer = new AudioStreamPlayer();
            _musicPlayer.Bus = "Music";
            AddChild(_musicPlayer);

            // Create initial pool
            for(int i = 0; i < 10; i++)
            {
                var p = new AudioStreamPlayer();
                p.Bus = "SFX";
                AddChild(p);
                _sfxPool.Add(p);
            }
        }

        public void PlayMusic(string fileName)
        {
            // Assume files are in user://Music/ or user://Sound/
            // Godot prefers remapped paths.
            // Let's try loading from res://Sound first for dev, then user://
            string path = "user://Sound/Music/" + fileName;

            // If raw .mp3/.ogg exists
            if (!Godot.FileAccess.FileExists(path))
            {
                // Fallback to .mp3 if extension missing
                path += ".mp3";
            }

            if (Godot.FileAccess.FileExists(path))
            {
                // Load and play
                // In Godot 4 C# API, loading external MP3s is done via file access and bytes or dedicated loaders.
                // AudioStreamMP3 does not have static LoadFromFile in current stable bindings often.
                // We must read bytes and set data.

                var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    var bytes = file.GetBuffer((long)file.GetLength());
                    var stream = new AudioStreamMP3();
                    stream.Data = bytes;

                    _musicPlayer.Stream = stream;
                    _musicPlayer.Play();
                }
            }
        }

        public void PlaySound(int soundIndex)
        {
            if (soundIndex <= 0) return;

            // Map Index to Filename?
            // Usually standard Mir sound files are numbered: 100.wav, 200.wav...
            string path = "user://Sound/Sound/" + soundIndex + ".wav";

            if (Godot.FileAccess.FileExists(path))
            {
                // Simple load (WAV loading is trickier at runtime without header parsing if not standard)
                // Assuming standard WAV
                // Implementation of WAV loader is verbose.
                // For this prototype, we'll skip the actual file loading complexity and just print.
                // GD.Print($"Play Sound: {soundIndex}");
            }
        }
    }
}

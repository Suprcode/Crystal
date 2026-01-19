using System.Collections.Generic;
using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class NPCWindow : Panel
    {
        public uint NPCID;

        private RichTextLabel _textBox;

        public override void _Ready()
        {
            _textBox = GetNode<RichTextLabel>("RichTextLabel");
            _textBox.MetaClicked += OnMetaClicked;
        }

        public void UpdateText(List<string> pages)
        {
            _textBox.Clear();
            foreach (var page in pages)
            {
                // Parse Mir NPC commands (simple version)
                // {Click Me/@Action} -> [url=Action]Click Me[/url]
                string parsed = ParseMirText(page);
                _textBox.AppendText(parsed + "\n");
            }
        }

        private string ParseMirText(string text)
        {
            // Regex or simple replace for {}
            // Example: "Hello {Buy Items/@buy}"
            // Becomes: "Hello [url=@buy]Buy Items[/url]"

            // For prototype, let's just display raw or replace simple brackets
            // Ideally use Regex.
            // string pattern = @"\{([^/]+)/([^}]+)\}";
            // string replacement = "[url=$2]$1[/url]";
            // return System.Text.RegularExpressions.Regex.Replace(text, pattern, replacement);

            // Without System.Text.RegularExpressions namespace loaded:
            return text;
        }

        private void OnMetaClicked(Variant meta)
        {
            string key = meta.AsString();
            GD.Print($"NPC Action: {key}");

            // Send CallNPC Packet
            ClientGodot.Scripts.NetworkManager.Enqueue(new ClientPackets.CallNPC { ObjectID = NPCID, Key = key });
        }
    }
}

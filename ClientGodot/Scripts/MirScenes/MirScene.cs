using Godot;

namespace ClientGodot.Scripts.MirScenes
{
    public abstract partial class MirScene : Control
    {
        public abstract void Process();
        public abstract void ProcessPacket(Packet p);
    }
}

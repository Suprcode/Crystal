using ClientGodot.Scripts.MirScenes;

namespace ClientGodot.Scripts
{
    // Simple static holder for the active scene to allow NetworkManager to access it
    public static class SceneManager
    {
        public static MirScene ActiveScene;
    }
}

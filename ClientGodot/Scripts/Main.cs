using Godot;
using ClientGodot.Scripts;
using ClientGodot.Scripts.MirScenes;

public partial class Main : Node
{
    private MirScene _activeScene;
    private PackedScene _loginSceneRes;

	public override void _Ready()
	{
		GD.Print("Main Scene Ready. Initializing Network...");

		Packet.IsServer = false;
        Settings.Load();

		NetworkManager.Connect();

        // Load Login Scene Resource
        _loginSceneRes = GD.Load<PackedScene>("res://Scenes/LoginScene.tscn");
        if (_loginSceneRes != null)
        {
            ChangeScene(_loginSceneRes.Instantiate<MirScene>());
        }
        else
        {
            GD.PrintErr("Failed to load LoginScene.tscn");
        }
	}

	public override void _Process(double delta)
	{
		NetworkManager.Process();
        _activeScene?.Process();

        // Dispatch received packets to the active scene
        // We need to modify NetworkManager to allow us to dequeue packets,
        // OR we just let NetworkManager handle the queue and we access it?
        // In the original, NetworkManager.Process() calls ActiveScene.ProcessPacket(p).
        // Let's adapt NetworkManager to call Main.Instance.ProcessPacket or similar.
        // For simplicity in this step, let's assume NetworkManager needs a reference to the Active Scene.
        // However, since NetworkManager is static, we can add a property there.
	}

    public void ChangeScene(MirScene newScene)
    {
        if (_activeScene != null)
        {
            _activeScene.QueueFree();
        }

        _activeScene = newScene;
        AddChild(_activeScene);

        // Update NetworkManager's reference (See next step)
        SceneManager.ActiveScene = _activeScene;
    }
}

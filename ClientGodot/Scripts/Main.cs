using Godot;
using ClientGodot.Scripts;

public partial class Main : Node
{
	public override void _Ready()
	{
		GD.Print("Main Scene Ready. Initializing Network...");

		// IMPORTANT: Set Packet.IsServer to false for client
		Packet.IsServer = false;

		NetworkManager.Connect();
	}

	public override void _Process(double delta)
	{
		NetworkManager.Process();
	}
}

using Godot;

namespace CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class SetupGameAsClient : Control
{
	
	
	[Export] private Button _joinButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_joinButton = GetNode<Button>("JoinButton");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_join_button_pressed()
	{
		ENetMultiplayerPeer peer = new ();
		peer.CreateClient(SetupGameAsHost.ServerIp, SetupGameAsHost.DefaultPort);
		var multiplayerApi = GetTree().GetMultiplayer();
		multiplayerApi.MultiplayerPeer = peer;
		GetTree().SetMultiplayer(multiplayerApi);
		GD.Print("Waiting to join...");
		
	}
}

/*

private void _clientPressed() // joinpressed
        {
           

            
             * _peer.CompressionMode = ENetMultiplayerPeer.CompressionModeEnum.RangeCoder;
             
            _peer.CreateClient(ServerIp, DefaultPort);
            
             * GetTree().NetworkPeer = _peer;
             
        }
        
*/
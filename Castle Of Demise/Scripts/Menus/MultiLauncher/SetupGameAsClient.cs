using CastleOfDemise.Scripts.Menus;
using Godot;

namespace CastleOfDemise.menus.MultiLauncher;

public partial class SetupGameAsClient : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
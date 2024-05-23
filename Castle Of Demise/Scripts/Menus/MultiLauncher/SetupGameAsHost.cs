using Godot;

namespace CastleOfDemise.menus.MultiLauncher;

public partial class SetupGameAsHost : Control
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
private void _hostPressed()  
        {
            _peer = new ENetMultiplayerPeer();
            
             * _peer.CompressionMode = ENetMultiplayerPeer.CompressionModeEnum.RangeCoder;
             
Error err = _peer.CreateServer(DefaultPort, MaxNumberOfPeers);
if (err != Error.Ok)
{
	// Is another server running?
	return;
}
GetTree().ChangeSceneToFile("res://maps/mpMap01.tscn");
_add_player();
}

private void _add_player(int id = 1)
{
	var player = GD.Load<PackedScene>("res://maps/mpMap01.tscn").Instantiate();
	player.Name = id.ToString();
	/* GetTree().Root.AddChild(player);
	
}
*/

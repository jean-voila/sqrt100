using Godot;

namespace CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class SetupGameAsHost : Control
{
	// Called when the node enters the scene tree for the first time.

	[Export] private ItemList _chooseGameMode;
	[Export] private ItemList _scoreToReach;
	[Export] private Button _startGameButton;
	[Export] private string _address = "127.0.0.1";
	private int? _scoreToReachValue = null;
	private int? _gameModeValue = null;
	
	
	
	public ENetMultiplayerPeer Peer = new();
	public const int DefaultPort = 8910;
	public const int MaxNumberOfPeers = 2;
	public const string ServerIp = "127.0.0.1";
	
	public override void _Ready()
	{
		_chooseGameMode = GetNode<ItemList>("ChooseGameMode");
		_scoreToReach = GetNode<ItemList>("ScoreToReach");
		_startGameButton = GetNode<Button>("StartGame");
		
		
		
		
	}
	
	
	private void PlayerConnected(int id)
	{
		//_add_player(id);
		GD.Print("Player connected: " + id);
	}

	// Ces 2 fonctions sont des fonctions de select pour le multi
	private void _on_choose_game_mode_item_selected(int index)
	{
		GD.Print("Game mode: " + _chooseGameMode.GetItemText(index));
		if (index != 0)
		{
			_gameModeValue = index;
		}
		else
		{
			_gameModeValue = null;
		}
		
		if (_gameModeValue != null && _scoreToReachValue != null)
		{
			_startGameButton.Disabled = false;
		}
		else
		{
			_startGameButton.Disabled = true;
		}
	}


	private void _on_score_to_reach_item_selected(int index)
	{
		GD.Print("Score to reach: " + _scoreToReach.GetItemText(index));
		if (index != 0)
		{
			_scoreToReachValue = int.Parse(_scoreToReach.GetItemText(index));
		}
		else
		{
			_scoreToReachValue = null;
		}
		
		if (_gameModeValue != null && _scoreToReachValue != null)
		{
			_startGameButton.Disabled = false;
		}
		else
		{
			_startGameButton.Disabled = true;
		}
	}
	
	private void _on_start_game_pressed()
	{
		 var err = Peer.CreateServer(DefaultPort, MaxNumberOfPeers);
         		if (err != Error.Ok)
         		{
         			// Is another server running?
         			GD.Print("Cannot host server");
         			return;
         		}
         
         		var multiplayerApi = GetTree().GetMultiplayer();
         		multiplayerApi.MultiplayerPeer = Peer;
         		GetTree().SetMultiplayer(multiplayerApi);
         		GD.Print("Waiting for players to connect...");
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

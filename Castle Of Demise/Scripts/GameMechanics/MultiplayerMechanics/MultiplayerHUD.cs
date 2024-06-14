using Godot;
using System;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class MultiplayerHUD : CanvasLayer
{
	
	
	private static int _scoretoReachvalue = 0;
	private static int _gameModeValue = 0;

	[Export] private string _hostScoreText;
	[Export] private string _scoreToReachText;
	[Export] private string _clientScoretext;

        
        
	private static int _hostScore = 0;
	private static int _clientScore = 0;
	private static int _scoreToReach = 0;

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public static void HostScored()
	{
		_hostScore++;
            
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]

	public static void ClientScored()
	{
		_clientScore++;
	}

	private static void CheckWin()
	{
		if (_hostScore >= _scoreToReach)
		{
			//host wins
		}
		else if (_clientScore >= _scoreToReach)
		{
			//client wins
		}
	}
        
        
        

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Player.IsMultiplayer)
		{
			(_scoretoReachvalue, _gameModeValue) = GetNode<SetupGameAsHost>("SetupGameAsHost").Data;
			_scoreToReach = _scoretoReachvalue;
		}
		

		


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player.IsMultiplayer)
		{
			GetNode<Label>("%HostScore").Text = _hostScoreText;
			GetNode<Label>("%ScoreToReach").Text = _scoreToReachText;
			GetNode<Label>("%ClientScore").Text = _clientScoretext;
			
			_hostScoreText = _hostScore.ToString();
			_clientScoretext = _clientScore.ToString();
		}
	}
}

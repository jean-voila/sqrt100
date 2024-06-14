using Godot;
using System;
using System.Diagnostics;
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]

	private  void CheckWin()
	{
		if (_hostScore >= _scoretoReachvalue)
		{
			menuPartieFinie.winnerName = "Hôte";
			GetTree().ChangeSceneToFile("res://Scripts/Menus/menuPartieFinie.cs");
		}
		else if (_clientScore >= _scoretoReachvalue)
		{
			menuPartieFinie.winnerName = "Client";
			GetTree().ChangeSceneToFile("res://Scripts/Menus/menuPartieFinie.cs");

		}
	}
        
        
        

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Player.IsMultiplayer)
		{
			_scoretoReachvalue = (int)SetupGameAsHost._scoreToReachValue;
			_gameModeValue = (int)SetupGameAsHost._gameModeValue;
			_scoreToReachText = _scoretoReachvalue.ToString();
			GetNode<RichTextLabel>("%ScoreToReach").Text = _scoreToReachText;
			
			
			GD.Print("");
			GD.Print("===== RAPPORT =====");
			GD.Print("score to reach: " + _scoretoReachvalue);
			GD.Print("host score: " + _hostScore);
			GD.Print("client score: " + _clientScore);

			GD.Print("===== END OF =====");
			GD.Print("");

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player.IsMultiplayer)
		{
			GetNode<RichTextLabel>("%HostScore").Text = _hostScoreText;
			GetNode<RichTextLabel>("%ClientScore").Text = _clientScoretext;
			
			_hostScoreText = _hostScore.ToString();
			_clientScoretext = _clientScore.ToString();
		}
	}
}

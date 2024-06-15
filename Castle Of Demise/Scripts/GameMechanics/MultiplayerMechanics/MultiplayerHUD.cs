using Godot;
using System;
using System.Diagnostics;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class MultiplayerHUD : CanvasLayer
{
	
	
	[Export] public static int ScoretoReachValue = -99;
	private static int _gameModeValue = 0;

	[Export] private  string _hostScoreText;
	[Export] private  string _scoreToReachText;
	[Export] private  string _clientScoreText;
	
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public static void HostScored()
	{
		mpMap02.PlayerList[0].PlayerScore++;
		GD.Print("Host scored");
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]

	public static void ClientScored()
	{
		mpMap02.PlayerList[1].PlayerScore++;
		GD.Print("Client scored");

	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]

	private  void CheckWin()
	{
		if (mpMap02.PlayerList[0].PlayerScore >= ScoretoReachValue)
		{
			menuPartieFinie.winnerName = "Hôte";
			GetTree().ChangeSceneToFile("res://Scripts/Menus/menuPartieFinie.cs");
		}
		else if (mpMap02.PlayerList[1].PlayerScore >= ScoretoReachValue)
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
			// _gameModeValue = SetupGameAsHost._gameModeValue == 0 ? (int)SetupGameAsHost._gameModeValue : -1;
			
			
			// SendMultiplayerStartGameReport();

		}
	}

	private void SendMultiplayerStartGameReport()
	{
		GD.Print("");
		GD.Print("===== RAPPORT =====");
		GD.Print("score to reach: " + ScoretoReachValue);
		GD.Print("host score: " + mpMap02.PlayerList[0].PlayerScore);
		GD.Print("client score: " + mpMap02.PlayerList[1].PlayerScore);

		GD.Print("===== END OF =====");
		GD.Print("");
	}  //pa toucher
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Player.IsMultiplayer)
		{
			GetNode<RichTextLabel>("%HostScore").Text = _hostScoreText;
			GetNode<RichTextLabel>("%ClientScore").Text = _clientScoreText;
			
			_hostScoreText = mpMap02.PlayerList[0].PlayerScore.ToString();
			_clientScoreText = mpMap02.PlayerList[1].PlayerScore.ToString();
			
			
			_scoreToReachText = ScoretoReachValue.ToString();
			GetNode<RichTextLabel>("%ScoreToReach").Text = _scoreToReachText;

		}
	}
}

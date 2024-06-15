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
	[Export] private  string _clientScoreText;

	public static int serverscore = 0;
	
	
	

	
	
	

	
        
        
        

	
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
			GetNode<RichTextLabel>("%HostScore").Text = "Hôte \n"+_hostScoreText;
			GetNode<RichTextLabel>("%ClientScore").Text = "Client \n"+_clientScoreText;
			
			_hostScoreText = mpMap02.PlayerList[0].PlayerScore.ToString();
			_clientScoreText = mpMap02.PlayerList[1].PlayerScore.ToString();
			
			
			GetNode<RichTextLabel>("%ScoreToReach").Text = "Objectif \n"+
				ScoretoReachValue.ToString();
			
		}
	}
}

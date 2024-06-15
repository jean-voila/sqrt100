using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class mpMap02 : Node3D
{
	[Export] private PackedScene _multiplayerScene02;
	public static readonly List<Player> PlayerList= new();
	public static int ScoreToReachUltimate;
	// private static int scoretoReachvalue = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var item in GameManager.Players)
		{
			Player current = (Player)_multiplayerScene02.Instantiate<Player>();
			
			current.PlayerName = item.Value.PlayerName;
			current.PlayerId = item.Value.PlayerId;
			current.PlayerScore = item.Value.PlayerScore;
			current.IsServer = item.Value.IsServer;
			if (Multiplayer.GetUniqueId() == 1)
			{
				MultiplayerHUD.ScoretoReachValue = SetupGameAsHost._scoreToReachValue;
				// GD.Print(ScoreToReachUltimate + "is the ultimate score to reach bigbadboy");
				Rpc(nameof(SetScoreToReach), SetupGameAsHost._scoreToReachValue);
				//GD.Print("call func");
			}
			else
			{
				//GD.Print("There is no scoretoreach value set bc client yk");
				
			}
			current.Name = item.Value.PlayerId.ToString(); // techniquement inutile
			// mais ca permet de suivre une logique appréciable
			PlayerList.Add(current);
			AddChild(current);
			current.GlobalScale(new Vector3(2, 2, 2));
			foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
			{
				if (spawnPoint.Name == item.Key.ToString())
				{
					current.Teleport(((Node3D)spawnPoint).GlobalTransform.Origin);
				}
			}
			
		}
		
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SetScoreToReach(int score)
	{
		ScoreToReachUltimate = score;
		MultiplayerHUD.ScoretoReachValue = score;
		//GD.Print("function called");
		//GD.Print("result: " + score + " , " + ScoreToReachUltimate + " , " + MultiplayerHUD.ScoretoReachValue);
	}

	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

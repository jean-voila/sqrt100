using Godot;
using System;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class mpMap02 : Node3D
{
	[Export] private PackedScene _multiplayerScene02;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player player0 = (Player)_multiplayerScene02.Instantiate();
		AddChild(player0);
		player0.PlayerName = GameManager.Players[0].PlayerName;
		player0.PlayerId = GameManager.Players[0].PlayerId;
		player0.PlayerScore = GameManager.Players[0].PlayerScore;
		foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
		{
			if (spawnPoint.Name == 0.ToString())
			{
				player0.Teleport(((Node3D)spawnPoint).GlobalTransform.Origin);
			}
		}
		
		Player player1 = (Player)_multiplayerScene02.Instantiate();
		AddChild(player1);
		player1.PlayerName = GameManager.Players[1].PlayerName;
		player1.PlayerId = GameManager.Players[1].PlayerId;
		player1.PlayerScore = GameManager.Players[1].PlayerScore;

		foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
		{
			if (spawnPoint.Name == 1.ToString())
			{
				player1.Teleport(((Node3D)spawnPoint).GlobalTransform.Origin);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

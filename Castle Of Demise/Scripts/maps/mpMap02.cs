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
		int index = 0;
		foreach (var item in GameManager.Players)
		{
			Player current = (Player)_multiplayerScene02.Instantiate();
			current.PlayerName = GameManager.Players[index].PlayerName;
			current.PlayerId = GameManager.Players[index].PlayerId;
			current.PlayerScore = GameManager.Players[index].PlayerScore;
			AddChild(current);
			foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
			{
				if (spawnPoint.Name == 0.ToString())
				{
					current.Teleport(((Node3D)spawnPoint).GlobalTransform.Origin);
				}
			}
			index++;
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

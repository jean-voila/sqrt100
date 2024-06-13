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
		foreach (var item in GameManager.Players)
		{
			Player current = (Player)_multiplayerScene02.Instantiate<Player>();
			current.PlayerName = item.Value.PlayerName;
			current.PlayerId = item.Value.PlayerId;
			current.PlayerScore = item.Value.PlayerScore;
			current.IsServer = item.Value.IsServer;
			current.Name = item.Value.PlayerId.ToString(); // techniquement inutile,
                                                  // mais ca permet de suivre une logique appréciable
			AddChild(current);
			foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
			{
				if (spawnPoint.Name == item.Key.ToString())
				{
					current.Teleport(((Node3D)spawnPoint).GlobalTransform.Origin);
				}
			}
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

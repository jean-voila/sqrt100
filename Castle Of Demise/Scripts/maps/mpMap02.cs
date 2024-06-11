using Godot;
using System;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class mpMap02 : Node3D
{
	[Export] private PackedScene _playerScene;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int index = 0;
		for (int i = 0; i < GameManager.Players.Count; i++)
		{
			Player currentPlayer = (Player)_playerScene.Instantiate();
			AddChild(currentPlayer);
			foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnpoint"))
			{
				if (spawnPoint.Name == index.ToString())
				{
					currentPlayer.Position = ((Node3D)spawnPoint).Transform.Origin;
					index++;
				}
			}

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

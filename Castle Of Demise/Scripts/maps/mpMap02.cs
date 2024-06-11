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
        for (int i = 0; i < GameManager.Players.Count; i++)
		{
			if (_multiplayerScene02 == null)
			{
				GD.Print("_playerScene is null");
				return;
			}

			Player currentPlayer = (Player)_multiplayerScene02.Instantiate();
			AddChild(currentPlayer);

			Node3D spawnPoint = GetNode<Node3D>(i.ToString());
			if (spawnPoint == null)
			{
				GD.PrintErr("No Node3D with name " + i.ToString());
				return;
			}

			currentPlayer.Teleport(spawnPoint.GlobalTransform.Origin);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

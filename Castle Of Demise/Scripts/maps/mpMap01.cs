using Godot;
using System;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class mpMap01 : Node3D
{
    
	/*   // DO NOT USE
	[Export] private PackedScene _playerScene;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		int index = 0;
		for (int i = 0; i < GameManager.Players.Count; i++)
		{
			Node currentPlayer = _playerScene.Instantiate();
			AddChild(currentPlayer);
			foreach (var spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoint"))
			{
				if (spawnPoint.Name == index.ToString())
				{
					((Node3D)currentPlayer).Transform = ((Node3D)currentPlayer).Transform with
					{
						Origin = ((Node3D)spawnPoint).Transform.Origin
					};					index++;
				}
			}

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	*/
}

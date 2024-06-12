using Godot;
using System;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class mpMap01 : Node3D
{
    
	[Export] public CharacterBody3D player;
	
	public override void _Ready()
	{
		// GD.Print(GetTree().GetNodesInGroup("ennemies"));
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(player.GlobalTransform.Origin);
		GetTree().CallGroup("ennemies","UpdateTargetLocation", player.GlobalTransform.Origin);
	}
}

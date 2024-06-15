using Godot;
using System;
using System.Linq;
using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class IEnnemiesTrackable : Node3D
{
    
	[Export] public CharacterBody3D player;
	[Export] private Node3D _ennemiesNode;
	
	
	public override void _Ready()
	{
		// GD.Print(GetTree().GetNodesInGroup("ennemies"));
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(player.GlobalTransform.Origin);
		GetTree().CallGroup("ennemies","UpdateTargetLocation", player.GlobalTransform.Origin);
		HandleWin();
		GD.Print(_ennemiesNode.GetChildCount()==0);

	}

	public void HandleWin()
	{
		if (_ennemiesNode.GetChildCount()==0)
		{
			GetTree().ChangeSceneToFile("res://menus/TitleScreen.tscn");
		}
	}
}

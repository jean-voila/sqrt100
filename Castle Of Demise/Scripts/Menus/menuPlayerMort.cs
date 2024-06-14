using Godot;
using System;
using CastleOfDemise.Scripts.Menus;

public partial class menuPlayerMort : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void _on_button_button_down()
	{
		GetTree().ChangeSceneToFile("res://menus/TitleScreen.tscn");
	}
}

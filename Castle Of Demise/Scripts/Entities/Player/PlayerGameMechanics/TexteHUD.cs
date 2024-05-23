using Godot;

namespace CastleOfDemise.Scripts.Entities.Player.PlayerGameMechanics;

public partial class TexteHUD : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	bool hideHUDDefault = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get a reference to the label
	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double d)
	{
		if (Input.IsActionJustPressed("hideHUD")){
			hideHUDDefault = !hideHUDDefault;
		}
		this.Visible = !hideHUDDefault;

	}
	
	
	
	
	
	// Multiplayer part made for testing purposes, to be archived
	
	
}
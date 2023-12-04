using Godot;
using System;

public class TexteHUD : RichTextLabel
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	bool hideHUD = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		

	}
	

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

		if (Input.IsActionJustPressed("hideHUD")){
			 hideHUD = !hideHUD;
		}
		this.Visible = !hideHUD;

	}
}

using Godot;
using System;
using CastleOfDemise.Scripts.Menus;

public partial class menuPartieFinie : Node2D
{
	private string winnerName = "shimmy";
	[Export] private Label winnerLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		winnerLabel.Text = $"Le joueur {winnerName} a gagné.";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_button_button_up()
	{
		GetTree().ChangeSceneToFile("res://menus/TitleScreen.tscn");
		MultiplayerMenu.Peer = null;
	}
}

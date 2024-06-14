using Godot;
using System;


public partial class TestTutoriel : Node3D
{
	private bool _hasAlready_entered = false;
	[Export] private AudioStreamPlayer _musicPlayer;

	[Export] private CanvasLayer _gameTitle;
	
	[Export] private CharacterBody3D _player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameTitle.Visible = false;
		_musicPlayer.MaxPolyphony = 1;
		_musicPlayer.EmitSignal("PlaySFXSignal", "AmbientNight");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_3d_body_entered(Node3D body)
	{
		if (!_hasAlready_entered)
		{
			_gameTitle.Show();
			_musicPlayer.EmitSignal("PlaySFXSignal", "DiesIrae");
			_hasAlready_entered = true;
			_player.EmitSignal("SwitchCinematicModeSignal", true);
			_player.EmitSignal("SwitchLevitationModeSignal", true);
			
		}
		
	}
	
}
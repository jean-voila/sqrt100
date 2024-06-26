using Godot;

namespace CastleOfDemise.Scripts.Menus;

public partial class TitleScreen : Control
{
	[Export] private AudioStreamPlayer _sfxPlayer;
	[Export] private AudioStreamPlayer _musicPlayer;
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
	
	private void _PlayButtonPressed()
	{
		GetNode<Control>("%TitleMenu").Hide();
		GetNode<Control>("%MultiplayerMenu").Hide();
		GetNode<Control>("%GamemodeMenu").Show();
	}

	
	
	private void _Exit()
	{
		GetTree().Quit();
	}

	private void _OptionButtonPressed()
	{
		GetNode<Control>("%TitleMenu").Hide();
		GetNode<Control>("%OptionsMenu").Show();
	}

	private void _focused_button()
	{
		_sfxPlayer.EmitSignal("PlaySFXSignal", "buttons/focused");
	}

	private void _pressed_button()
	{
		_sfxPlayer.EmitSignal("PlaySFXSignal", "buttons/pressed");
	}

	private void _show_title_screen_()
	{
		GetNode<Control>("%GamemodeMenu").Hide();
		GetNode<Control>("%OptionsMenu").Hide();
		GetNode<Control>("%TitleMenu").Show();
	}
	
	private void _DebugButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://maps/TestsMap.tscn");
	}
	
	private void _TutoButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://maps/TestTutoriel.tscn");
	}

	private void _NewMultiPressed()
	{
		GetTree().ChangeSceneToFile("res://maps/mpMap02.tscn");
	}

	private void _LeidenStadtPressed()
	{
		GetTree().ChangeSceneToFile("res://maps/LeidenStadt.tscn");
	}

	private void _DemoButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://maps/mpMap01.tscn");
	}
	

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double d)
	{

	}
}

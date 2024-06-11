using Godot;
using System;
using Godot.Collections;

public partial class FPS : Control
{
	private OptionButton _button;
	private Dictionary<int, int> FPS_DI = new Dictionary<int, int>
	{
		{ 0, 0 },
		{ 1, 120 },
		{ 2, 60 },
		{ 3, 30 }
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<OptionButton>("HBoxContainer/OptionButton");
		LoadFpsFromSettings();
	}

	public void LoadFpsFromSettings()
	{
		var fps = ConfigFileHandler.LoadFpsSettings();
		OnFpsDropdownItemSelected(fps);
	}
	private void OnFpsDropdownItemSelected(int index)
	{
		if (FPS_DI.ContainsKey(index))
		{
			Engine.MaxFps = FPS_DI[index];
			_button.Select(index);
			GD.Print($"3 {FPS_DI[index]}");
			ConfigFileHandler.SaveFpsSetting(index);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

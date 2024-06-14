using Godot;
using System;
using Godot.Collections;

public partial class menu_de_difficulté : Control
{
	private OptionButton _button;
	private Dictionary<int, string> DiffNiveau = new Dictionary<int, string>
	{
		{ 0, "Facile" },
		{ 1, "Normale" },
		{ 2, "Difficile" },
		{ 3, "Impossible" }
	};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<OptionButton>("HBoxContainer/OptionButton");
		LoadDiffFromSettings();
	}
	public void LoadDiffFromSettings()
	{
		var Diff = ConfigFileHandler.LoadDiffSettings();
		OnDiffDropdownItemSelected(Diff);
	}
	private void OnDiffDropdownItemSelected(int index)
	{
		if (DiffNiveau.ContainsKey(index))
		{
			// changement que tu veux faire
			_button.Select(index);
			ConfigFileHandler.SaveDiffSetting(index);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

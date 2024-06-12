using Godot;
using System;

public partial class audio_slider_settings : Control
{
	private Label _audioNameLbl;
	private Label _audioNumLbl;
	private HSlider _hSlider;
	private int _busIndex = 0;
	public enum BusName
	{
		Master,
		Musique,
		SFX
	}
	[Export] public BusName busName { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioNameLbl = GetNode<Label>("HBoxContainer/Audio_Name_Lbl");
		_audioNumLbl = GetNode<Label>("HBoxContainer/Audio_Num_Lbl");
		_hSlider = GetNode<HSlider>("HBoxContainer/HSlider");
		LoadSoundFromSettings();
		_hSlider.ValueChanged += OnValueChanged;
		
		GetBusNameByIndex();
		SetNameLabelText();
		SetSliderValue();
	}
	
	public void LoadSoundFromSettings()
	{
		var keybindings = ConfigFileHandler.LoadAudioSettings();
		AudioServer.SetBusVolumeDb(0,(float)LinearToDb(keybindings["Master"]));
		AudioServer.SetBusVolumeDb(1,(float)LinearToDb(keybindings["SFX"]));
		AudioServer.SetBusVolumeDb(2,(float)LinearToDb(keybindings["Musique"]));
		/*int i = 0;
		foreach (var action in keybindings.Values)
		{
			
			AudioServer.SetBusVolumeDb(i,(float)LinearToDb(action));
			i += 1;
		}*/
	}
	
	private void OnValueChanged(double value)
	{
		AudioServer.SetBusVolumeDb(_busIndex, (float)LinearToDb(value));
		SetAudioNumLabelText();
		ConfigFileHandler.SaveAudioSetting($"{busName}",value);
	}
	public void SetNameLabelText()
	{
		_audioNameLbl.Text = $"{busName} Volume";
	}
	public void SetAudioNumLabelText()
	{
		_audioNumLbl.Text = (_hSlider.Value * 100).ToString("0.#")+"%";
	}
	public void GetBusNameByIndex()
	{
		_busIndex = AudioServer.GetBusIndex($"{busName}");
	}
	public void SetSliderValue()
	{
		_hSlider.Value = DbToLinear(AudioServer.GetBusVolumeDb(_busIndex));
		SetAudioNumLabelText();
	}
	
	
	public double LinearToDb(double linear) 
	{
		if (linear <= 0)
		{
			return -80; // -80 dB is typically the minimum dB level in many audio systems
		}
		return 20 * Math.Log10(linear);
	}
	public static double DbToLinear(double db)
	{
		return Math.Pow(10, db / 20);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

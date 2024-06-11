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
		
		_hSlider.ValueChanged += OnValueChanged;
		
		GetBusNameByIndex();
		SetNameLabelText();
		SetSliderValue();
	}
	private void OnValueChanged(double value)
	{
		AudioServer.SetBusVolumeDb(_busIndex, (float)LinearToDb(value));
		SetAudioNumLabelText();
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
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using FileAccess = Godot.FileAccess;

public partial class ConfigFileHandler : Node
{
	private const string SETTINGS_FILE_PATH = "user://settings.ini";
	public static ConfigFile config = new ConfigFile();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!FileAccess.FileExists(SETTINGS_FILE_PATH))
		{
			config.SetValue("Diff", "diff",1);
			config.SetValue("FPS","fps",1);
			config.SetValue("Audio", "Master", 1.0);
			config.SetValue("Audio", "Musique", 1.0);
			config.SetValue("Audio", "SFX", 1.0);
			config.SetValue("keybinding", "key_z", "Z");
			config.SetValue("keybinding", "key_s", "S");
			config.SetValue("keybinding", "key_q", "Q");
			config.SetValue("keybinding", "key_d", "D");
			config.SetValue("keybinding", "key_space", "Space");
			config.SetValue("keybinding", "key_escape", "Escape");
			config.SetValue("keybinding", "hideHUD", "F3");
			config.SetValue("keybinding", "key_r", "R");
			config.Save(SETTINGS_FILE_PATH);
		}
		else
		{
			config.Load(SETTINGS_FILE_PATH);
		}
	}
	public static void SaveFpsSetting(double value)
	{
		config.SetValue("FPS", "fps", value);
		config.Save(SETTINGS_FILE_PATH);
	}

	public static int LoadFpsSettings()
	{
		config.Load(SETTINGS_FILE_PATH);
		var res = config.GetValue("FPS","fps").AsInt32();
		return res;
	}
	public static void SaveAudioSetting(string key, double value)
	{
		config.SetValue("Audio", key, value);
		config.Save(SETTINGS_FILE_PATH);
	}

	public static Dictionary<string,double> LoadAudioSettings()
	{
		var audioSett = new Dictionary<string,double>();
		config.Load(SETTINGS_FILE_PATH);
		var keys = config.GetSectionKeys("Audio");
		if (keys!=null)
		{
			foreach (string key in keys)
			{
				audioSett[key] = (double)config.GetValue("Audio", key);
			}
		}
		return audioSett;
	}

	public static void SaveKeybinding(StringName action, InputEvent @event)
	{
		var eventstr = "";
		if (@event is InputEventKey keyevent)
		{
			eventstr = OS.GetKeycodeString(keyevent.GetKeycodeWithModifiers());
		}
		config.SetValue("keybinding",action,eventstr);
		config.Save(SETTINGS_FILE_PATH);
	}

	public static Dictionary<string,object> LoadKeybinding()
	{
		var keybinding = new Dictionary<string, object>();
		var keys = config.GetSectionKeys("keybinding");
		foreach (var key in keys)
		{
			var eventstr = config.GetValue("keybinding", key);
			InputEventKey inputevent = new InputEventKey();
			inputevent.Keycode = OS.FindKeycodeFromString((string)eventstr);
			keybinding[key] = inputevent;
		}
		return keybinding;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SFXPlayer : AudioStreamPlayer
{
	private const string SoundEffectsPath = "res://Assets/SoundEffects/";

	
	[Signal]
	public delegate bool PlaySFXSignalEventHandler(string soundType);

	
	private static string[] ListFilesInDir(string dirPath, string extension)
	{
		string[] res;
		using var dir = DirAccess.Open(dirPath);
		if (dir != null)
		{
			res = dir.GetFiles().Where(f => f.EndsWith(extension)).ToArray();
		}
		else
		{
			throw new Exception("invalid path");
		}

		return res;
	}
	private static string RandomFilePath(string[] files)
	{
		var rand = new Random();
		return files[rand.Next(files.Length)];
	}
	private string AudioPathGetter(string folderName, string extension = ".wav")
	{
		var res = SoundEffectsPath + folderName + "/" + RandomFilePath(ListFilesInDir(SoundEffectsPath + folderName + "/", extension));
		return res;
	}

	public bool PlaySFX(string soundType)
	{
		string path = AudioPathGetter(soundType);
		Stream = GD.Load<AudioStream>(path);
		Play();
		return true;
	}

	public override void _Ready()
	{
		MaxPolyphony = 20;
		PlaySFXSignal += PlaySFX;
		
	}

	public void ChangeSoundEffectsVolume(float _volume)
	{
		AudioServer.SetBusVolumeDb(1, (float)LinearToDb(_volume));
		
	}
	
	public void ChangeMusicVolume(float _volume)
	{
		AudioServer.SetBusVolumeDb(2, (float)LinearToDb(_volume));
		
	}
	
	
	public double LinearToDb(double linear) 
	{
		if (linear <= 0)
		{
			return -80; // -80 dB is typically the minimum dB level in many audio systems
		}
		return 20 * Math.Log10(linear);
	}
	
	
	
	
}

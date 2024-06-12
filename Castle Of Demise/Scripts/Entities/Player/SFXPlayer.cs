using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SFXPlayer : AudioStreamPlayer
{
	private const string SoundEffectsPath = "res://Assets/SoundEffects/";
	private static int _generalVolumePercentage = 100;
	private static bool _muted = false;
	
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
		GD.Print(res);
		return res;
	}

	private class SoundControl
	{
		public void ChangeVolume(int percentage)
		{
			_generalVolumePercentage = percentage;
		}
	
		public void Mute(bool mute)
		{
			_muted = mute;
		}
	}


	public bool PlaySFX(string soundType)
	{
		string path = AudioPathGetter(soundType);
		Play();
		return true;
	}

	public override void _Ready()
	{
		PlaySFXSignal += PlaySFX;
	}
	
	
	
	
}

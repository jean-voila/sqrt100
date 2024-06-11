using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SFXPlayer : AudioStreamPlayer
{
	private int GeneralVolumePercentage = 100;
	private bool Muted = false;
	
	
	private static string[] ListFilesInDir(string dirPath, string extension)
	{
		var res = new string[] { };
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
	private static string _soundEffectsPath = "res://Assets/SoundEffects/";

	private static string CompletePath(string path)
	{
		return _soundEffectsPath + path;
	}

	private class AudioPathGetter
	{
		public string GunShotSound()
		{
			var res = CompletePath(RandomFilePath(ListFilesInDir(CompletePath("GunShotSounds/"), ".wav")));
			GD.Print(res);
			return res;
		}

		public string CantShootSound()
		{
			return CompletePath("CantShoot.wav");
		}
		
	}

	public bool PlaySFX(string soundType, int volumePercentage = 100)
	{
		var pathGetter = new AudioPathGetter();
		switch (soundType)
		{
			case "gunshot":
				Stream = GD.Load<AudioStream>(pathGetter.GunShotSound());
				GD.Print("Playing gunshot sound");
				break;

			case "cantshoot":
				Stream = GD.Load<AudioStream>(pathGetter.CantShootSound());
				break;
			
			default:
				return false;
		}
		Play();
		return true;

	}
	
	public void ChangeVolume(int percentage)
	{
		GeneralVolumePercentage = percentage;
	}
	
	public void Mute(bool mute)
	{
		Muted = mute;
	}
	
	
}

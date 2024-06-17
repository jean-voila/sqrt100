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
	private string AudioPathGetter(string folderName, string extension = ".import")
	{
		var res = SoundEffectsPath + folderName + "/" + RandomFilePath(ListFilesInDir(SoundEffectsPath + folderName + "/", extension));
		return res;
	}

	public bool PlaySFX(string soundType)
	{
		string path = AudioPathGetter(soundType).Replace(".import", "");
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
	
    
    
    
    
    
    
    
    /*
     * Afin de pouvoir gérer uniformément les effets sonores du jeu, nous avons pris le parti d’implémenter, pour chaque nœud pouvant émettre un son, un nœud AudioStreamPlayer lié à une classe SFXPlayer, que nous avons créé.
       
       La classe SFXPlayer permet de lier l’objet parent à un fichier audio, de le jouer et de gérer le volume des effets sonores.
       
       Cette centralisation des effets sonores permet de simplifier la gestion des sons dans le jeu, en évitant de devoir charger les fichiers audio par chemin absolu à chaque fois que l’on souhaite jouer un son.
       
       La méthode PlaySFX permet de jouer un son en fonction de son type, en choisissant aléatoirement un fichier audio parmi ceux disponibles dans le dossier correspondant.
       La musique est gérée de manière similaire, avec un nœud AudioStreamPlayer dédié à la musique et assignée à son bus audio correspondant.

     */
	
	
	
}

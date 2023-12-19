using Godot;
using System;
using System.Collections.Generic;
public partial class Player
{

    private List<AudioStreamPlayer3D> _gunShotSounds;
    private List<AudioStreamPlayer3D> _stepSounds;
    private List<AudioStreamPlayer3D> _allSoundEffects;
    private AudioStreamPlayer3D _hitSound;
    private AudioStreamPlayer3D _jumpSound;
    private AudioStreamPlayer3D _alternateShotSound;
    private AudioStreamPlayer3D _cantShootSound;
    private AudioStreamPlayer3D _landSound;
    private String _musicPlayerPath;
    private AudioStreamPlayer2D _musicPlayer;
    private bool _SEEnabled;

    public void _audioInit()
    {
        
        _alternateShotSound = GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/JeansMod");
        _cantShootSound = GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/CantShoot");
        _hitSound = GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/Hit");
        _gunShotSounds = new List<AudioStreamPlayer3D>
        {
            GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot01"),
            GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot02"),
            GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot03"),
        };
        _stepSounds = new List<AudioStreamPlayer3D>
        {
            GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step01"),
            GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step02"),
            GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step03"),
            GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step04"),
            GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step05")
        };
        _jumpSound = GetNode<AudioStreamPlayer3D>("jumpAndLandSoundEffect/jump");
        _landSound = GetNode<AudioStreamPlayer3D>("jumpAndLandSoundEffect/Land");
        _allSoundEffects = new List<AudioStreamPlayer3D>();
        _allSoundEffects.AddRange(_gunShotSounds);
        _allSoundEffects.AddRange(_stepSounds);
        _allSoundEffects.Add(_jumpSound);
        _allSoundEffects.Add(_landSound);
        _allSoundEffects.Add(_alternateShotSound);
        _allSoundEffects.Add(_hitSound);
        _musicPlayerPath = "BackGroundMusic";
        _musicPlayer = GetNode<AudioStreamPlayer2D>(_musicPlayerPath);
        _SEEnabled = true;
        
    }
    
    
    public void ChangeMusicVolume(float percentage)
    {

        percentage = Mathf.Clamp(percentage, 0, 100);
        float volumeDb = Mathf.Lerp(-40, 0, percentage / 100.0f);
        _musicPlayer.VolumeDb = volumeDb;
    }
    
    public void SwitchSEPlayer(bool playingButton)
    {
        _SEEnabled = playingButton;
    }
    
    public void SwitchMusicPlayer(bool playingButton)
    {
        _musicPlayer.Playing = playingButton;

    }
    
    public void ChangeSoundEffectsVolume(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 100);
        float volumeDb = Mathf.Lerp(-40, 0, percentage / 100.0f);
        foreach (var soundEffect in _allSoundEffects)
        {
            soundEffect.MaxDb = volumeDb;
        }
    }
}

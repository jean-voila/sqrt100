using System.Diagnostics;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player : CharacterBody3D
{
	[Export] private AudioStreamPlayer _sfxPlayer;
	[Export] private Node3D _head;
	[Export] private float _maxFov = 1.15f;
	[Export] private float _fovChangingSpeed = 17f;
	[Export] private float _camRotationAmount = 0.1f;
	[Export] private float _mouseSensitivity = 0.005f;
	[Export] private float _shakeDelay = 10f;
	[Export] private float _shakeStrength = 0.2f;
	[Export] private Node3D _usedCamera;
	[Export] private AnimationPlayer _animReload;
	[Export] private MeshInstance3D _revolverModel;
	[Export] private float _revolverModelRotationAmount = -0.3f;
	public  string PlayerName { get; set; }
	public  int PlayerId { get; set; }
	public  int PlayerScore { get; set; }
	private Vector2 _lastMouseMovement;
	public static bool IsMultiplayer = false;
	
	public override void _Ready()
	{
		GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(PlayerId);
		GD.Print("Authority set to" + GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority());
		GD.Print("Multiplayer Authority: " + PlayerId);
		GD.Print($" Player ID: {PlayerId}, Player Name: {PlayerName}, Player Score: {PlayerScore}");

		GD.Print(Multiplayer.GetUniqueId());
		_shootInit();
		_stepsInit();
		_pauseMenuInit();
		_graphismsInit();
		_gameWindowInit();
		_playerHealthInit();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseInput)
		{
			HandleMouseMovement(mouseInput);
			_lastMouseMovement = mouseInput.Relative;
		}
		if (@event.IsActionPressed("mouse_left_click"))
		{
			if (_ammoInMag > 0) Shoot();
			else _sfxPlayer.EmitSignal("PlaySFXSignal", "gun/cantshoot");
		}
		if (Input.IsActionJustPressed("key_escape"))
			Pause();
		if (@event.IsActionPressed("key_r") && canReload())
		{
			_sfxPlayer.EmitSignal("PlaySFXSignal", "gun/reload");
			_animReload.Play("reload");
			reload();
		}
		if (@event.IsActionPressed("key_r") && outOfAmmo())
		{
			_sfxPlayer.EmitSignal("PlaySFXSignal", "gun/cantshoot");
		}
	}
	public override void _PhysicsProcess(double d)
	{
		// il faut un truc pour check si les gens controlent pas la mm personne?
		if (IsMultiplayer && GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
		{
			HandleMouseMovementInputs((float)d);
			HandleMovements(d);
			HandleRespawn();
			UpdateDebugInfo();
			UpdatePlayerInfo();
			CameraShakeProcess();
			WeaponSway();
		}
		if (!IsMultiplayer)
		{
			HandleMouseMovementInputs((float)d);
			HandleMovements(d);
			HandleRespawn();
			UpdateDebugInfo();
			UpdatePlayerInfo();
			CameraShakeProcess();
			WeaponSway();

		}
		/*
		GD.Print("===== RAPPORT =====");
		GD.Print(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
		GD.Print(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority().ToString());
		GD.Print(Multiplayer.GetUniqueId());
		GD.Print("===== ENDOF =====");
		*/
	}

	public override void _EnterTree()
	{
		// RpcId(1, "SetnetworkMaster", PlayerId);
	}


	[Export]
	private int _dummyExport; // Dummy exported variable to call the private method from the editor

	private void SetNetworkMaster(int newMaster)
	{
		// This method would be called on the peer with ID 1, changing the network master of the node
	}
	
	
	
													
}
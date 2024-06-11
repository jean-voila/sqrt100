using System.Diagnostics;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player : CharacterBody3D
{
	
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
	public string PlayerName = "";
	public int PlayerId = 0;
	public int PlayerScore = 0;
	private Vector2 _lastMouseMovement;
	
	public override void _Ready()
	{
		_audioInit();
		_shootInit();
		_stepsInit();
		_pauseMenuInit();
		_graphismsInit();
		_gameWindowInit();
		_playerHealthInit();
		Engine.MaxFps = 60;
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
			else _cantShootSound.Play();
		}
		if (Input.IsActionJustPressed("key_escape"))
			Pause();
		if (@event.IsActionPressed("key_r") && canReload())
		{
			_weaponReload.Play();
			_animReload.Play("reload");
			reload();
		}
		if (@event.IsActionPressed("key_r") && outOfAmmo())
		{
			_cantShootSound.Play();
		}
	}
	public override void _PhysicsProcess(double d)
	{
		// il faut un truc pour check si les gens controlent pas la mm personne?
		HandleMouseMovementInputs((float)d);
		HandleMovements(d);
		HandleRespawn();
		UpdateDebugInfo();
		UpdatePlayerInfo();
		CameraShakeProcess();
		WeaponSway();
	}

	public override void _EnterTree()
	{
		RpcId(1, "SetnetworkMaster", PlayerId);
	}


	[Export]
	private int _dummyExport; // Dummy exported variable to call the private method from the editor

	private void SetNetworkMaster(int newMaster)
	{
		// This method would be called on the peer with ID 1, changing the network master of the node
	}
	
	
	
													
}
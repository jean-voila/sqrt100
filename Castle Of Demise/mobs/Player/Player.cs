using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player : CharacterBody3D
{
	
	[Export] private Node3D _head;
	
	
	[Export] private float _maxFov = 1.15f;
	[Export] private float _fovChangingSpeed = 17f;
	[Export] private float _camRotationAmount = 0.1f;
	[Export] private float _mouseSensitivity = 0.005f;
	[Export] private float _shakeDelay = 300.0f;
	[Export] private float _shakeStrength;
	[Export] private Node3D _usedCamera;
	[Export] private AnimationPlayer _animReload;
	
	public override void _Ready()
	{
		_audioInit();
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
			HandleMouseMovement(mouseInput);
		if (@event.IsActionPressed("mouse_left_click"))
		{
			if (_ammoInMag > 0) Shoot();
			else _cantShootSound.Play();
		}
		if (Input.IsActionJustPressed("key_escape"))
			Pause();
		if (@event.IsActionPressed("key_r") && canReload())
		{
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
	}

	public override void _EnterTree()
	{
		RpcId(1, "SetNetworkMaster", int.Parse(Name));
	}

	[Export]
	private int _dummyExport; // Dummy exported variable to call the private method from the editor

	private void SetNetworkMaster(int newMaster)
	{
		// This method would be called on the peer with ID 1, changing the network master of the node
	}
	
		
}
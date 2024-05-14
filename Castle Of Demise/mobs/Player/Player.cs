using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player : CharacterBody3D
{
	
	[Export] private Node3D _head;
	[Export] private static Node3D _camera;
	[Export] private static float _originalFov = 1.0f;
	[Export] private float _maxFov = 1.15f;
	[Export] private float _fovChangingSpeed = 9.0f;
	[Export] private float _camRotationAmount = 0.1f;
	[Export] private float _mouseSensitivity = 0.005f;
	[Export] private float _shakeDelay = 300.0f;
	[Export] private float _shakeStrength;
	[Export] private string _version = "3.0";
	public override void _Ready()
	{
		_audioInit();
		_shootInit();
		_stepsInit();
		_pauseMenuInit();
		_graphismsInit();
		_gameWindowInit();
		_jeansModInit();
		
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseInput) 
			HandleMouseMovement(mouseInput);
		if (@event.IsActionPressed("mouse_left_click"))
		{
			if (_ammoAvailable > 0) Shoot();
			else _cantShootSound.Play();
		}
		if (Input.IsActionJustPressed("key_escape"))
			Pause();
	}
	public void _PhysicsProcess(float delta)
	{
		HandleMouseMovementInputs(delta);
		HandleJump();
		HandleMovements(delta);
		HandleRespawn();
		UpdateDebugInfo();
		UpdatePlayerInfo();
		CameraShakeProcess();
	}
}
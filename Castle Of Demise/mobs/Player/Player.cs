using Godot;
using System;
using System.Collections.Generic;


public class Player : KinematicBody
{
	private RayCast _shootRayCast;
	private PackedScene _bulletHoleScene;
	
	private List<AudioStreamPlayer3D> _gunShotSounds;
	private List<AudioStreamPlayer3D> _stepSounds;
	private List<AudioStreamPlayer3D> _allSoundEffects;

	private AudioStreamPlayer3D _jumpSound;
	private AudioStreamPlayer3D _alternateShotSound;
	private AudioStreamPlayer3D _landSound;
	private RayCast _floorRayCast;
	private Timer _stepTimer;
	
	private float _originalFOV;
	private float _targetFOV;

	public float PositionX;
	public float PositionY;
	public float PositionZ;

	public float AccelerationX;
	public float AccelerationY;
	public float AccelerationZ;

	public float OrientationX;
	public float OrientationY;

	[Export]
	private string _version = "1.0.3";

	[Export] private bool _jeansModEnabled = false;

	[Export] 
	private bool _musicPlayingByDefault = false;
	[Export]
	public float CamRotationAmount = 0.1f;
	[Export]
	private float _accelerationSpeed = 4.5f;
	[Export]
	private float _decelerationSpeed = 4.5f;
	[Export]
	private float _maxSpeed = 20f;
	[Export]
	private float _gravity = 60f;
	[Export]
	private float _jumpSpeed = 20f;
	[Export]
	private float _mouseSensitivity = 0.005f;
	[Export] 
	private float _maxFov = 1.15f;
	[Export]
	private float _FOVChangeSpeed = 9.0f;


	private Vector3 _direction = new Vector3();
	private Vector3 _velocity = new Vector3();

	public int BulletCount = 0;

	[Export]
	private NodePath _headNodePath;
	private Spatial _head;

	[Export]
	private NodePath _cameraNodePath;
	private Spatial _camera;
	private Camera _cameraForFOV;
	
	[Export]
	private string _pauseMenuPath;
	private Control _pauseMenu;

	private String _pixelShaderPath;
	private MeshInstance _pixelShader;

	private String _musicPlayerPath;
	private AudioStreamPlayer2D _musicPlayer;

	public float Fps;
	public ulong LastJumpTime = Time.GetTicksUsec();
	public bool Landed = true;

	public ulong _lastPauseTime;
	public bool _pixelShaderEnabled;
	public bool _musicPlaying;

	private float _lastMusicDb;
	private bool _SEEnabled;
	
	

	private void RotateCamera(float inputX, float delta)
	{
		if (_camera != null)
		{

			_camera.Rotation = new Vector3(
				_camera.Rotation.x,
				_camera.Rotation.y,
				Mathf.Lerp(_camera.Rotation.z, -inputX * CamRotationAmount, 10 * delta)
			);
		}
	}

	public override void _Ready()
	{
		_shootRayCast = GetNode<RayCast>("Head/Camera/RayCast");

		_gunShotSounds = new List<AudioStreamPlayer3D>
		{
			GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot01"),
			GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot02"),
			GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/GunShot03")
		};

		_alternateShotSound = GetNode<AudioStreamPlayer3D>("GunShotSoundsEffects/JeansMod");

		_stepSounds = new List<AudioStreamPlayer3D>
		{
			GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step01"),
			GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step02"),
			GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step03"),
			GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step04"),
			GetNode<AudioStreamPlayer3D>("StepSoundsEffetcs/Step05")
		};

		_stepTimer = GetNode<Timer>("StepSoundsEffetcs/TimerBetweenStep");

		_jumpSound = GetNode<AudioStreamPlayer3D>("jumpAndLandSoundEffect/jump");
		_landSound = GetNode<AudioStreamPlayer3D>("jumpAndLandSoundEffect/Land");
		
		_allSoundEffects = new List<AudioStreamPlayer3D>();
		
		_allSoundEffects.AddRange(_gunShotSounds);
		_allSoundEffects.AddRange(_stepSounds);
		_allSoundEffects.Add(_jumpSound);
		_allSoundEffects.Add(_landSound);
		_allSoundEffects.Add(_alternateShotSound);



		_floorRayCast = GetNode<RayCast>("checkFloor");

		_head = GetNode<Spatial>("Head");
		_camera = GetNode<Spatial>(_cameraNodePath);
		_cameraForFOV = GetNode<Camera>(_cameraNodePath);
		_pauseMenuPath = "CanvasLayer2/MenuPause";
		_pauseMenu = GetNode<Control>(_pauseMenuPath);
		_originalFOV = _cameraForFOV.Fov;
		_targetFOV = _originalFOV;
		_pauseMenu.Visible = false;
		_lastPauseTime = Time.GetTicksMsec();
		_pixelShaderEnabled = true;
		_pixelShaderPath = "Head/Camera/PixeliseShader";
		_pixelShader = GetNode<MeshInstance>(_pixelShaderPath);
		_musicPlaying = _musicPlayingByDefault;
		_musicPlayerPath = "EasterEgg";
		_musicPlayer = GetNode<AudioStreamPlayer2D>(_musicPlayerPath);
		_SEEnabled = true;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouseInput = (InputEventMouseMotion) @event;
			RotateY(-mouseInput.Relative.x * _mouseSensitivity);
			_head.RotateX(-mouseInput.Relative.y * _mouseSensitivity);
		}

		if (@event.IsActionPressed("mouse_left_click"))
		{
			Shoot();
		}
		
		if (Input.IsActionJustPressed("key_escape"))
		{
			Pause();
		}
	}

	private void Shoot()
	{
		var rayEnd = _shootRayCast.GetCollisionPoint();
		_bulletHoleScene = GD.Load<PackedScene>("res://Assets/Effects/BulletHole/BulletHoleScene.tscn");

		if (_shootRayCast.IsColliding())
		{
			var bulletHole = (Spatial)_bulletHoleScene.Instance();
			var hitObject = _shootRayCast.GetCollider() as Spatial;

			if (hitObject != null)
			{
				hitObject.AddChild(bulletHole);
				bulletHole.GlobalTransform = new Transform(bulletHole.GlobalTransform.basis, rayEnd);
				bulletHole.LookAt(rayEnd + _shootRayCast.GetCollisionNormal() + new Vector3(0.01f, 0.01f, 0.01f), Vector3.Up);
				bulletHole.GetNode<CPUParticles>("CPUParticles").Restart();
				BulletCount++;
			}
		}

		if (_SEEnabled)
		{
			if (!_jeansModEnabled)
			{
				var randomGunShot = new Random().Next(0, _gunShotSounds.Count);
				_gunShotSounds[randomGunShot].PitchScale = new Random().Next(1, 2);
				_gunShotSounds[randomGunShot].Play();
			}
			else
			{
				_alternateShotSound.Play();
			}
		}


	}

	public override void _PhysicsProcess(float delta)
	{
		Fps = 1 / delta;

		var inputMovementVector = new Vector2();
		bool isMoving = false;
		

		if (Input.IsActionPressed("key_z"))
		{
			inputMovementVector.y += 1;
			isMoving = true;
		}

		if (Input.IsActionPressed("key_s"))
		{
			inputMovementVector.y -= 1;
			isMoving = true;
		}

		if (Input.IsActionPressed("key_q"))
		{
			inputMovementVector.x -= 1;
			isMoving = true;
		}

		if (Input.IsActionPressed("key_d"))
		{
			inputMovementVector.x += 1;
			isMoving = true;
		}

		HandleStepSounds(isMoving);

		_direction = new Vector3();
		_direction += -GlobalTransform.basis.z * inputMovementVector.y;
		_direction += GlobalTransform.basis.x * inputMovementVector.x;

		HandleJump();

		var horizontalVelocity = _velocity;
		horizontalVelocity.y = 0;

		var target = _direction * _maxSpeed;
		var acceleration = (_direction.Dot(horizontalVelocity) > 0) ? _accelerationSpeed : _decelerationSpeed;

		horizontalVelocity = horizontalVelocity.LinearInterpolate(target, acceleration * delta);

		_velocity.x = horizontalVelocity.x;
		_velocity.z = horizontalVelocity.z;

		_velocity.y -= delta * _gravity;

		_velocity = MoveAndSlide(_velocity, Vector3.Up);
		RotateCamera(inputMovementVector.x, delta);
		AdjustFOV(isMoving, delta);
		
		if (PositionY < -90)
		{
			Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
			Teleport(newCoordinates);
		}
		UpdatePlayerInfo();
	}

	private void HandleStepSounds(bool isMoving)
	{
		if (isMoving && _SEEnabled && _floorRayCast.GetCollider() != null && _stepTimer.TimeLeft <= 0)
		{
			var randomStep = new Random().Next(0, _stepSounds.Count);
			_stepSounds[randomStep].PitchScale = new Random().Next(1, 5);
			_stepSounds[randomStep].Play();
			_stepTimer.Start(0.2f);
		}
	}

	private void HandleJump()
	{
		if (_floorRayCast.GetCollider() == null) return;

		if (!Landed && Time.GetTicksUsec() - LastJumpTime > 10)
		{
			Landed = true;
			if (_SEEnabled) _landSound.Play();
		}

		if (_floorRayCast.GetCollider() != null && Input.IsActionPressed("key_space"))
		{
			LastJumpTime = Time.GetTicksUsec();
			_velocity.y = _jumpSpeed;
			
			if (_SEEnabled) _jumpSound.Play();
			Landed = false;
		}
	}


	private void UpdatePlayerInfo()
	{
		var textLeftHUD = GetNode<RichTextLabel>("CanvasLayer/HUD/Textes/HUDGauche");
		var textRightHUD = GetNode<RichTextLabel>("CanvasLayer/HUD/Textes/HUDDroite");
		
		var transform = GlobalTransform;
		var position = transform.origin;
		PositionX = position.x;
		PositionY = position.y;
		PositionZ = position.z;

		var acceleration = _velocity;
		AccelerationX = acceleration.x;
		AccelerationY = acceleration.y;
		AccelerationZ = acceleration.z;


		var cameraTransform = _cameraForFOV.GlobalTransform;
		var cameraBasis = cameraTransform.basis;
		var cameraEulerAngles = cameraBasis.GetEuler();

		OrientationX = cameraEulerAngles.x;
		OrientationY = cameraEulerAngles.y;

		var leftText = GenerateLeftHUDText();
		textLeftHUD.BbcodeText = leftText;

		var rightText = GenerateRightHUDText();
		textRightHUD.BbcodeText = rightText;
	}

	private string GenerateLeftHUDText()
	{
		return
			$"{Title("Position")}" +
			$"{HUDData("posX", PositionX)}" +
			$"{HUDData("posY", PositionY)}" +
			$"{HUDData("posZ", PositionZ)}" +

			$"{Title("Acceleration")}" +
			$"{HUDData("accX", AccelerationX)}" +
			$"{HUDData("accY", AccelerationY)}" +
			$"{HUDData("accZ", AccelerationZ)}" +

			$"{Title("Orientation")}" +
			$"{HUDData("oriX", OrientationX)}" +
			$"{HUDData("oriY", OrientationY)}" +

			$"{Title("Other")}" +
			$"{HUDData("Bullet Count", BulletCount)}";
	}

	private string GenerateRightHUDText()
	{
		return
			$"{Title("Game Version")}" +
			$"{HUDData("CoDem", _version)}" +

			$"{Title("Execution Info")}" +
			$"{HUDData("FPS", Fps)}";
	}

	private string HUDData(string name, object value, bool newLine = true)
	{
		var newLineStr = newLine ? "\n" : "";
		var space = new string(' ', 22 - name.Length - $"{value}".Length - 2);
		return $" {Red(name)}{space}{value} {newLineStr}";
	}

	private string Title(string name)
	{
		var equals = new string('=', ((22 - name.Length) / 2) + 1);
		return $"|{equals} {name.ToUpper()} {equals}|\n";
	}

	private string Red(string text)
	{
		return $"[color=red]{text}[/color]";
	}

	public void Pause()
	{
		_pauseMenu.Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().Paused = true;
		_lastPauseTime = Time.GetTicksMsec();
	}

	public void UnPause()
	{
		if (Time.GetTicksMsec() - _lastPauseTime > 10)
		{
			_pauseMenu.Visible = false;
			GetTree().Paused = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

	}

	public void SwitchPixelShader()
	{
		_pixelShaderEnabled = !_pixelShaderEnabled;
		_pixelShader.Visible = _pixelShaderEnabled;
	}

	public void SwitchMusicPlayer(bool playingButton)
	{
		_musicPlayer.Playing = playingButton;

	}
	
	public void SwitchSEPlayer(bool playingButton)
	{
		_SEEnabled = playingButton;

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
	
	public void ChangeMusicVolume(float percentage)
	{

		percentage = Mathf.Clamp(percentage, 0, 100);
		float volumeDb = Mathf.Lerp(-40, 0, percentage / 100.0f);
		_musicPlayer.VolumeDb = volumeDb;
	}
	
	
	private void Teleport(Vector3 newPosition)
	{
		Transform newTransform = Transform.Identity;
		newTransform.origin = newPosition;
		GlobalTransform = newTransform;
	}
	
	private void AdjustFOV(bool isMoving, float delta)
	{
		if (Input.IsActionPressed("key_z") && _floorRayCast.GetCollider() != null)
		{
			_targetFOV = Mathf.Lerp(_targetFOV, _originalFOV * _maxFov, _FOVChangeSpeed * delta);
		}
		else
		{
			_targetFOV = Mathf.Lerp(_targetFOV, _originalFOV, _FOVChangeSpeed * delta);
		}

		_cameraForFOV.Fov = _targetFOV;
	}

	public void SwitchJeansMod(bool value)
	{
		_jeansModEnabled = value;
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
	
	
	


	

}

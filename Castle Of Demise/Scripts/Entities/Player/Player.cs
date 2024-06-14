using System.Diagnostics;
using CastleOfDemise.Scripts.Menus.MultiLauncher;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player : CharacterBody3D
{
	[Signal]
	public delegate void SwitchCinematicModeSignalEventHandler(bool value);
	[Signal]
	public delegate void SwitchLevitationModeSignalEventHandler(bool value);
	
	
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
	[Export] private CanvasLayer _pausecanvas;
	[Export] private CanvasLayer _HUDCanvas;

	private bool _cinematicMode;
	private bool _levitationMode;

	public string PlayerName = "";
	public int PlayerId = 0;
	public int PlayerScore = 0;
	public bool IsServer = false;
	private Vector2 _lastMouseMovement;
	public static bool IsMultiplayer = false;
	
	// cleaner synchronisation
	private Vector3 _syncPos = new Vector3(0, 0, 0);  // position
	//private Vector3 _syncRotation = new Vector3(0, 0, 0); //rotation du perso
	
	public override void _Ready()
	{
		SwitchCinematicModeSignal += SwitchCinematicMode;
		SwitchLevitationModeSignal += SwitchLevitationMode;
		AddToGroup("Player");
		_shootInit();
		_stepsInit();
		_pauseMenuInit();
		_graphismsInit();
		_gameWindowInit();
		_playerHealthInit();

		if (!IsMultiplayer)
		{
			_HUDCanvas.Show();
			_pausecanvas.Show();
		}
		else
		{
			GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));

		}
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

		switch(_cinematicMode)
		{
			case true:
				_HUDCanvas.Hide();
				break;
			case false:
				_HUDCanvas.Show();
				break;
			
		}

		if (IsMultiplayer)
		{
			if (Multiplayer.MultiplayerPeer != null 
			    && GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId()
			   )
			{
				HandleMouseMovementInputs((float)d);
				HandleMovements(d);
				HandleRespawn();
				UpdateDebugInfo();
				UpdatePlayerInfo();
				CameraShakeProcess();
				WeaponSway();
				this._HUDCanvas.Show();
				this._pausecanvas.Show();
				this.CameraForFov.Current = true;
				/*
				 * this.CameraForFov.Current = true;
				   this._HUD.Show();
				   this._pauseHUD.Show();
				 */

				
				// synchronisation of players
				_syncPos = GlobalPosition;
				//_syncRotation = GetNode<Node3D>("rotation").RotationDegrees;
			}
			else
			{
				GlobalPosition = GlobalPosition.Lerp(_syncPos, 0.1f);
				//GetNode<Node3D>("rotation").RotationDegrees = RotationDegrees.Lerp(_syncRotation, 0.1f);
			}


		}
		
		else
		{
			HandleMouseMovementInputs((float)d);
			HandleMovements(d);
			HandleRespawn();
			UpdateDebugInfo();
			UpdatePlayerInfo();
			CameraShakeProcess();
			WeaponSway();
		}
		
		
	}



	private void SendMultiplayerAuthorityReport()
	{
		GD.Print("");
		GD.Print("===== RAPPORT =====");
		GD.Print("The session is " + PlayerName);
		GD.Print("Are they both equal? ::" + (GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId()));
		GD.Print("The authority is "+ GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority().ToString());
		GD.Print("The Id is " + Multiplayer.GetUniqueId());
		GD.Print("===== END OF =====");
		GD.Print("");

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

	private void SwitchCinematicMode(bool _value)
	{
		_cinematicMode = _value;
	}

	private void SwitchLevitationMode(bool _value)
	{
		_levitationMode = _value;
	}
	
	
	
													
}
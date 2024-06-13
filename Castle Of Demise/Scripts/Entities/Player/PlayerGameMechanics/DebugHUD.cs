using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
	private float _fps;
	
	private void UpdateDebugInfo()
	{
		_fps = (float)Engine.GetFramesPerSecond();
		var textLeftHud = GetNode<RichTextLabel>("HUD/Debug/Textes/HUDGauche");
		var textRightHud = GetNode<RichTextLabel>("HUD/Debug/Textes/HUDDroite");
    		
		var transform = GlobalTransform;
		var position = transform.Origin;
		_positionX = position.X;
		_positionY = position.Y;
		_positionZ = position.Z;
    
		var acceleration = Velocity;
		_accelerationX = acceleration.X;
		_accelerationY = acceleration.Y;
		_accelerationZ = acceleration.Z;
    
    
		var cameraTransform = ((Camera3D)_usedCamera).GlobalTransform;
		var cameraBasis = cameraTransform.Basis;
		var cameraEulerAngles = cameraBasis.GetEuler();
    
		_orientationX = cameraEulerAngles.X;
		_orientationY = cameraEulerAngles.Y;
    
		var leftText = GenerateLeftHudText();
		textLeftHud.Text = leftText;
    
		var rightText = GenerateRightHudText();
		textRightHud.Text = rightText;
		    
		
	}
    	
    	
    
	private string GenerateLeftHudText()
	{
		return
			$"{Title("Position")}" +
			$"{HudData("posX", _positionX)}" +
			$"{HudData("posY", _positionY)}" +
			$"{HudData("posZ", _positionZ)}" +

			$"{Title("Acceleration")}" +
			$"{HudData("accX", _accelerationX)}" +
			$"{HudData("accY", _accelerationY)}" +
			$"{HudData("accZ", _accelerationZ)}" +

			$"{Title("Orientation")}" +
			$"{HudData("oriX", _orientationX)}" +
			$"{HudData("oriY", _orientationY)}" +

			$"{Title("Other")}" +
			$"{HudData("Bullet Count", _ammoShooted)}" +
			$"{HudData("Strength", _strength)}" +
			$"{HudData("Killed", _killedEnemmies)}" +

			$"{Title("Multiplayer")}" +
			$"{HudData("IsMultiplayer", Player.IsMultiplayer)}" +
			$"{HudData("ServerSide", Player.IsMultiplayer ? ((PlayerId == 1) ? "Server" : "Client") : "None")}";
	}
    
	private string GenerateRightHudText()
	{
		return
			$"{Title("Game Version")}" +
			$"{HudData("Version", "")}" +

			$"{Title("Execution Info")}" +
			$"{HudData("FPS", _fps)}";

	}
    
	private string HudData(string name, object value, bool newLine = true)
	{
		var newLineStr = newLine ? "\n" : "";
		var space = new string(' ', 22 - name.Length - $"{value}".Length - 2);
		return $" {Red(name)}{space}{value} {newLineStr}";
	}
	    
	    
}
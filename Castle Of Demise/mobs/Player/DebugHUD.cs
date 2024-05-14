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
		PositionX = position.X;
		PositionY = position.Y;
		PositionZ = position.Z;
    
		var acceleration = _velocity;
		AccelerationX = acceleration.X;
		AccelerationY = acceleration.Y;
		AccelerationZ = acceleration.Z;
    
    
		var cameraTransform = CameraForFov.GlobalTransform;
		var cameraBasis = cameraTransform.Basis;
		var cameraEulerAngles = cameraBasis.GetEuler();
    
		OrientationX = cameraEulerAngles.X;
		OrientationY = cameraEulerAngles.Y;
    
		var leftText = GenerateLeftHudText();
		textLeftHud.Text = leftText;
    
		var rightText = GenerateRightHudText();
		textRightHud.Text = rightText;
		    

	}
    	
    	
    
	private string GenerateLeftHudText()
	{
		return
			$"{Title("Position")}" +
			$"{HudData("posX", PositionX)}" +
			$"{HudData("posY", PositionY)}" +
			$"{HudData("posZ", PositionZ)}" +
    
			$"{Title("Acceleration")}" +
			$"{HudData("accX", AccelerationX)}" +
			$"{HudData("accY", AccelerationY)}" +
			$"{HudData("accZ", AccelerationZ)}" +
    
			$"{Title("Orientation")}" +
			$"{HudData("oriX", OrientationX)}" +
			$"{HudData("oriY", OrientationY)}" +
    
			$"{Title("Other")}" +
			$"{HudData("Bullet Count", _ammoShooted)}"+
			$"{HudData("Strength", _strength)}" +
			$"{HudData("Killed", _killedEnemmies)}";
	}
    
	private string GenerateRightHudText()
	{
		return
			$"{Title("Game Version")}" +
			$"{HudData("CoDem", _version)}" +

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
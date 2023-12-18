using Godot;

public partial class Player
{
	public float Fps;
	
    private void UpdateDebugInfo()
    	{
		    Fps = Engine.GetFramesPerSecond();
    		var textLeftHUD = GetNode<RichTextLabel>("HUD/Debug/Textes/HUDGauche");
    		var textRightHUD = GetNode<RichTextLabel>("HUD/Debug/Textes/HUDDroite");
    		
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
    			$"{HUDData("Bullet Count", _ammoShooted)}";
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
}
using Godot;
using System;
using System.Runtime.Remoting.Messaging;

public class Player : KinematicBody{
	[Export]
	float accelerationSpeed = 4.5f;
	[Export]
	float decelerationSpeed = 4.5f;
	[Export]
	float maxSpeed = 30f;
	[Export]
	float gravity = 60f;
	[Export]
	float jumpSpeed = 20f;

	[Export]
	float mouse_sensitivity = 0.005f;

	Vector3 direction = new Vector3();
	Vector3 velocity = new Vector3();


	[Export]
	NodePath HeadNodePath;
	Spatial Head;

	[Export]
	NodePath CameraNodePath;
	Spatial Camera;

	public override void _Ready(){
		Head = GetNode<Spatial>(HeadNodePath);
		Camera = GetNode<Spatial>(CameraNodePath);
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event){
		if (@event is InputEventMouseMotion){
			InputEventMouseMotion mouseImput = (InputEventMouseMotion) @event;
			RotateY(-mouseImput.Relative.x * mouse_sensitivity);
			Head.RotateX(-mouseImput.Relative.y * mouse_sensitivity);//rotate the head and not the entire body to avoid wrong rotations
		}

		if (Input.IsActionPressed("key_escape")){ //temporaire pour fermer le jeu proprement
			GetTree().Quit();
		}
	}

	public override void _PhysicsProcess(float delta){

		// get the current direction in wich we want to move
		direction = new Vector3();
		var inputMouvementVector = new Vector2();

		if (Input.IsActionPressed("key_z")){
			inputMouvementVector.y += 1;
		}
		if (Input.IsActionPressed("key_s")){
			inputMouvementVector.y -= 1;
		}
		if (Input.IsActionPressed("key_q")){
			inputMouvementVector.x -= 1;
		}
		if (Input.IsActionPressed("key_d")){
			inputMouvementVector.x += 1;
		}

		//adapte the mouvement to the camera orientation
		direction += -GlobalTransform.basis.z * inputMouvementVector.y;//transphorm 2d direction into 3d so "y" becomes "z"
		direction += GlobalTransform.basis.x * inputMouvementVector.x;//and "x" stays "x"
		
		if (! IsOnFloor()){
			velocity.y -= delta * gravity;
		}
			
		if (IsOnFloor() && Input.IsActionPressed("key_space")){
			velocity.y = jumpSpeed;
		}

		//process that direction into actual in game mouvement

		var horizontalVelocity = velocity;
		horizontalVelocity.y = 0;

		var target = direction;
		target *= maxSpeed;

		float acceleration;
		if (direction.Dot(horizontalVelocity) > 0){
			acceleration = accelerationSpeed;
		} else {
			acceleration = decelerationSpeed;
		}

		horizontalVelocity = horizontalVelocity.LinearInterpolate(target, acceleration * delta);

		velocity.x = horizontalVelocity.x;
		velocity.z = horizontalVelocity.z;

		velocity = MoveAndSlide(velocity, Vector3.Up);
		
		Transform transformeeGlobale = GlobalTransform;
        Vector3 position = transformeeGlobale.origin;
		var accel = position;
		float accelX=accel.x;
		float accelY=accel.y;
		float accelZ=accel.z;
		Update(accelX, accelY, accelZ);



		if (accelY<-90){
			Vector3 nouvellesCoordonnees = new Vector3(0.0f, 6.0f, 0.0f);
			Teleporter(nouvellesCoordonnees);
		}
		
		
		
		
	}
	private void Teleporter(Vector3 nouvellePosition)
    {
        Transform nouvelleTransformee = Transform.Identity;
        nouvelleTransformee.origin = nouvellePosition;

        GlobalTransform = nouvelleTransformee;
    }

	public void Update(float aX, float aY, float aZ){
		var texteHUD = GetNode<RichTextLabel>("CanvasLayer/Control/RichTextLabel");
		var texte="";
		
		texte+=$"accelX: {aX}\n";
		texte+=$"accelY: {aY}\n";
		texte+=$"accelZ: {aZ}";
		
		
		texte+="\n";
		texteHUD.SetText(texte);
	}
}

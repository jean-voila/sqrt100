using Godot;

using System;
using System.Runtime.Remoting.Messaging;

public class Player : KinematicBody
{
	private RayCast rayCastShoot;
	private PackedScene bulletHoleScene;
	
	public float posiX;
	public float posiY;
	public float posiZ;

	public float accX;
	public float accY;
	public float accZ;

	public float oriX;
	public float oriY;
		
	[Export]
	string version = "1.0.0";

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

	public float fps;

	public override void _Ready()
	{
		rayCastShoot = GetNode<RayCast>("Head/Camera/RayCast");
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

		if (@event.IsActionPressed("mouse_left_click"))
		{
			var rayEnd = rayCastShoot.GetCollisionPoint();
			GD.Print(rayCastShoot.GetCollider());
			
			
			PackedScene bulletHoleScene = GD.Load<PackedScene>("res://Assets/Effects/BulletHole/BulletHoleScene.tscn");
			var newBulletHoleDecal = bulletHoleScene.Instance();
			if (rayCastShoot.IsColliding())
			{
				Spatial bulletHole = (Spatial)bulletHoleScene.Instance();
				Spatial hitObject = rayCastShoot.GetCollider() as Spatial;
				if (hitObject != null)
				{
					hitObject.AddChild(bulletHole);
					bulletHole.GlobalTransform = new Transform(bulletHole.GlobalTransform.basis, rayEnd);
					bulletHole.LookAt(rayCastShoot.GetCollisionPoint() + rayCastShoot.GetCollisionNormal() + new Vector3(0.01f,0.01f,0.01f),
						Vector3.Up);
				}
			}
		}
		/*if (Input.IsActionPressed("key_escape")){ //temporaire pour fermer le jeu proprement
			GetTree().Quit();
		}*/
	}

	public override void _PhysicsProcess(float delta){
		fps = 1/delta;

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

		
		var camera = GetNode<Camera>("Head/Camera");
		Transform cameraTransform = camera.GlobalTransform;
		Basis cameraBasis = cameraTransform.basis;
		Vector3 cameraEulerAngles = cameraBasis.GetEuler();

		velocity = MoveAndSlide(velocity, Vector3.Up);
		
		Transform transformeeGlobale = GlobalTransform;
        Vector3 position = transformeeGlobale.origin;
		var pos = position;
		 posiX=pos.x;
		 posiY=pos.y;
		 posiZ=pos.z;

		var accel = velocity;
		 accX=accel.x;
		 accY=accel.y;
		 accZ=accel.z;
		 

		var angleCam = cameraEulerAngles;
		 oriX = angleCam.x;
		 oriY = angleCam.y;

		if (posiY<-90){
			Vector3 nouvellesCoordonnees = new Vector3(0.0f, 6.0f, 0.0f);
			Teleporter(nouvellesCoordonnees);
		} // Si le joueur est tombé, le faire re-spawn


		

		Update();

		
	}
	private void Teleporter(Vector3 nouvellePosition)
    {
        Transform nouvelleTransformee = Transform.Identity;
        nouvelleTransformee.origin = nouvellePosition;
        GlobalTransform = nouvelleTransformee;
    }

	public void Pause() {
		GetTree().Paused = true;
		
	}

	public void UnPause() {
		GetTree().Paused = false;

	}

	public string red(string texte) {
		return $"[color=red]{texte}[/color]";
	}
	public string bold(string texte) {
		return $"[b]{texte}[/b]";
	}

	public int longueurLigne=22;
	public string Data(string nom, object valeur, bool retourLigne=true){
		
		string retour= retourLigne ? "\n" : "";
		string espacesVides=new string (' ', longueurLigne-nom.Length-($"{valeur}".Length)-2);
		return $" {red(nom)}{espacesVides}{valeur} {retour}";
	}

	public string Titre(string nom){
		var egals=new string ('=', ((longueurLigne-nom.Length)/2)+1);
		return $"|{egals} {nom.ToUpper()} {egals}|\n";
	}


	public void Update(){
		var texteGaucheHUD = GetNode<RichTextLabel>("CanvasLayer/HUD/Textes/HUDGauche");
		var texteDroiteHUD = GetNode<RichTextLabel>("CanvasLayer/HUD/Textes/HUDDroite");

		var texteGauche="";

		texteGauche+= Titre("Position");
		texteGauche+= Data("posX", posiX);
		texteGauche+= Data("posY", posiY);
		texteGauche+= Data("posZ", posiZ);

		texteGauche+= Titre("Accéleration");
		texteGauche+= Data("accX", accX);
		texteGauche+= Data("accY", accY);
		texteGauche+= Data("accZ", accZ);

		texteGauche+= Titre("Orientation");
		texteGauche+= Data("oriX", oriX);
		texteGauche+= Data("oriY", oriY);

		texteGaucheHUD.SetBbcode(texteGauche);


		var texteDroite="";

		texteDroite+= Titre("Version du jeu");
		texteDroite+= Data("CoDem", version);

		texteDroite+= Titre("Infos execution");
		texteDroite+= Data("FPS", fps);

		texteDroiteHUD.SetBbcode(texteDroite);
	}
}

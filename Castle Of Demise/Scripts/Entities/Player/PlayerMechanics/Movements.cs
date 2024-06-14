using System;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player 
{
    private float _positionX;
    private float _positionY;
    private float _positionZ;
    private float _accelerationX;
    private float _accelerationY;
    private float _jumpVelocity;
    private float _accelerationZ;
    private float _orientationX;
    private float _orientationY;
    private Vector3 _direction;
    private ulong _lastJumpTime = Time.GetTicksUsec();
    private bool _landed=true;
    
    [Export] private float _accelerationSpeed;
    //10 
    
    [Export] private float _decelerationSpeed;
    // 10
    
    [Export] private float _maxSpeed;
    //12.5
    
    [Export] private float _gravity;
    // 34
    
    [Export] private float _jumpSpeed;
    // 25
    



    private void HandleMouseMovementInputs(float d)
    {
        var inputMovementVector = new Vector2();
        bool isMoving = false;
        if (!_cinematicMode)
        {
            if (Input.IsActionPressed("key_z"))
            {
                inputMovementVector.Y += 1;
                isMoving = true;
            }

            if (Input.IsActionPressed("key_s"))
            {
                inputMovementVector.Y -= 1;
                isMoving = true;
            }

            if (Input.IsActionPressed("key_q"))
            {
                inputMovementVector.X -= 1;
                isMoving = true;
            }

            if (Input.IsActionPressed("key_d"))
            {
                inputMovementVector.X += 1;
                isMoving = true;
            }
        }
        

        _direction = new Vector3();
        _direction += -GlobalTransform.Basis.Z * inputMovementVector.Y;
        _direction += GlobalTransform.Basis.X * inputMovementVector.X;
        HandleStepSounds(isMoving);
        
        RotateCamera(inputMovementVector.X, d);
        RotateWeapon(inputMovementVector.X, d);
        AdjustFov(d);
    }
    


    public void HandleMovements(double d)
    {
        if (_levitationMode)
        {
            return;
        }
        var horizontalVelocity = Velocity;


        var target = _direction * _maxSpeed;
        var acceleration = (_direction.Dot(horizontalVelocity) > 0) ? _accelerationSpeed : _decelerationSpeed;

        horizontalVelocity = horizontalVelocity.Lerp(target, acceleration * (float)d);
        

        if (_floorRayCast.GetCollider() != null)
        {

            if (!_landed && Time.GetTicksUsec() - _lastJumpTime > 100)
            {
                _landed = true;
                horizontalVelocity.Y = 0;
                _sfxPlayer.EmitSignal("PlaySFXSignal", "landed");
                CameraShake();
            }

            else if (Input.IsActionJustPressed("key_space"))
            {
                _lastJumpTime = Time.GetTicksUsec();
                horizontalVelocity.Y = Velocity.Y + _jumpSpeed;
                _sfxPlayer.EmitSignal("PlaySFXSignal", "jump");
                _landed = false;
            }
        }

        else
        {
            horizontalVelocity.Y = Velocity.Y - ((float)d * _gravity);
        }
        
        
        Velocity = horizontalVelocity;
        
        MoveAndSlide();
    }
    
    public void Teleport(Vector3 newPosition)
    {
        Transform3D newTransform = Transform3D.Identity;
        newTransform.Origin = newPosition;
        GlobalTransform = newTransform;
    }

    private void HandleRespawn(double d)//pas de raport au multi, par rapport a out of bound.
    {
        if (_levitationMode)
        {
            Velocity = new Vector3(0, 80*(float)d, -80*(float)d);
            MoveAndSlide();
        }
        else if (_positionY < -25)
        {
            //Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = -60f;
            //Teleport(newCoordinates);
        }
        else if (_positionY > 35)
        {
            //Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = 75f;
            //Teleport(newCoordinates);
        } else if (_positionY > 0)
        {
            //Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = 34f;
            //Teleport(newCoordinates);
        }
    }

}
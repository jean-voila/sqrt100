using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player 
{
    private float PositionX;
    private float PositionY;
    private float PositionZ;
    private float AccelerationX;
    private float AccelerationY;
    private float AccelerationZ;
    private float OrientationX;
    private float OrientationY;
    private Vector3 _direction;
    private Vector3 _velocity;
    private ulong LastJumpTime = Time.GetTicksUsec();
    private bool Landed = false;
    private float _accelerationSpeed = 6f;
    private float _decelerationSpeed = 6f;
    private float _maxSpeed = 17f;
    private float _gravity = 34f;
    private float _jumpSpeed = 25f;
    
    
    private void HandleMouseMovementInputs(float delta)
    {
        var inputMovementVector = new Vector2();
        bool isMoving = false;
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
        
        _direction = new Vector3();
        _direction += -GlobalTransform.Basis.Z * inputMovementVector.Y;
        _direction += GlobalTransform.Basis.X * inputMovementVector.X;
        HandleStepSounds(isMoving);
        
        RotateCamera(inputMovementVector.X, delta);
        AdjustFov(delta);
    }
    
    private void HandleJump()
    {
        if (_floorRayCast.GetCollider() == null) return;

        if (!Landed && Time.GetTicksUsec() - LastJumpTime > 10)
        {
            Landed = true;
            if (_SEEnabled)
            {
                _landSound.Play();
                // cameraShake();
            }
        }

        if (_floorRayCast.GetCollider() != null && Input.IsActionPressed("key_space"))
        {
            LastJumpTime = Time.GetTicksUsec();
            _velocity.Y = _jumpSpeed;
			
            if (_SEEnabled) _jumpSound.Play();
            Landed = false;
        }
    }

    public void HandleMovements(float delta)
    {
        var horizontalVelocity = Velocity;
        
        horizontalVelocity.Y = 0;

        var target = _direction * _maxSpeed;
        var acceleration = (_direction.Dot(horizontalVelocity) > 0) ? _accelerationSpeed : _decelerationSpeed;

        horizontalVelocity = horizontalVelocity.Lerp(target, acceleration * delta);
        
        horizontalVelocity.X = horizontalVelocity.X;
        horizontalVelocity.Z = horizontalVelocity.Z;

        if (_floorRayCast.GetCollider() == null)
        {
            horizontalVelocity.Y -= delta * _gravity;
        }

        Velocity = new Vector3(horizontalVelocity.X, horizontalVelocity.Y, horizontalVelocity.Z);
        MoveAndSlide();
    }
    
    private void Teleport(Vector3 newPosition)
    {
        Transform3D newTransform = Transform3D.Identity;
        newTransform.Origin = newPosition;
        GlobalTransform = newTransform;
    }

    private void HandleRespawn()
    {
        if (PositionY < -25)
        {
            Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = -60f;
            //Teleport(newCoordinates);
        }
        if (PositionY > 35)
        {
            Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = 75f;
            //Teleport(newCoordinates);
        } else if (PositionY > 0)
        {
            Vector3 newCoordinates = new Vector3(0.0f, 6.0f, 0.0f);
            _gravity = 34f;
            //Teleport(newCoordinates);
        }
    }

}
using Godot;

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
        
        _direction = new Vector3();
        _direction += -GlobalTransform.basis.z * inputMovementVector.y;
        _direction += GlobalTransform.basis.x * inputMovementVector.x;
        HandleStepSounds(isMoving);
        
        RotateCamera(inputMovementVector.x, delta);
        AdjustFOV(delta);
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
                cameraShake();
            }
        }

        if (_floorRayCast.GetCollider() != null && Input.IsActionPressed("key_space"))
        {
            LastJumpTime = Time.GetTicksUsec();
            _velocity.y = _jumpSpeed;
			
            if (_SEEnabled) _jumpSound.Play();
            Landed = false;
        }
    }

    public void HandleMovements(float delta)
    {
        var horizontalVelocity = _velocity;
        horizontalVelocity.y = 0;

        var target = _direction * _maxSpeed;
        var acceleration = (_direction.Dot(horizontalVelocity) > 0) ? _accelerationSpeed : _decelerationSpeed;

        horizontalVelocity = horizontalVelocity.LinearInterpolate(target, acceleration * delta);

        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;

        if (_floorRayCast.GetCollider() == null)
        {
            _velocity.y -= delta * _gravity;
        }
		
        _velocity = MoveAndSlide(_velocity, Vector3.Up);
    }
    
    private void Teleport(Vector3 newPosition)
    {
        Transform newTransform = Transform.Identity;
        newTransform.origin = newPosition;
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

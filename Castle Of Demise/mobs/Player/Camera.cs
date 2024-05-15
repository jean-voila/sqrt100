using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private readonly RandomNumberGenerator _randShake = new();
        
    private Camera3D CameraForFov => _usedCamera as Camera3D;
    [Export] private static readonly float OriginalFov = 100;
    private float _targetFov = OriginalFov;
    


    public void HandleMouseMovement(InputEventMouseMotion mouseInput)
    {
        // Adjust the Y rotation (Yaw)
        RotateY(-mouseInput.Relative.X * _mouseSensitivity);

        // Adjust the X rotation (Pitch)
        _head.RotateX(-mouseInput.Relative.Y * _mouseSensitivity);

        // Define your minimum and maximum pitch angles (in degrees)
        float minPitch = -90.0f;
        float maxPitch = 90.0f;

        // Clamp the X rotation within the specified range
        var currentRotation = _head.RotationDegrees;
        currentRotation.X = Mathf.Clamp(currentRotation.X, minPitch, maxPitch);

        // Apply the new rotation to the camera
        _head.RotationDegrees = currentRotation;
    }
        
    private void AdjustFov(float d)
    {
        if (Input.IsActionPressed("key_z") && _floorRayCast.GetCollider() != null)
            _targetFov = Mathf.Lerp(_targetFov, OriginalFov * _maxFov, _fovChangingSpeed * d);

        else
            _targetFov = Mathf.Lerp(_targetFov, OriginalFov, _fovChangingSpeed * d);
            
        CameraForFov.Fov = _targetFov;
    }
        
    private void RotateCamera(float inputX, float d)
    {
        if (_usedCamera != null)
        {
            _usedCamera.Rotation = new Vector3(
                _usedCamera.Rotation.X,
                _usedCamera.Rotation.Y,
                Mathf.Lerp(_usedCamera.Rotation.Z, -inputX * _camRotationAmount, 10 * d)
            );
        }
    }


    private Vector2 RandomOffset()
    {
        return new Vector2(_randShake.RandfRange(-_shakeStrength, _shakeStrength),
            _randShake.RandfRange(-_shakeStrength, _shakeStrength));
    }

    private void CameraShake()
    {
        _shakeStrength = _randShake.RandfRange(0.1f, 0.3f);
    }

    private void CameraShakeProcess()
    {
        if (_shakeStrength > 0)
        {
            _shakeStrength = (float)Mathf.Lerp(_shakeStrength, 0, _shakeDelay * GetProcessDeltaTime());
            Vector2 offset = RandomOffset();
            CameraForFov.HOffset = offset[0];
            CameraForFov.VOffset = offset[1];
        }
    }
        
}
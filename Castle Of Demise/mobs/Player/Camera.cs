using Godot;


public partial class Player
    {
        private Spatial _head;
        private NodePath _cameraNodePath;
        private Spatial _camera;
        private Camera _cameraForFOV;
        private float _originalFOV;
        private float _targetFOV;
        private float _maxFov = 1.15f;
        private float _FOVChangeSpeed = 9.0f;
        private float CamRotationAmount = 0.1f;
        private float _mouseSensitivity = 0.005f;

        public void _cameraInit()
        {
            _cameraNodePath = "Head/Camera";
            _head = GetNode<Spatial>("Head");
            _camera = GetNode<Spatial>(_cameraNodePath);
            _cameraForFOV = GetNode<Camera>(_cameraNodePath);
            _originalFOV = _cameraForFOV.Fov;
            _targetFOV = _originalFOV;
        }
        public void HandleMouseMovement(InputEventMouseMotion mouseInput)
        {
            // Adjust the Y rotation (Yaw)
            RotateY(-mouseInput.Relative.x * _mouseSensitivity);

            // Adjust the X rotation (Pitch)
            _head.RotateX(-mouseInput.Relative.y * _mouseSensitivity);

            // Define your minimum and maximum pitch angles (in degrees)
            float minPitch = -90.0f;
            float maxPitch = 90.0f;

            // Clamp the X rotation within the specified range
            var currentRotation = _head.RotationDegrees;
            currentRotation.x = Mathf.Clamp(currentRotation.x, minPitch, maxPitch);

            // Apply the new rotation to the camera
            _head.RotationDegrees = currentRotation;
        }
        
        private void AdjustFOV(float delta)
        {
            if (Input.IsActionPressed("key_z") && _floorRayCast.GetCollider() != null) 
                _targetFOV = Mathf.Lerp(_targetFOV, _originalFOV * _maxFov, _FOVChangeSpeed * delta);
            
            else 
                _targetFOV = Mathf.Lerp(_targetFOV, _originalFOV, _FOVChangeSpeed * delta);
            
            _cameraForFOV.Fov = _targetFOV;
        }
        
        private void RotateCamera(float inputX, float delta)
        {
            if (_camera != null)
            {
                _camera.Rotation = new Vector3(
                    _camera.Rotation.x,
                    _camera.Rotation.y,
                    Mathf.Lerp(_camera.Rotation.z, -inputX * CamRotationAmount, 10 * delta)
                );
            }
        }
        
        
    }

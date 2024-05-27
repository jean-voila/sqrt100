using System;
using CastleOfDemise.mobs.Ennemies;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private RayCast3D _shootRayCast;
    private PackedScene _bulletHoleScene;
    private PackedScene _bloodHit;
    private int _ammoAvailable;
    private int _ammoInMag;
    private int _ammoShooted;
    private int _strength;
    private int _killedEnemmies;
    private AnimationPlayer _animShoot;
    private const int _maxAmmo = 60;
    
    [Signal]
    public delegate bool KillSignalEventHandler();
    public void _shootInit()
    {
        _shootRayCast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
        _ammoAvailable = _maxAmmo;
        _ammoShooted = 0;
        _strength = 10;
        _ammoInMag = 6;
        _killedEnemmies = 0;
        _bulletHoleScene = GD.Load<PackedScene>("res://Assets/Effects/BulletHole/BulletHoleScene.tscn");
        _bloodHit = GD.Load<PackedScene>("res://Assets/Effects/BloodHit/BloodHit.tscn");
        _animShoot = GetNode<AnimationPlayer>("Head/Revolver/shoot");
    }
    
    public void SetAmmoInc(int ammo)
    {
        if (_ammoAvailable + ammo < _maxAmmo)
        {
            _ammoAvailable += ammo;
        }
        else
        {
            _ammoAvailable = _maxAmmo;
        }
    }

    public bool CanPickupAmmo()
    {
        return _ammoAvailable != _maxAmmo;
    }

    public bool canReload()
    {
        return _ammoInMag != 6 && _ammoAvailable != 0;
    }

    public bool outOfAmmo()
    {
        return _ammoAvailable == 0 && _ammoInMag == 0;
    }
    
    public void Shoot()
    {
        _ammoInMag--;
        _ammoShooted++;
        bool isEnnemiTouched = false;
        var rayEnd = _shootRayCast.GetCollisionPoint();
        // CameraShake();
        _animShoot.Play("Shoot");
        if (_shootRayCast.IsColliding())
        {
            var bulletHole = (Node3D)_bulletHoleScene.Instantiate();
            var bloodHit = (Node3D)_bloodHit.Instantiate();
            var hitObject = _shootRayCast.GetCollider() as Node;
            if (hitObject != null)
            {
                Node mobTouche = hitObject.GetParent<Node>();
                isEnnemiTouched = mobTouche.IsInGroup("ennemies");
                if (isEnnemiTouched && !((Enemy)hitObject).ImDead)
                {
                    Hit((Enemy)hitObject);
                    hitObject.AddChild(bloodHit);
                    bloodHit.GlobalTransform = new Transform3D(bloodHit.GlobalTransform.Basis, rayEnd);
                    bloodHit.LookAt(rayEnd + _shootRayCast.GetCollisionNormal() + new Vector3(0.01f, 0.01f, 0.01f), Vector3.Up);
                    bloodHit.GetNode<CpuParticles3D>("CPUParticles3D").Restart();
                }
                else if (!isEnnemiTouched)
                {
                    hitObject.AddChild(bulletHole);
                    bulletHole.GlobalTransform = new Transform3D(bulletHole.GlobalTransform.Basis, rayEnd);
                    bulletHole.LookAt(rayEnd + _shootRayCast.GetCollisionNormal() + new Vector3(0.01f, 0.01f, 0.01f), Vector3.Up);
                    bulletHole.GetNode<CpuParticles3D>("CPUParticles3D").Restart();
                }
            }
        }
        if (_SEEnabled && !isEnnemiTouched)
        {
             var randomGunShot = new Random().Next(0, _gunShotSounds.Count); 
             _gunShotSounds[randomGunShot].PitchScale = new Random().Next(1, 2); 
             _gunShotSounds[randomGunShot].Play();

        }
    }
    
    private void Hit(Enemy mobTouche)
    {
        if (!mobTouche.ImDead)
        {
            mobTouche.EmitSignal("HitSignal", _strength);
            if (!mobTouche.ImDead) _hitSound.Play();
            else _killedEnemmies++;

        }
    }

    private void reload()
    {
        
        if (_ammoAvailable > 0 && _ammoAvailable < 6)
        {
            _ammoInMag += _ammoAvailable;
            _ammoAvailable = 0;
        }
        else
        {
            _ammoAvailable -= 6 - _ammoInMag;
            _ammoInMag = 6;
        }
    }

    private void RotateWeapon(float inputX, float d)
    {
        if (_revolverModel != null)
        {
            _revolverModel.Rotation = new Vector3(
                _revolverModel.Rotation.X,
                _revolverModel.Rotation.Y,
                Mathf.Lerp(_revolverModel.Rotation.Z, -inputX * _revolverModelRotationAmount, 10 * d)
            );
        }
    }

    private void WeaponSway()
    {
        float cameraMovementDirectionX = _lastMouseMovement.X;
        float cameraMovementDirectionY = _lastMouseMovement.Y;
        float swayFactor = 0.007f;
        Vector3 sway = new Vector3(cameraMovementDirectionY * swayFactor, cameraMovementDirectionX * swayFactor, 0);
        float returnSpeed = 0.1f;
        if (_revolverModel != null)
        {
            _revolverModel.Rotation += sway;
            _revolverModel.Rotation = new Vector3(
                Mathf.Lerp(_revolverModel.Rotation.X, 0, returnSpeed),
                Mathf.Lerp(_revolverModel.Rotation.Y, Mathf.DegToRad(170), returnSpeed),
                Mathf.Lerp(_revolverModel.Rotation.Z, 0, returnSpeed)
            );
        }
    }
}
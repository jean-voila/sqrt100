using Godot;
using System;
using CastleOfDemise.mobs.Ennemies;


public partial class Player
{
    private RayCast _shootRayCast;
    private PackedScene _bulletHoleScene;
    private PackedScene _bloodHit;
    private AnimationPlayer _animationPlayer;
    private int _ammoAvailable;
    private int _ammoShooted;
    private int _strength;
    private int _killedEnemmies;
    
    [Signal]
    public delegate bool kill_signal();
    public void _shootInit()
    {
        _shootRayCast = GetNode<RayCast>("Head/Camera/RayCast");
        _ammoAvailable = 100;
        _ammoShooted = 0;
        _strength = 10;
        _killedEnemmies = 0;
        _bulletHoleScene = GD.Load<PackedScene>("res://Assets/Effects/BulletHole/BulletHoleScene.tscn");
        _bloodHit = GD.Load<PackedScene>("res://Assets/Effects/BloodHit/BloodHit.tscn");
        _animationPlayer = GetNode<AnimationPlayer>("Head/RevolverAnimationPlayer");
    }
    
    public void Shoot()
    {
        _ammoAvailable--;
        _ammoShooted++;
        bool isEnnemiTouched = false;
        var rayEnd = _shootRayCast.GetCollisionPoint();
        cameraShake();
        if (!_animationPlayer.IsPlaying())
        {
            _animationPlayer.Play("shoot");
        }
        GD.Print(_animationPlayer.CurrentAnimation);
        if (_shootRayCast.IsColliding())
        {
            var bulletHole = (Spatial)_bulletHoleScene.Instance();
            var bloodHit = (Spatial)_bloodHit.Instance();
            var hitObject = _shootRayCast.GetCollider() as Node;
            if (hitObject != null)
            {
                Node mobTouche = hitObject.GetParent<Node>();
                isEnnemiTouched = mobTouche.IsInGroup("ennemies");
                if (isEnnemiTouched)
                {
                    Hit((Ennemy)hitObject);
                    hitObject.AddChild(bloodHit);
                    bloodHit.GlobalTransform = new Transform(bloodHit.GlobalTransform.basis, rayEnd);
                    bloodHit.LookAt(rayEnd + _shootRayCast.GetCollisionNormal() + new Vector3(0.01f, 0.01f, 0.01f), Vector3.Up);
                    bloodHit.GetNode<CPUParticles>("CPUParticles").Restart();
                }
                else
                {
                    hitObject.AddChild(bulletHole);
                    bulletHole.GlobalTransform = new Transform(bulletHole.GlobalTransform.basis, rayEnd);
                    bulletHole.LookAt(rayEnd + _shootRayCast.GetCollisionNormal() + new Vector3(0.01f, 0.01f, 0.01f), Vector3.Up);
                    bulletHole.GetNode<CPUParticles>("CPUParticles").Restart();
                }
            }
        }
        if (_SEEnabled && !isEnnemiTouched)
        {
            if (!_jeansModEnabled)
            {
                var randomGunShot = new Random().Next(0, _gunShotSounds.Count);
                _gunShotSounds[randomGunShot].PitchScale = new Random().Next(1, 2);
                _gunShotSounds[randomGunShot].Play();
            }
            else
                _alternateShotSound.Play();
        }
    }
    
    private void Hit(Ennemy mobTouche)
    {
        if (!mobTouche.ImDead)
        {
            mobTouche.EmitSignal("HitSignal", _strength);
            if (!mobTouche.ImDead) _hitSound.Play();
            else _killedEnemmies++;

        }
    }
}
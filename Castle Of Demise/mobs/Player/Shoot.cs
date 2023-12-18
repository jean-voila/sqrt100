using Godot;
using System;


public partial class Player
{
    private RayCast _shootRayCast;
    private PackedScene _bulletHoleScene;
    private int _ammoAvailable;
    private int _ammoShooted;


    public void _shootInit()
    {
        _shootRayCast = GetNode<RayCast>("Head/Camera/RayCast");
        _ammoAvailable = 30;
        _ammoShooted = 0;
        _bulletHoleScene = GD.Load<PackedScene>("res://Assets/Effects/BulletHole/BulletHoleScene.tscn");

    }
    
    public void Shoot()
    {
        _ammoAvailable--;
        _ammoShooted++;
        bool isEnnemiTouched = false;
        var rayEnd = _shootRayCast.GetCollisionPoint();
        if (_shootRayCast.IsColliding())
        {
            var bulletHole = (Spatial)_bulletHoleScene.Instance();
            var hitObject = _shootRayCast.GetCollider() as Node;
            if (hitObject != null)
            {
                Node mobTouche = hitObject.GetParent<Node>();
                isEnnemiTouched = mobTouche.IsInGroup("ennemies");
                if (isEnnemiTouched)
                    Kill(mobTouche);
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
    
    private void Kill(Node mobTouche)
    {
        if (_SEEnabled) _killedSound.Play();
		mobTouche.QueueFree();
    }

}
using Godot;
using Godot.Collections;

namespace CastleOfDemise.mobs.Ennemies
{
    public class Ennemy : KinematicBody
    {
        [Signal]
        public delegate void hit_signal(int strength);


        private ulong _timeIsDead;
        private ulong _timeBeforeDisapp;
        public int _health { get; private set; }
        public bool _isDead { get; private set; }
    
        public override void _Ready()
        {
            _health = 100;
            _isDead = false;
            _timeIsDead = 0;
            _timeBeforeDisapp = 500;
            
            Connect("hit_signal", this, "Hit");
            
        }

        public override void _Process(float delta)
        {
            HandleDie();
        }

        public void Hit(int strength)
        {
            if (!_isDead)
            {
                _health -= strength;
                _isDead = _health <= 0;
                if (_isDead)
                {
                    GetChild<AnimatedSprite3D>(0).Animation = "dying";
                    _timeIsDead = Time.GetTicksMsec();
                }
            }
            
        }

        private void HandleDie()
        {
            ulong timeNow = Time.GetTicksMsec();
            if (_isDead && (timeNow - _timeIsDead > _timeBeforeDisapp))
                QueueFree();
        }
        
        
    }
}
using Godot;
using Godot.Collections;

namespace CastleOfDemise.mobs.Ennemies
{
    public abstract class Ennemy : KinematicBody
    {
        [Signal] 
        public delegate void HitSignal(int strength);
        
        private readonly AudioStreamPlayer2D _deathSound = GetDeathSound();
        public bool ImDead { get; private set; }
        private ulong _timeSinceImDead;
        
        
        private const ulong TimeBeforeDisappear = 500;
        public virtual int Health { get; set; }
        
        private static AudioStreamPlayer2D GetDeathSound()
        {
            AudioStreamPlayer2D deathSound = new AudioStreamPlayer2D();
            deathSound.Stream = GD.Load<AudioStreamSample>("res://Assets/SoundEffects/death.wav");
            return deathSound;
        }
        
        public override void _Ready()
        {
            AddChild(_deathSound);
            Connect("HitSignal", this, "Hit");
        }

        public override void _Process(float delta)
        {
            ulong timeNow = Time.GetTicksMsec();
            if (ImDead && timeNow - _timeSinceImDead > TimeBeforeDisappear)
            {
                QueueFree();
            }
            
        }

        public void Hit(int strength)
        {
            if (!ImDead)
            {
                Health -= strength;
                ImDead = Health <= 0;
                if (ImDead)
                {
                    _deathSound.Play();
                    GetChild<AnimatedSprite3D>(0).Play("dying");
                    _timeSinceImDead = Time.GetTicksMsec();
                }
            }
        }
        
    }
}
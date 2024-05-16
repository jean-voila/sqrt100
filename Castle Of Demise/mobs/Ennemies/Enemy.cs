using Godot;
using Godot.Collections;

namespace CastleOfDemise.mobs.Ennemies
{
    public abstract partial class Enemy : CharacterBody3D
    {
        [Signal] 
        public delegate void HitSignalEventHandler(int strength);
        
        private readonly AudioStreamPlayer2D _deathSound = GetDeathSound();
        public bool ImDead { get; private set; }
        private ulong _timeSinceImDead;
        
        
        private const ulong TimeBeforeDisappear = 500;
        public virtual int Health { get; set; }
        
        private static AudioStreamPlayer2D GetDeathSound()
        {
            AudioStreamPlayer2D deathSound = new AudioStreamPlayer2D();
            deathSound.Stream = GD.Load<AudioStream>("res://Assets/SoundEffects/death.wav");
            return deathSound;
        }
        
        public override void _Ready()
        {
            AddChild(_deathSound);
            Connect("HitSignal", new Callable(this, "Hit"));
            GetChild<AnimatedSprite3D>(0).Play("idle");

        }

        public override void _Process(double d)
        {
            ulong timeNow = Time.GetTicksMsec();
            if (ImDead && timeNow - _timeSinceImDead > TimeBeforeDisappear)
            {
                QueueFree();
            }

            if (!ImDead)
            {
                if (GetChild<AnimatedSprite3D>(0).Animation == "touched")
                {
                    if (GetChild<AnimatedSprite3D>(0).Frame == GetChild<AnimatedSprite3D>(0).SpriteFrames.GetFrameCount("touched")-1)
                    {
                        GetChild<AnimatedSprite3D>(0).Play("idle");
                    }

                }


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
                else
                {
                    GetChild<AnimatedSprite3D>(0).Play("touched");
                    
                }
            }
        }
        
    }
}
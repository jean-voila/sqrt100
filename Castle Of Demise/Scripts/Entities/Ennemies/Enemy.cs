using Godot;
using Godot.Collections;

namespace CastleOfDemise.mobs.Ennemies
{
    public abstract partial class Enemy : CharacterBody3D
    {
        [Signal]
        public delegate void HitSignalEventHandler(int strength);
        public bool ImDead { get; private set; }
        private ulong _timeSinceImDead;

        private const ulong TimeBeforeDisappear = 500;
        protected virtual int Health { get; set; }
        
        protected virtual float Speed { get; set; } = 0.30f;
        protected virtual int Damage { get; set; }

        [Export] private AudioStreamPlayer _sfxPlayer;
        
        [Export] public NavigationAgent3D NavAgent;
        public Vector3 target;
        

        public override void _Ready()
        {

            SetPhysicsProcess(false);
            WaitForPhysicsFrame();
            AddToGroup("ennemies");
            Connect("HitSignal", new Callable(this, "Hit"));
            GetChild<AnimatedSprite3D>(0).Play("idle");
            SetPhysicsProcess(true);
        }
        
        private async void WaitForPhysicsFrame()
        {
            await ToSignal(GetTree(), "physics_frame");
            SetPhysicsProcess(true);
        }

        public override void _Process(double d)
        {
            ulong timeNow = Time.GetTicksMsec();
            switch (ImDead)
            {
                case true when timeNow - _timeSinceImDead > TimeBeforeDisappear:
                    QueueFree();
                    break;
                case false:
                {
                    if (GetChild<AnimatedSprite3D>(0).Animation == "touched")
                    {
                        if (GetChild<AnimatedSprite3D>(0).Frame ==
                            GetChild<AnimatedSprite3D>(0).SpriteFrames.GetFrameCount("touched") - 1)
                        {
                            GetChild<AnimatedSprite3D>(0).Play("idle");
                        }
                    }
                    break;
                }
            }
        }

        public void UpdateTargetLocation(Vector3 targetLocation)
        {
            NavAgent.TargetPosition = targetLocation;
        }

        public override void _PhysicsProcess(double d)
        {
            Vector3 currentLocation = GlobalTransform.Origin;
            Vector3 nextLocation = NavAgent.GetNextPathPosition();
            Vector3 newVelocity = (nextLocation - currentLocation).Normalized() * Speed * 10f;
            Velocity = newVelocity;
            MoveAndSlide();
            // MoveTowardsPlayer(d);
        }
    

        public void Hit(int strength)
        {
            if (!ImDead)
            {
                Health -= strength;
                ImDead = Health <= 0;
                if (ImDead)
                {
                    _sfxPlayer.EmitSignal("PlaySFXSignal", "death");
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
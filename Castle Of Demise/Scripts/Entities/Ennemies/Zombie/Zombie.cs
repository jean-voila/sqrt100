using Godot;
using System;
using CastleOfDemise.mobs.Ennemies;
using CastleOfDemise.mobs.Player;

public partial class Zombie : Enemy
{
    protected override int Health { get; set; } = 30;
    
    protected override float Speed { get; set; } = 1.6f;
    
    protected override int Damage { get; set; } = 15;
    
    [Export] public Timer Cooldown;
    private Player _player;

    private void _on_range_zombie_body_entered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _player = (Player)body;
            Cooldown.Start();
        }
    }

    private void _on_range_zombie_body_exited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _player = (Player)body;
            Cooldown.Stop();
        }
    }
    
    private void _on_cooldown_timeout()
    {
        _player.TakeDamage(Damage);
    }

    
}
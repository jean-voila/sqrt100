using Godot;
using System;
using CastleOfDemise.mobs.Ennemies;

public partial class Zombie : Enemy
{
    protected override int Health { get; set; } = 60;
    
    protected override float Speed { get; set; } = 0.6f;

}
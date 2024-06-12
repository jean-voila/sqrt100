using Godot;
using System;
using CastleOfDemise.mobs.Ennemies;

public partial class Bat : Enemy
{
    protected override int Health { get; set; } = 30;
    
    protected override float Speed { get; set; } = 1.2f;
    
    
}

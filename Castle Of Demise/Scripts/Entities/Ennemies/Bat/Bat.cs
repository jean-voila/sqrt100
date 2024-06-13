using Godot;
using System;
using CastleOfDemise.mobs.Ennemies;
using CastleOfDemise.mobs.Player;

public partial class Bat : Enemy
{
	protected override int Health { get; set; } = 30;

	protected override float Speed { get; set; } = 1.2f;

	protected override int Damage { get; set; } = 10;

	[Export] public Timer Cooldown;
	private Player _player;

	private void _on_attack_range_body_entered(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{
			_player = (Player)body;
			_on_attack_cool_down_timeout();//the enemy should attack the player immediately
			Cooldown.Start();
		}
	}

	private void _on_attack_range_body_exited(Node3D body)
	{
		if (body.IsInGroup("Player"))
		{ 
			_player = (Player)body;
			Cooldown.Stop();
		}
	}

	private void _on_attack_cool_down_timeout()
	{
		_player.TakeDamage(Damage);
	}
}
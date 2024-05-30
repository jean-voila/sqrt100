using Godot;
using System;

public partial class healthPack : Area3D
{
	[Export] public int healthInc = 10;
	private AudioStreamPlayer3D _healthPickupSound;
	private Timer _time;
	public Sprite3D _healthSprite;
    
	public override void _Ready()
	{
		_healthPickupSound = GetNode<AudioStreamPlayer3D>("healthPickupSound");
		_time = GetNode<Timer>("Timer");
		_healthSprite = GetNode<Sprite3D>("Sprite3D");
	}
    
	public void _on_body_entered(Node body)
	{
		if (body is CastleOfDemise.mobs.Player.Player player && _time.TimeLeft <= 0 && player.CanPickupHealth())
		{
			_healthPickupSound.Play();
			_time.Start();
			player.SetHealthInc(healthInc);
			_healthSprite.QueueFree();
		}
	}
    
	public void _on_timer_timeout()
	{
		QueueFree();
	}
}

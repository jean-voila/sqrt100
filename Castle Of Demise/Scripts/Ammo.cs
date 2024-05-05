using Godot;
using System;

public class Ammo : Area
{
    [Export] public int ammoInc = 10;
    private AudioStreamPlayer3D _reloadSound;
    private Timer _time;
    public Sprite3D _ammoSprite;
    
    public override void _Ready()
    {
        _reloadSound = GetNode<AudioStreamPlayer3D>("reloadSound");
        _time = GetNode<Timer>("Timer");
        _ammoSprite = GetNode<Sprite3D>("Sprite3D");
    }
    
    public void _on_Ammo_body_entered(Node body)
    {
        if (body is Player player && _time.TimeLeft <= 0)
        {
            _reloadSound.Play();
            _time.Start();
            player.SetAmmo(ammoInc);
            _ammoSprite.QueueFree();
        }
    }
    
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}

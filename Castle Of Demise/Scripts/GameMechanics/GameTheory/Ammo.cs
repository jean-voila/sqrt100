using Godot;

namespace CastleOfDemise.Scripts;

public partial class Ammo : Area3D
{
    [Export] public int ammoInc = 12;
    private AudioStreamPlayer3D _reloadSound;
    private Timer _time;
    public Sprite3D _ammoSprite;
    [Export] private AudioStreamPlayer _sfxPlayer;
    
    public override void _Ready()
    {
        _time = GetNode<Timer>("Timer");
        _ammoSprite = GetNode<Sprite3D>("Sprite3D");
    }
    
    public void _on_Ammo_body_entered(Node body)
    {
        if (body is CastleOfDemise.mobs.Player.Player player && _time.TimeLeft <= 0 && player.CanPickupAmmo())
        {
            _sfxPlayer.EmitSignal("PlaySFXSignal", "pickupable/ammo");
            _time.Start();
            player.SetAmmoInc(ammoInc);
            _ammoSprite.QueueFree();
        }
    }
    
    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}
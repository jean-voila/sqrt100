using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private bool IsDead;
    [Export] private Sprite2D BloodHitEffect;
    [Export] private Timer BloodHitEffectTimer;
    
    public int PlayerHealth;
    private int _maxHealth;

    public void _playerHealthInit()
    {
        PlayerHealth = 80;
        _maxHealth = 100;
    }

    public void SetHealthInc(int newH)
    {
        PlayerHealth += newH;
    }
    
    public bool CanPickupHealth()
    {
        return PlayerHealth < _maxHealth;
    }

    public void TakeDamage(int var)//make player take damage if it can and return if the player took damage and kills him if needed.
    {
        if (PlayerHealth - var > 0)
        {
            PlayerHealth -= var;
            IsDead = false;
            BloodHitEffect.Show();
            BloodHitEffectTimer.Start();
            CameraShake();
            _sfxPlayer.EmitSignal("PlaySFXSignal", "playerhit");
        }
        else
        {
            PlayerHealth -= var;
            IsDead = true;
        }
    }
}
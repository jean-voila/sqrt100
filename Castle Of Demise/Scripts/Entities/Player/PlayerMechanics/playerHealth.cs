using CastleOfDemise.Scripts.Menus.MultiLauncher;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private bool IsDead;
    [Export] private Sprite2D BloodHitEffect;
    [Export] private Timer BloodHitEffectTimer;
    
    public static int PlayerHealth;
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

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
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
        HandleDeath();
    }
    
    private void _on_blood_hit_effect_timer_timeout()
    {
        BloodHitEffect.Hide();
    }

    private void HandleDeath()
    {
        if (IsDead)
        {
            if (!IsMultiplayer)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
                GetTree().ChangeSceneToFile("res://menus/menuPlayerMort.tscn");
            }
            else
            {
                if (IsServer)
                {
                    mpMap02.PlayerList[1].Teleport(new Vector3(7,14,2));
                    Rpc(nameof(MultiplayerHUD.HostScored));
                }
                else
                {
                    mpMap02.PlayerList[0].Teleport(new Vector3(7,14,2));
                    Rpc(nameof(MultiplayerHUD.ClientScored));
                }
                // GD.Print(Name + " is " + PlayerName);
                // GD.Print(PlayerHealth);
                PlayerHealth = _maxHealth;
                // GD.Print(PlayerHealth);

            }
            
        }
    }
}
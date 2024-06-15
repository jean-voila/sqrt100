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
        _maxHealth = 100;
        PlayerHealth = _maxHealth;
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
            RpcId(this.PlayerId, nameof(UpdatePlayerInfo));
        }
        else
        {
            PlayerHealth -= var;
            IsDead = true;
            RpcId(this.PlayerId, nameof(UpdatePlayerInfo));
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
                    mpMap02.PlayerList[1].Teleport(new Vector3(0,24,2));
                    Rpc(nameof(HostScored));
                }
                else
                {
                    mpMap02.PlayerList[0].Teleport(new Vector3(0,24,2));
                    Rpc(nameof(ClientScored));

                }
                // GD.Print(Name + " is " + PlayerName);
                // GD.Print(PlayerHealth);
                PlayerHealth = _maxHealth;
                AmmoAvailable = 6;
                RpcId(this.PlayerId, nameof(UpdatePlayerInfo));
                // GD.Print(PlayerHealth);

            }
            
        }
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private static void HostScored()
    {
        mpMap02.PlayerList[0].PlayerScore++;
        GD.Print("Host scored");
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]

    private static void ClientScored()
    {
        mpMap02.PlayerList[1].PlayerScore++;
        GD.Print("Client scored");

    }
}
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
    
    
    private static int clientScore = 0;
    private static int serverScore = 0;

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
                    //GD.Print(clientScore + " serverkill");
                    Rpc(nameof(UpdateServerScore));
                    mpMap02.PlayerList[0].PlayerScore = serverScore;

                }
                else
                {
                    mpMap02.PlayerList[0].Teleport(new Vector3(0,24,2));
                    //GD.Print(serverScore + " clientkill");
                    Rpc(nameof(UpdateClientScore));
                    mpMap02.PlayerList[1].PlayerScore = clientScore;
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
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void UpdateServerScore()
    {
        serverScore++;
        mpMap02.PlayerList[0].PlayerScore = serverScore;
        CheckWin();

    }
   
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void UpdateClientScore()
    {
        clientScore++;
        mpMap02.PlayerList[1].PlayerScore = clientScore;

       CheckWin();
    }
    
    

    private  void CheckWin()
    {
        //GD.Print("checking win");
        GD.Print(("condition 1 (serverwon)" + (mpMap02.PlayerList[0].PlayerScore >= mpMap02.ScoreToReachUltimate)));
        GD.Print("condition 2 (clientwon)" + (mpMap02.PlayerList[1].PlayerScore >= mpMap02.ScoreToReachUltimate));
        if (mpMap02.PlayerList[0].PlayerScore >= MultiplayerHUD.ScoretoReachValue)
        {
            menuPartieFinie.winnerName = "Hôte";
            GetTree().ChangeSceneToFile("res://menus/menuPartieFinie.tscn");
        }
        else if (mpMap02.PlayerList[1].PlayerScore >= MultiplayerHUD.ScoretoReachValue)
        {
            menuPartieFinie.winnerName = "Client";
            GetTree().ChangeSceneToFile("res://menus/menuPartieFinie.tscn");

        }
    }
}
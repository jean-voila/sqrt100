namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    public int PlayerHealth;

    public void _playerHealthInit()
    {
        PlayerHealth = 100;
    }

    public void SetHealthInc(int newH)
    {
        PlayerHealth += newH;
    }
}
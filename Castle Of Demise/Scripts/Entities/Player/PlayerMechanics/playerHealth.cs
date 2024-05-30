namespace CastleOfDemise.mobs.Player;

public partial class Player
{
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
}
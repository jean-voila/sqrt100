using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private bool _tutorielFini = false;

    public void _PanneauBienvenueAtteint()
    {
        if (!_tutorielFini)
        {
            GD.Print("tutoriel finitopesto");
            _tutorielFini = true;
        }
        
    }
}
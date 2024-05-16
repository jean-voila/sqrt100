using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private void UpdatePlayerInfo()
    {
        var texteAmmo = GetNode<RichTextLabel>("HUD/PlayerInfos/Ammo");
        var valeurTexteAmmo = $"[right]{_ammoAvailable}[/right] ";
        texteAmmo.Text = valeurTexteAmmo;
		
        var texteHealth = GetNode<RichTextLabel>("HUD/PlayerInfos/Health");
        var valeurTexteHealth = $"[right]{PlayerHealth}[/right] ";
        texteHealth.Text = valeurTexteHealth;
    }
}
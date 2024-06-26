using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private void UpdatePlayerInfo()
    {
        var texteAmmo = GetNode<RichTextLabel>("HUD/PlayerInfos/Ammo");
        var valeurTexteAmmo = $"[right]{AmmoAvailable}[/right] ";
        texteAmmo.Text = valeurTexteAmmo;
		
        var texteHealth = GetNode<RichTextLabel>("HUD/PlayerInfos/Health");
        var valeurTexteHealth = $"[right]{PlayerHealth}[/right] ";
        texteHealth.Text = valeurTexteHealth;

        var texteMag = GetNode<RichTextLabel>("HUD/PlayerInfos/AmmoMag");
        var valeurTexteMag = _ammoInMag.ToString();
        texteMag.Text = valeurTexteMag;
    }
}
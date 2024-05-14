using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private void UpdatePlayerInfo()
    {
        var texteAmmo = GetNode<RichTextLabel>("HUD/PlayerInfos/Ammo");
        var valeurTexteAmmo = $"[right]{_ammoAvailable}[/right] ";
        texteAmmo.Text = valeurTexteAmmo;
		
    }
}
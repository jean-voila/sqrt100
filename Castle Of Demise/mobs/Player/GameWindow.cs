using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private void _gameWindowInit()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    private void ToggleFullscreen(bool fullscreen)
    {
        DisplayServer.WindowSetMode(fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
    }
    
    private void QuitGame()
    {
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://menus/TitleScreen.tscn");
    }

}
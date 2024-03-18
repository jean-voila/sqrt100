using Godot;

public partial class Player
{
    private void _gameWindowInit()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    private void ToggleFullscreen(bool fullscreen)
    {
        OS.WindowFullscreen = fullscreen;
    }
    
    private void QuitGame()
    {
        GetTree().ChangeScene("res://menus/TitleScreen.tscn");
    }

}

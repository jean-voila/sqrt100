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
        _pauseMenu.Visible = false;
        GetTree().Paused = false;
        GetTree().ChangeScene("res://menus/TitleScreen.tscn");
    }

}

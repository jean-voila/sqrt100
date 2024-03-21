using Godot;
using System;

public class TitleScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    
    private void _PlayButtonPressed()
    {
        GetNode<Control>("%TitleMenu").Hide();
        GetNode<Control>("%MultiplayerMenu").Hide();
        GetNode<Control>("%GamemodeMenu").Show();
        
        
    }

    private void _Exit()
    {
        GetTree().Quit();
    }

    private void _OptionButtonPressed()
    {
        GetNode<Control>("%TitleMenu").Hide();
        GetNode<Control>("%OptionsMenu").Show();
    }

    private void _focused_button()
    {
        GetNode<AudioStreamPlayer2D>("%FocusedButtonSound").Play();
    }

    private void _pressed_button()
    {
        GetNode<AudioStreamPlayer2D>("%PressedButtonSound").Play();
    }

    private void _show_title_screen_()
    {
        GetNode<Control>("%GamemodeMenu").Hide();
        GetNode<Control>("%OptionsMenu").Hide();
        GetNode<Control>("%TitleMenu").Show();
    }
    
  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if (Input.IsActionPressed("key_f6"))
      {
          GetTree().ChangeScene("res://maps/DebugMap.tscn");
      }
  }
}

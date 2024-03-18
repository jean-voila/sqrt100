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
        GetTree().ChangeScene("res://menus/GamemodeMenu.tscn");
    }

    private void _Exit()
    {
        GetTree().Quit();
    }

    private void _OptionButtonPressed()
    {
        GetTree().ChangeScene("res://menus/OptionsMenu.tscn");
    }
    
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

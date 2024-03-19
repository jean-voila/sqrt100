using Godot;
using System;

public class GamemodeMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }



    private void _singlePlayer()
    {
        GetTree().ChangeScene("res://maps/DebugMap.tscn");
    }

    private void _multiplayerPressed()
    {
        GetNode<Control>("%GamemodeMenu").Hide();
        GetNode<Control>("%MultiplayerMenu").Show();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

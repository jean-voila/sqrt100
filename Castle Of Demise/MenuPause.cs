using Godot;
using System;

public class MenuPause : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        this.Visible = false;

    }

    private void Resume(){

    }



  public override void _Process(float delta)
  {

  }
}

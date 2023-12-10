using Godot;
using System;

public class DebugMap : Spatial
{
    public override void _Ready()
    {
        var menuPause = GetNode<Control>("CanvasLayer/MenuPause");

    }

    public override void _Input(InputEvent @event){


		if (Input.IsActionJustPressed("key_escape")){ //temporaire pour fermer le jeu proprement
			GetTree().Paused = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			var menuPause = GetNode<Control>("CanvasLayer/MenuPause");
			menuPause.Visible = true;

		}
	}

    public void Resumer(){
        
            var menuPause = GetNode<Control>("CanvasLayer/MenuPause");
            menuPause.Visible = false;
            Input.MouseMode = Input.MouseModeEnum.Captured;
            GetTree().Paused = false;
			
        
    }
    
public override void _Process(float delta)
      {
        var bouton = GetNode<Button>("CanvasLayer/MenuPause/Panneau/Elements/Resume");

        if (bouton.Pressed) Resumer();
      
      }
}

using Godot;

namespace CastleOfDemise.Scripts;

public partial class Resume : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double d)
    {
        if (Input.IsActionJustPressed("key_escape"))
        {
            EmitSignal("pressed");
        }
    }
}
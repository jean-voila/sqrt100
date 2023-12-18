using Godot;
using System;

public class obelix : KinematicBody
{
    public float Speed = 5.0f;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      MoveRandomly(delta);
  }
  
  private void MoveRandomly(float delta)
  {
      // Generate a random direction vector
      Vector3 randomDirection = new Vector3(
          (float)GD.Randf(), // Random value between 0 and 1 for X
          0.0f,               // Keep the mob at the same height (Y-axis)
          (float)GD.Randf()  // Random value between 0 and 1 for Z
      );

      // Normalize the vector to ensure constant speed
      randomDirection = randomDirection.Normalized();

      // Update the mob's position based on the random direction and speed
      Translation += randomDirection * Speed * delta;
  }
}

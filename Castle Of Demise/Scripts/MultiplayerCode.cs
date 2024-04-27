using System.Collections.Generic;
using Godot;

namespace CastleOfDemise.Scripts
{
    
    



    public class Player : Node2D
    {
        public int Id { get; set; }
        public  Vector2 Position { get; set; }
    }



    public class MultiplayerSynchroniser : Control
    {

        Player hostPlayer;
        Player clientPlayer;

        public override void _Ready()
        {
            hostPlayer = new Player { Id = 1 };
            clientPlayer = new Player { Id = 2 };
        }


        public override void _Process(float delta)
        {
            
        }



       

        
















    }
}


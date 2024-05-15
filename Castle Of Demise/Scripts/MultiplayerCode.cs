using System.Collections.Generic;
using Godot;

namespace CastleOfDemise.Scripts
{
    
    



    public partial class Player : Node2D
    {
        public int Id { get; set; }
        public  Vector2 Position { get; set; }
    }



    public partial class MultiplayerSynchroniser : Control
    {

        Player hostPlayer;
        Player clientPlayer;

        public override void _Ready()
        {
            hostPlayer = new Player { Id = 1 };
            clientPlayer = new Player { Id = 2 };
        }


        public override void _Process(double d)
        {
            
        }



       

        
















    }
}


using CastleOfDemise.Scripts.Menus.MultiLauncher;
using Godot;

namespace CastleOfDemise.Scripts.GameMechanics.MultiplayerMechanics
{

    public partial class MultiplayerCode : Control
    {
        private static int _scoretoReachvalue = 0;
        private static int _gameModeValue = 0;

        private int YouScore = 0;
        private int OpponentScore = 0;
        private int ScoreToReach = 0;
        
        
        
        public override void _Ready()
        {
            (_scoretoReachvalue, _gameModeValue) = GetNode<SetupGameAsHost>("SetupGameAsHost").Data;
            ScoreToReach = _scoretoReachvalue;
            
            
            
        }
        





    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

}




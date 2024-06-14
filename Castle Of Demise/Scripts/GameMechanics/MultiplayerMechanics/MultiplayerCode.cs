using CastleOfDemise.Scripts.Menus.MultiLauncher;
using Godot;

namespace CastleOfDemise.Scripts.GameMechanics.MultiplayerMechanics
{

    public partial class MultiplayerCode : Control
    {
        private static int _scoretoReachvalue = 0;
        private static int _gameModeValue = 0;

        private static int _hostScore = 0;
        private static int _clientScore = 0;
        private static int _scoreToReach = 0;

        public static void HostScored()
        {
            _hostScore++;
        }
        public static void ClientScored()
        {
            _hostScore++;
        }

        
        public override void _Ready()
        {
            (_scoretoReachvalue, _gameModeValue) = GetNode<SetupGameAsHost>("SetupGameAsHost").Data;
            _scoreToReach = _scoretoReachvalue;
            
            
            
        }
        





    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

}




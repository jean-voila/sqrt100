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

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
        public static void HostScored()
        {
            _hostScore++;
            
        }
        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]

        public static void ClientScored()
        {
            _clientScore++;
        }

        private static void CheckWin()
        {
            if (_hostScore >= _scoreToReach)
            {
                //host wins
            }
            else if (_clientScore >= _scoreToReach)
            {
                //client wins
            }
        }

        
        public override void _Ready()
        {
            (_scoretoReachvalue, _gameModeValue) = GetNode<SetupGameAsHost>("SetupGameAsHost").Data;
            _scoreToReach = _scoretoReachvalue;
            
            
            
        }
        





    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

}




using Godot;

namespace CastleOfDemise.Scripts.Menus
{
    

    public partial class MultiplayerMenu : Control
    {

        private ENetMultiplayerPeer _peer = new();
        private Label _statusOk;
        private Label _statusFail;
        private Button _hostButton;
        private Button _joinButton;
    
    
        private const int DefaultPort = 8910;
        private const int MaxNumberOfPeers = 2;
        public const string ServerIp = "127.0.0.1";


        private void _clientPressed()
        {
            GetNode<Control>("%GamemodeMenu").Hide();
            GetNode<Control>("%SetupGameAsClient").Show();
        }
        
        private void _hostPressed()
        {
            GetNode<Control>("%GamemodeMenu").Hide();
            GetNode<Control>("%SetupGameAsHost").Show();
        }
        
        
       
    
       
      
    }
}
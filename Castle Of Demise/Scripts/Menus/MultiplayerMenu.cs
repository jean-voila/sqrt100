using Godot;

namespace CastleOfDemise.Scripts.Menus
{
    

    public partial class MultiplayerMenu : Control
    {

    
    
    
        


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
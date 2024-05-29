using Godot;

namespace CastleOfDemise.Scripts.Menus
{


    public partial class MultiplayerMenu : Control
    {

        [Export] private int port = 8910;
        [Export] private string address = "127.0.0.1";

        private ENetMultiplayerPeer peer;

// Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Multiplayer.PeerConnected += PeerConnected;
            Multiplayer.PeerDisconnected += PeerDisconnected;
            Multiplayer.ConnectedToServer += ConnectedToServer;
            Multiplayer.ConnectionFailed += ConnectionFailed;
        }

        private void ConnectionFailed()
        {
            GD.Print("CONNECTION FAILED");
        }

        private void ConnectedToServer()
        {
            GD.Print("CONNECTED TO SERVER");
        }

        private void PeerDisconnected(long id)
        {
            GD.Print("PLAYER DISCONNECTED: " + id.ToString());
        }

        private void PeerConnected(long id)
        {
            GD.Print("PLAYER CONNECTED! " + id.ToString());
        }

// Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
        private void _clientPressed()
        {
            peer = new ENetMultiplayerPeer();
            peer.CreateClient(address, port);
            peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
            Multiplayer.MultiplayerPeer = peer;
            GD.Print("JOINING GAME...");
        }

        private void _hostPressed()
        {
            GetNode<Control>("%MultiplayerMenu").Hide();
            // GetNode<Control>("%GamemodeMenu").Hide();
            GetNode<Control>("%SetupGameAsHost").Show();


            peer = new ENetMultiplayerPeer();
            var error = peer.CreateServer(port, 2);
            if (error != Error.Ok)
            {
                GD.Print("ERROR CANNOT HOST: " + error.ToString());
                return;
            }

            peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
            Multiplayer.MultiplayerPeer = peer;
            GD.Print("WAITING FOR PLAYERS!");
        }
    }

    // test




}

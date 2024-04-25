using Godot;
using System;


public class MultiplayerMenu : Control
{


    private NetworkedMultiplayerENet _peer;
    private Label _statusOk;
    private Label _statusFail;
    private const int DefaultPort = 8910;
    private const int MaxNumberOfPeers = 2;
    public string ServerIp;

    public MultiplayerMenu(string serverIp)
    {
        ServerIp = serverIp;
    }


    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    
    private void _return_pressed_button()
    {
        // Stop the server and client peers
        _peer?.CloseConnection();

        // Disconnect them from the network
        GetTree().NetworkPeer = null;

        GD.Print("Stopped everything relating to network connections");
    }
    
    
    
    private void _clientPressed()
    {
        if (!ServerIp.IsValidIPAddress())
        {
            SetStatus("IP address is invalid", false);
            return;
        }

        _peer = new NetworkedMultiplayerENet();
        _peer.CompressionMode = NetworkedMultiplayerENet.CompressionModeEnum.RangeCoder;
        _peer.CreateClient(ServerIp, DefaultPort);
        GetTree().NetworkPeer = _peer;
        SetStatus("Connecting...", true);    
    }
    
    
    
    
    private void SetStatus(string text, bool isOk)
    {
        // Simple way to show status.
        if (isOk)
        {
            _statusOk.Text = text;
            _statusFail.Text = "";
        }
        else
        {
            _statusOk.Text = "";
            _statusFail.Text = text;
        }
    }
    private void _hostPressed()  
    {
        _peer = new NetworkedMultiplayerENet();
        _peer.CompressionMode = NetworkedMultiplayerENet.CompressionModeEnum.RangeCoder;
        Error err = _peer.CreateServer(DefaultPort, MaxNumberOfPeers);
        if (err != Error.Ok)
        {
            // Is another server running?
            SetStatus("Can't host, address in use.", false);
            return;
        }

        GetTree().NetworkPeer = _peer;
        SetStatus("Waiting for player...", true);
        
    }

    /*
    
    // Bouton appuyé après avoir entré l'adresse IP du serveur
    // Ancien bouton, maintenant unitilisé à compter du 20/04/2024
    private void _serverConnectPressed()
    {
        _clientPeer = new NetworkedMultiplayerENet();
        _clientPeer.CreateClient(_serverIp, 8910);
        GetTree().NetworkPeer = _clientPeer;
    }

*/
    
    
    
    public override void _Ready()
    {
        // Get nodes - the generic is a class, argument is node path.
        _statusOk = GetNode<Label>("StatusOk");
        _statusFail = GetNode<Label>("StatusFail");

        // Connect all callbacks related to networking.
        // Note: Use snake_case when talking to engine API.
        GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
        GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
        GetTree().Connect("connected_to_server", this, nameof(ConnectedOk));
        GetTree().Connect("connection_failed", this, nameof(ConnectedFail));
        GetTree().Connect("server_disconnected", this, nameof(ServerDisconnected));
    }

    private void PlayerConnected(int id)
    {
        // Someone connected, start the game!
        var pong = ResourceLoader.Load<PackedScene>("res://pong.tscn").Instance();

        // Connect deferred so we can safely erase it from the callback.
        pong.Connect("GameFinished", this, nameof(EndGame), new Godot.Collections.Array(), (int) ConnectFlags.Deferred);

        GetTree().Root.AddChild(pong);
        Hide();
    }

    private void EndGame(string withError = "")
    {
        if (HasNode("/root/Pong"))
        {
            // Erase immediately, otherwise network might show
            // errors (this is why we connected deferred above).
            GetNode("/root/Pong").Free();
            Show();
        }

        GetTree().NetworkPeer = null; // Remove peer.
        _hostButton.Disabled = false;
        _joinButton.Disabled = false;

        SetStatus(withError, false);
    }

    // Callback from SceneTree, only for clients (not server).
    private void ConnectedOk()
    {
        // This function is not needed for this project.
    }

    // Callback from SceneTree, only for clients (not server).
    private void ConnectedFail()
    {
        SetStatus("Couldn't connect", false);

        GetTree().NetworkPeer = null; // Remove peer.
        _hostButton.Disabled = false;
        _joinButton.Disabled = false;
    }

    private void ServerDisconnected()
    {
        EndGame("Server disconnected");
    }
}
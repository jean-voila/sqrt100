using Godot;
using System;


public class MultiplayerMenu : Control
{


    private NetworkedMultiplayerENet _clientPeer;
    private NetworkedMultiplayerENet _serverPeer;
    private const int _port = 8910;
    private string _serverIp = "";
    
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    
    private void _return_pressed_button()
    {
        // Stop the server and client peers
        _serverPeer?.CloseConnection();
        _clientPeer?.CloseConnection();

        // Disconnect them from the network
        GetTree().NetworkPeer = null;

        GD.Print("Stopped everything");
    }
    
    // Creates the server
    private void _createServer()
    {
        _serverPeer = new NetworkedMultiplayerENet();
        var error = _serverPeer.CreateServer(_port, 2);
        if (error != Error.Ok)
        {
            GD.Print("Failed to create server: " + error);
            return;
        }
        if (_serverPeer.GetConnectionStatus() != NetworkedMultiplayerPeer.ConnectionStatus.Connected &&
            _serverPeer.GetConnectionStatus() != NetworkedMultiplayerPeer.ConnectionStatus.Connecting)
        {
            GD.Print("Server is not connecting or connected");
            return;
        }
        GetTree().NetworkPeer = _serverPeer;
    }
    
    
    // Joins the server
    private void _joinServer()
    {
        _clientPeer = new NetworkedMultiplayerENet();
        _clientPeer.CreateClient(_serverIp, _port);
        GetTree().NetworkPeer = _clientPeer;
    }
    
    private void _clientPressed()
    {
        _createServer();
    }
    
    private void _hostPressed()  
    {
        _joinServer();
        
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
        // Make sure the node name matches exactly with the name in your scene
        var serverIpNode = GetNodeOrNull<LineEdit>("ServerIp");
        if (serverIpNode != null)
        {
            _serverIp = serverIpNode.Text;
        }
        else
        {
            GD.Print("ServerIp node not found");
        }

        // Check that the signal and method names are spelled correctly
        GetTree().Connect("connected_to_server", this, nameof(_PlayerConnected));
        GetTree().Connect("server_disconnected", this, nameof(_PlayerDisconnected));
    }

    // 


    private void _PlayerConnected(int id)
    {
        GD.Print("Player with ID: " + id + " has connected.");
        GetTree().ChangeScene("res://maps/mpMap01.tscn");
    }

    private void _PlayerDisconnected(int id)
    {
        GD.Print("Player with ID: " + id + " has disconnected.");
    }
    
    


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

        if (_clientPeer != null)
        {
            GetNode<RichTextLabel>("%ServerIsConnectedDebug").Text = _clientPeer.GetConnectionStatus().ToString();
        }
        if (_serverPeer!=null)
        {
            GetNode<RichTextLabel>("%ClientConnected").Text = _serverPeer.GetConnectionStatus().ToString();
        }
    }
}
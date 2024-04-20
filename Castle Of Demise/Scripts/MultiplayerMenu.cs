using Godot;
using System;


public class MultiplayerMenu : Control
{


    private NetworkedMultiplayerENet _clientPeer;
    private NetworkedMultiplayerENet _serverPeer;
    private string _serverIp = "";
    
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    

    private void _hostPressed()
    {
        _serverPeer = new NetworkedMultiplayerENet();
        _serverPeer.CreateServer(8910, 2);
        GetTree().NetworkPeer = _serverPeer;
        
    }

    private void _clientPressed()
    {
        _clientPeer = new NetworkedMultiplayerENet();
        _clientPeer.CreateClient(_serverIp, 8910);
        GetTree().NetworkPeer = _clientPeer;
        GetNode<Node2D>("%JoinOptions").Visible = !GetNode<Node2D>("%JoinOptions").Visible;
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
        GetTree().Connect("network_peer_connected", this, nameof(_PlayerConnected));
        GetTree().Connect("network_peer_disconnected", this, nameof(_PlayerDisconnected));
    }


    private void _PlayerConnected(int id)
    {
        GD.Print("Player with ID: " + id + " has connected.");
        GetTree().ChangeScene("res://maps/mpMap01.tscn");
    }

    private void _PlayerDisconnected(int id)
    {
    GD.Print("Player with ID: " + id + " has disconnected.");
    }
    private void _server_IP_Input(string ip)
    {
        _serverIp = ip;
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
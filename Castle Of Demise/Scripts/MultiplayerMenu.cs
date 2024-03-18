using Godot;
using System;


public class MultiplayerMenu : Control
{


    private NetworkedMultiplayerENet clientPeer = new NetworkedMultiplayerENet();
    private string serverIP = "";
    
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void _hostPressed()
    {
        var serverPeer = new NetworkedMultiplayerENet();
        serverPeer.CreateServer(8910, 2);
        GetTree().NetworkPeer = serverPeer;

        


    }

    private void _clientPressed()
    {
        GetNode<Node2D>("%JoinOptions").Visible = !GetNode<Node2D>("%JoinOptions").Visible;
        

    }

    // Bouton appuyé après avoir entré l'adresse IP du serveur
    private void _serverConnectPressed()
    {
        
        
        clientPeer.CreateClient(serverIP, 8910);
        GetTree().NetworkPeer = clientPeer;

        



    }

    private void _server_IP_Input(string ip)
    {
        serverIP = ip;
    }
    

    private void _back()
    {
        GetTree().ChangeScene("res://menus/GamemodeMenu.tscn");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
      
        if (clientPeer.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connecting)
        {
            GetNode<RichTextLabel>("%ServerIsConnectedDebug").Text = "c en train d'être bon frerot";
        } else if (clientPeer.GetConnectionStatus() == NetworkedMultiplayerPeer.ConnectionStatus.Connected)
        {
            GetNode<RichTextLabel>("%ServerIsConnectedDebug").Text = "c bon frerot";

        }
        else
        {
            GetNode<RichTextLabel>("%ServerIsConnectedDebug").Text = "c  pas bon frerot";
        }
    }
}
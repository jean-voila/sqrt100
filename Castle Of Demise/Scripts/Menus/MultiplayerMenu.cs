using CastleOfDemise.mobs.Player;
using CastleOfDemise.Scripts.Menus.MultiLauncher;
using Godot;

namespace CastleOfDemise.Scripts.Menus
{


    public partial class MultiplayerMenu : Control
    {

        [Export] private int _port = 8910;
        private string _address = "127.0.0.1";
        private string _name = "Player";
        private TextEdit _codeToJoin;
        private TextEdit _nameToJoin;
        private ENetMultiplayerPeer _peer;

// Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Multiplayer.PeerConnected += PeerConnected;
            Multiplayer.PeerDisconnected += PeerDisconnected;
            Multiplayer.ConnectedToServer += ConnectedToServer;
            Multiplayer.ConnectionFailed += ConnectionFailed;
            _codeToJoin = GetNode<TextEdit>("%CodeToJoin");
            _nameToJoin = GetNode<TextEdit>("%PlayerName");

        }

        private void ConnectionFailed()
        {
            GD.Print("CONNECTION FAILED");
        }

        private void ConnectedToServer()
        {
            GD.Print("CONNECTED TO SERVER");
            RpcId(1, SendPlayerInformation($"Player pls", Multiplayer.GetUniqueId()));
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
                if (_nameToJoin.Text != "") 
                {
                    GetNode<Button>("%SceneJoinButton").Disabled = false;
                    GetNode<Button>("%SceneHostButton").Disabled = false;
                    _name = _nameToJoin.Text;
                    try
                    {
                        _address = CodeParser.CodeToIp(_codeToJoin.Text);
                        GetNode<Button>("%SceneJoinButton").Disabled = false;
                    }
                    catch
                    {
                        GetNode<Button>("%SceneJoinButton").Disabled = true;
                    }
                }
                else
                {
                    GetNode<Button>("%SceneJoinButton").Disabled = true;
                    GetNode<Button>("%SceneHostButton").Disabled = true;
                }
        }
        private void _clientPressed()
        {
            _peer = new ENetMultiplayerPeer();
            _peer.CreateClient(_address, _port);
            _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
            Multiplayer.MultiplayerPeer = _peer;
            GD.Print("JOINING GAME...");
        }

        private void _hostPressed()
        {
            GetNode<Control>("%MultiplayerMenu").Hide();
            GetNode<Control>("%SetupGameAsHost").Show();

            _peer = new ENetMultiplayerPeer();
            var error = _peer.CreateServer(_port, 2);
            if (error != Error.Ok)
            {
                GD.Print("ERROR CANNOT HOST: " + error.ToString());
                return;
            }

            _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
            Multiplayer.MultiplayerPeer = _peer;
            GD.Print("WAITING FOR PLAYERS!");

            var multiplayerMenu = GetNode<Control>("%MultiplayerMenu");
            if (multiplayerMenu == null)
            {
                GD.Print("ERROR: MultiplayerMenu node not found");
                return;
            }

            SendPlayerInformation(_name, Multiplayer.GetUniqueId());
        }

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true,
            TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        public StringName SendPlayerInformation(string name, int id)
        {
            if (!GameManager.Players.Exists(x => x.PlayerId == id))
            {
                var player = new Player();
                player.PlayerName = name;
                player.PlayerId = id;
                GameManager.Players.Add(player);
            }

            if (!Multiplayer.IsServer()) return null;
            {
                foreach (var player in GameManager.Players)
                {
                    // Prevent the server from calling Rpc on SendPlayerInformation for itself
                    if (player.PlayerId != Multiplayer.GetUniqueId())
                    {
                        var multiplayerMenu = (MultiplayerMenu)GetNode("%MultiplayerMenu");
                        multiplayerMenu.Rpc(nameof(multiplayerMenu.SendPlayerInformation), player.PlayerName,
                            player.PlayerId);
                    }
                }
            }

            return null;
        }
       
        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        public void StartGame()
        {
            GetTree().ChangeSceneToFile("res://maps/mpMap01.tscn");
            
        }
    }
    
    

    // test




}

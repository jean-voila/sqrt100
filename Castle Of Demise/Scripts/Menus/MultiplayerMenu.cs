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
        private ENetMultiplayerPeer _peer;

// Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Multiplayer.PeerConnected += PeerConnected;
            Multiplayer.PeerDisconnected += PeerDisconnected;
            Multiplayer.ConnectedToServer += ConnectedToServer;
            Multiplayer.ConnectionFailed += ConnectionFailed;
            _codeToJoin = GetNode<TextEdit>("%CodeToJoin");

        }

        private void ConnectionFailed()
        {
            GD.Print("CONNECTION FAILED");
        }
        
        private void ConnectedToServer()
        {
            GD.Print("CONNECTED TO SERVER");
            // Call SendPlayerInformation on the server
            RpcId(1, nameof(SendPlayerInformation), $"Client", Multiplayer.GetUniqueId(), false, false);
            GD.Print("SENDING PLAYER INFORMATION...");
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

            SendPlayerInformation("Host", Multiplayer.GetUniqueId());
        }

        [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
        private void SendPlayerInformation(string name, int id, bool recursive = true, bool isHost = true)
        {
            GD.Print("SendPlayerInformation, beginning...");
            if (!GameManager.Players.ContainsValue(new Player {PlayerId = id}))
            {
                var player = new Player();
                player.PlayerName = name;
                player.PlayerId = id;
                player.PlayerScore = 0;
                if (isHost) GameManager.Players[0] = player;
                else GameManager.Players[1] = player;
                GD.Print(player.PlayerId + " " + player.PlayerName + " " + player.PlayerScore);
            }

            GD.Print("SendPlayerInformation, player added...");

            if (Multiplayer.IsServer() || !recursive)
            {
                GD.Print("SendPlayerInformation, server found...");
                GD.Print("===== LIST OF PLAYERS =====");

                foreach (var player in GameManager.Players)
                {
                    GD.Print(
                        $"Player ID: {player.Key}, Player Name: {player.Value.PlayerName}, Player Score: {player.Value.PlayerScore}");
                    // Call Rpc instead of RpcId
                    Rpc(nameof(AddPlayerToAllPeers), player.Value.PlayerName, player.Value.PlayerId, isHost);
                }

                GD.Print("===== END OF LIST =====");
            }
            GD.Print($"SendPlayerInformation, end of function with {GameManager.Players.Count} players...");
        }

        [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
        private void AddPlayerToAllPeers(string name, int id, bool isHost)
        {
            if (!GameManager.Players.ContainsValue(new Player {PlayerId = id}))
            {
                var player = new Player();
                player.PlayerName = name;
                player.PlayerId = id;
                player.PlayerScore = 0;
                if (isHost) GameManager.Players[0] = player;
                else GameManager.Players[1] = player;
                GD.Print("After adding to the peers");
                GD.Print(player.PlayerId + " " + player.PlayerName + " " + player.PlayerScore);

            }
        }

        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
        public void StartGame()
        {
            GetTree().ChangeSceneToFile("res://maps/mpMap02.tscn");
            
        }
    }
    
    

    // test




}

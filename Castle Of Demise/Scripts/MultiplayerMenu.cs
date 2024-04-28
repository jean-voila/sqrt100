using Godot;

namespace CastleOfDemise.Scripts
{
    public class CodeParser 
    {


        public const string basicIP = "127.0.0.1";


        public static string IpToCode(string ip)
        {
            if (ip.ToLower() == "localhost")
            {
                // on a le localhost, donc on transfère directement en code sans caulculer
                return "1MCCIR5T";
            }

            string[] ipParts = ip.Split('.');
            if (ipParts.Length != 4)
            {
                // on a pas 4 parties, donc on retourne une erreur
                return basicIP;
            }

            long ipValue = 0;
            for (int i = 0; i < 4; i++)
            {
                if (int.Parse(ipParts[i]) > 255 || int.Parse(ipParts[i]) < 0)
                {
                    // Vu que un octet ne peut pas être plus grand que 255, on retourne une erreur
                    return basicIP;
                }

                ipValue *= 1000;
                ipValue += int.Parse(ipParts[i]);
                // Console.WriteLine($"Converting {ip}, stage {i} =" + ipValue);
            }

            string code = Base36Converter.ConvertTo(ipValue);
            if (code.Length < 8)
            {
                // Tous les codes font exactement 8 caractères, donc tout va bien
                code = code.PadLeft(8, '0');
            }

            return code;

        }

        public static string CodeToIp(string code)
        {
            if (code.ToUpper() == "1MCCIR5T")
            {
                return basicIP;
            }

            if (code.Length != 8)
            {
                // Tous les codes font exactement 8 caractères, donc si on en a pas 8, on retourne une erreur
                return basicIP;
            }

            long ipValue = Base36Converter.ConvertFrom(code);

            // on check si les ip trouvées sont valide

            if (ipValue >= 10000000000 && ipValue <= 10255255255 ||
                ipValue >= 172016000000 && ipValue <= 172031255255 ||
                ipValue >= 192168000000 && ipValue <= 192168255255)
            {
                string ip = "";
                for (int i = 0; i < 4; i++)
                {
                    ip = (ipValue % 1000) + "." + ip;
                    ipValue /= 1000;
                }

                string[] ipParts = ip.Split('.');

                for (int i = 0; i < 4; i++)
                {
                    if (int.Parse(ipParts[i]) > 255 || int.Parse(ipParts[i]) < 0)
                    {
                        // Je viens de copier cette partie qui va check individuellement si l'ip est valide, donc il y a pas de problèmes normalement
                        return basicIP;
                    }
                }

                return ip.Substring(0, ip.Length - 1);
            }

            return basicIP;
            // 




        }
    } //touchez pas ca marche bien
    public static class Base36Converter  // touchez pas ca svp ca marche bien
    {
        public const int Base = 36;
        public const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ConvertTo(long value)
        {
            string result = "";

            while (value > 0)
            {
                result = Chars[(int)(value % Base)] + result;
                value /= Base;
            }

            return result;
        }

        public static long ConvertFrom(string value)
        {
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                result *= Base;
                result += Chars.IndexOf(value[i]);
            }

            return result;
        }
    }


    public class MultiplayerMenu : Control
    {
    
        private NetworkedMultiplayerENet _peer;
        private Label _statusOk;
        private Label _statusFail;
        private Button _hostButton;
        private Button _joinButton;
        private LineEdit _address;
    
    
        private const int DefaultPort = 8910;
        private const int MaxNumberOfPeers = 2;
        public string ServerIp;

        public MultiplayerMenu(string serverIp)
        {
            ServerIp = serverIp;
        }
        public MultiplayerMenu()
        {
            ServerIp = "127.0.0.1"; // Default IP
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
            string ip = CodeParser.CodeToIp(_address.Text);
            if (!ip.IsValidIPAddress())
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
            //hides the button?
            /*
            _hostButton.Disabled = true;
            _joinButton.Disabled = true;
            */
            SetStatus($"Code: {CodeParser.IpToCode(ServerIp)} \n Waiting for player...", true);
        
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
    
        public override void _Ready()
        {
            // Get nodes - the generic is a class, argument is node path.
            _statusOk = GetNode<Label>("ConnectionControl/StatusOk");
            _statusFail = GetNode<Label>("ConnectionControl/StatusFail");
            _address = GetNode<LineEdit>("ConnectionControl/Address");
            _hostButton = GetNode<Button>("ConnectionControl/HostButton");
            _joinButton = GetNode<Button>("ConnectionControl/JoinButton");
            // Connect all callbacks related to networking.
            // Note: Use snake_case when talking to engine API.
            GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
            GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
            GetTree().Connect("connected_to_server", this, nameof(ConnectedOk));
            GetTree().Connect("connection_failed", this, nameof(ConnectedFail));
            GetTree().Connect("server_disconnected", this, nameof(ServerDisconnected));
        
            // test?
            var returnButton = GetNode<Button>("BackButton");
            if (returnButton != null)
            {
                returnButton.Connect("pressed", this, nameof(_return_pressed_button));
            }
            else
            {
                GD.Print("BackButton node not found");
            }
        }

        private void PlayerConnected(int id)
        {
            // Someone connected, start the game!
            var pong = ResourceLoader.Load<PackedScene>("res://maps/mpMap01.tscn").Instance();

            // Connect deferred so we can safely erase it from the callback.
            pong.Connect("GameFinished", this, nameof(EndGame), new Godot.Collections.Array(), (int) ConnectFlags.Deferred);

            GetTree().Root.AddChild(pong);
            Hide();
        }
    
        private void PlayerDisconnected(int id)
        {
            EndGame(GetTree().IsNetworkServer() ? "Client disconnected" : "Server disconnected");
        }
    
    
        // FONCTIONNE PAS POUR LE MOMENT PAUL CA SERA PROBABLEMENT A TOI DE GERER LA FIN DE LA PARTIE (fais gaffe au truc pong mdr)

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
            // shows the button?
            /*
            _hostButton.Disabled = false;
            _joinButton.Disabled = false;
            */

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
        }

        private void ServerDisconnected()
        {
            EndGame("Server disconnected");
        }
    }
}
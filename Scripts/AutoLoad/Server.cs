using Godot;
using Godot.Collections;


namespace NightFallServersUtils.Scripts.AutoLoad
{
    public class DefaultNetworkedServer : Node
    {

        private static DefaultNetworkedServer singleton;
        public static DefaultNetworkedServer Singleton => singleton;
        private NetworkedMultiplayerENet serverPeer;


        public DefaultNetworkedServer()
        {
            singleton = this;

            serverPeer = new NetworkedMultiplayerENet();
        }

        public override void _EnterTree()
        {
            SetupDTLS();
            ValidateEnvironmentVariables();
            GetTree().Connect("network_peer_connected", this, nameof(PeerConnected));
            GetTree().Connect("network_peer_disconnected", this, nameof(PeerDisconnected));
        }

        public override void _Ready()
        {
            var port = DefaultServerConfiguration.Singleton.GetPort(defaultPort: 4444);
            var maxClients = DefaultServerConfiguration.Singleton.GetMaxClients(defaultMaxClients: 2);
            serverPeer.CreateServer(port, maxClients);
            GetTree().NetworkPeer = serverPeer;
        }

        public int GetRpcSenderId()
        {
            return GetTree().GetRpcSenderId();
        }

        public void DisconnectPeer(int id, bool now = false)
        {
            serverPeer.DisconnectPeer(id, now);
        }

        /// Loads and sets certificate and key for ENet connection.
        private void SetupDTLS()
        {
            serverPeer.DtlsVerify = false;
            serverPeer.UseDtls = true;
            var pathToDTLS = "user://DTLS";
            if (DirExists(pathToDTLS))
            {
                var x509Cert = new X509Certificate();
                var certificateFile = pathToDTLS.PlusFile("basic.crt");
                var error = x509Cert.Load(certificateFile);
                if (error != Error.Ok)
                {
                    Logger.Server.Error($"Could not load certificate file {ProjectSettings.GlobalizePath(certificateFile)}. Error code: {error}");
                    GetTree().Quit(-(int)error);
                    return;
                }
                serverPeer.SetDtlsCertificate(x509Cert);

                var cryptoKey = new CryptoKey();
                var keyFile = pathToDTLS.PlusFile("basic.key");
                error = cryptoKey.Load(keyFile);
                if (error != Error.Ok)
                {
                    Logger.Server.Error($"Could not load key file {ProjectSettings.GlobalizePath(keyFile)}. Error code: {error}");
                    GetTree().Quit(-(int)error);
                    return;
                }
                serverPeer.SetDtlsKey(cryptoKey);
            }
            else
            {
                Logger.Server.Error($"Directory {ProjectSettings.GlobalizePath(pathToDTLS)} doesn't exist!. Abording");
                GetTree().Quit(-(int)Error.FileBadPath);
            }
        }

        private bool DirExists(string path)
        {
            var dir = new Directory();
            return dir.DirExists(path);
        }
        
        private void ValidateEnvironmentVariables()
        {
            var environmentVariables = new[] { "GATEWAY_TOKEN", "GAME_SERVER_TOKEN" };
            foreach (var environmentVariable in environmentVariables)
            {
                if (OS.GetEnvironment(environmentVariable).Length == 0)
                {
                    Logger.Server.Error($"Environment varianle {environmentVariable} is not set. Abording...");
                    GetTree().Quit(-(int)Error.PrinterOnFire);
                }
            }
        }

        private void PeerConnected(int id)
        {
            Logger.Server.Info($"Peer {id} has connected");
        }

        private void PeerDisconnected(int id)
        {
            Logger.Server.Info($"Peer {id} has diconnected");
        }
    }
}
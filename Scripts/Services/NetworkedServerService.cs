using Godot;

using ServersUtils.Scripts.Loaders;
using ServersUtils.Scripts.Logging;

using SharedUtils.Scripts.Common;
using SharedUtils.Scripts.Loaders;
using SharedUtils.Scripts.Services;

namespace ServersUtils.Scripts.Services
{
    public abstract class NetworkedServerService : Service
    {
        private readonly NetworkedMultiplayerENet serverPeer;


        protected NetworkedServerService()
        {
            serverPeer = new NetworkedMultiplayerENet();
        }

        public override void _EnterTree()
        {
            SetupDTLS();
        }

        protected void CreateServer(int port, int maxClients)
        {
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
        protected void SetupDTLS()
        {
            serverPeer.DtlsVerify = false;
            serverPeer.UseDtls = true;
            
            var pathToDTLS = "user://DTLS";
            if (DirectoryUtils.DirExists(pathToDTLS))
            {
                Error error;

                serverPeer.SetDtlsCertificate(X509CertificateLoader.Load(pathToDTLS, "ag.crt", out error));
                QuitOnError((int)error);

                serverPeer.SetDtlsKey(CryptoKeyLoader.Load(pathToDTLS, "ag.key", out error));
                QuitOnError((int)error);
            }
            else
            {
                Logger.Server.Error($"Directory {ProjectSettings.GlobalizePath(pathToDTLS)} doesn't exist!. Abording");
                QuitOnError((int)Error.FileBadPath);
            }
        }
    }
}
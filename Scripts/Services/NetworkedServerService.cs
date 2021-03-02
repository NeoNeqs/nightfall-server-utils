using Godot;
using NightFallServersUtils.Scripts.Common;
using NightFallServersUtils.Scripts.Loaders;
using NightFallServersUtils.Scripts.Logging;

namespace NightFallServersUtils.Scripts.Services
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
                Quit((int)error);

                serverPeer.SetDtlsKey(CryptoKeyLoader.Load(pathToDTLS, "ag.key", out error));
                Quit((int)error);
            }
            else
            {
                Logger.Server.Error($"Directory {ProjectSettings.GlobalizePath(pathToDTLS)} doesn't exist!. Abording");
                Quit((int)Error.FileBadPath);
            }
        }
    }
}
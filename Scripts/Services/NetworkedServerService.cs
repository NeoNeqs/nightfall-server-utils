using Godot;

using ServersUtils.Scripts.Loaders;

using SharedUtils.Scripts.Loaders;
using SharedUtils.Scripts.Services;

namespace ServersUtils.Scripts.Services
{
    public abstract class NetworkedServerService : NetworkedPeerService
    {
        private static NetworkedServerService _singleton;
        public static NetworkedServerService Singleton => _singleton;


        protected NetworkedServerService() : base()
        {
            _singleton = this;
        }

        public override void _EnterTree()
        {
            base._EnterTree();
        }

        protected Error CreateServer(int port, int maxClients)
        {
            var creationError = _peer.CreateServer(port, maxClients);
            base.Create();
            return creationError;
        }

        public int GetRpcSenderId()
        {
            return GetTree().GetRpcSenderId();
        }

        public void DisconnectPeer(int id, bool now = false)
        {
            _peer.DisconnectPeer(id, now);
        }

        /// Loads and sets certificate and key for ENet connection.
        protected override void SetupDTLS(string path)
        {
            base.SetupDTLS(path);
            Error error;

            _peer.SetDtlsCertificate(X509CertificateLoader.Load(path, "ag.crt", out error));
            QuitIfError((int)error);

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, "ag.key", out error));
            QuitIfError((int)error);

            return;
        }
    }
}
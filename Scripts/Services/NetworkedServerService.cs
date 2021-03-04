using ServersUtils.Scripts.Exceptions;
using ServersUtils.Scripts.Loaders;

using SharedUtils.Scripts.Exceptions;
using SharedUtils.Scripts.Common;
using SharedUtils.Scripts.Loaders;
using SharedUtils.Scripts.Services;

namespace ServersUtils.Scripts.Services
{
    public abstract class NetworkedServerService : NetworkedPeerService
    {

        protected NetworkedServerService() : base()
        {
        }

        protected ErrorCode CreateServer(int port, int maxClients)
        {
            var creationError = _peer.CreateServer(port, maxClients);
            base.Create();
            return (ErrorCode)((int)creationError);
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

            ErrorCode error;
            _peer.SetDtlsCertificate(X509CertificateLoader.Load(path, GetCertificateName(), out error));
            if (error != ErrorCode.Ok)
            {
                throw new X509CertificateNotFoundException(path);
            }

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, GetCryptoKeyName(), out error));
            if (error != ErrorCode.Ok)
            {
                throw new CryptoKeyNotFoundException(path);
            }

        }

        protected abstract void PeerConnected(int id);
        protected abstract void PeerDisconnected(int id);

        protected override void ConnectSignals(NetworkedPeerService to)
        {
            GetTree().Connect("network_peer_connected", to, nameof(PeerConnected));
            GetTree().Connect("network_peer_disconnected", to, nameof(PeerDisconnected));
        }
    }
}
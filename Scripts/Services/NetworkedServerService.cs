using ServersUtils.Scripts.Loaders;

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
        protected override ErrorCode SetupDTLS(string path)
        {
            ErrorCode error = base.SetupDTLS(path);
            if (error != ErrorCode.Ok)
            {
                return error;
            }

            _peer.SetDtlsCertificate(X509CertificateLoader.Load(path, GetCertificateName(), out error));
            if (error != ErrorCode.Ok)
            {
                return error;
            }

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, GetCryptoKeyName(), out error));
            if (error != ErrorCode.Ok)
            {
                return error;
            }

            return ErrorCode.Ok;
        }

    }
}
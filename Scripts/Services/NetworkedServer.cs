using Godot;

using ServersUtils.Exceptions;
using ServersUtils.Loaders;

using SharedUtils.Exceptions;
using SharedUtils.Common;
using SharedUtils.Loaders;
using SharedUtils.Services;

namespace ServersUtils.Services
{
    public abstract class NetworkedServer : NetworkedPeer
    {
        protected NetworkedServer() : base()
        {
        }

        public override void _EnterTree()
        {
            base._EnterTree();
            SetupDTLS();

        }

        public override void _Ready()
        {
            ConnectSignals();
        }

        public override void _Process(float delta) => base._Process(delta);

        protected ErrorCode CreateServer(int port, int maxClients)
        {
            var creationError = _peer.CreateServer(port, maxClients);
            base.Create();
            return (ErrorCode)((int)creationError);
        }

        public int GetRpcSenderId() => CustomMultiplayer.GetRpcSenderId();

        public void DisconnectPeer(int id, bool now = false) => _peer.DisconnectPeer(id, now);

        /// Loads and sets certificate and key for ENet connection.
        private void SetupDTLS()
        {
            string path = "user://DTLS/";
            SetupDTLS(path);

            ErrorCode error;
            _peer.SetDtlsCertificate(X509CertificateLoader.Load(path, GetCertificateName(), out error));
            if (error != ErrorCode.Ok)
            {
                var errorMessage = $"Failed to load x509 certificate from '{path.PlusFile(GetCertificateName())}'";
                GD.LogError(errorMessage);
                throw new X509CertificateNotFoundException(errorMessage);
            }

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, GetCryptoKeyName(), out error));
            if (error != ErrorCode.Ok)
            {
                var errorMessage = $"Failed to load crypto key from '{path.PlusFile(GetCryptoKeyName())}'";
                GD.LogError(errorMessage);
                throw new CryptoKeyNotFoundException(errorMessage);
            }

        }

        protected override void ConnectSignals()
        {
            CustomMultiplayer.Connect("network_peer_connected", this, nameof(PeerConnected));
            CustomMultiplayer.Connect("network_peer_disconnected", this, nameof(PeerDisconnected));
        }

        protected abstract string GetCryptoKeyName();
        protected abstract void PeerConnected(int id);
        protected abstract void PeerDisconnected(int id);
    }
}
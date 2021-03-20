using Godot;

using ServersUtils.Exceptions;
using ServersUtils.Loaders;

using SharedUtils.Common;
using SharedUtils.Services;

namespace ServersUtils.Services
{
    public abstract class NetworkedServer<T> : NetworkedPeer<T> where T : Node
    {
        protected NetworkedServer() : base() { }


        public override void _EnterTree()
        {
            base._EnterTree();
            _ = SetupDTLS();
        }

        public override void _Ready() => base._Ready();

        public override void _Process(float delta) => base._Process(delta);

        protected ErrorCode CreateServer(int port, int maxClients)
        {
            var creationError = _peer.CreateServer(port, maxClients);
            base.Create();
            return (ErrorCode)((int)creationError);
        }

        public void DisconnectPeer(int id, bool now = false) => _peer.DisconnectPeer(id, now);

        /// Loads and sets certificate and key for ENet connection.
        protected override string SetupDTLS()
        {
            string path = base.SetupDTLS();

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, GetCryptoKeyName(), out ErrorCode error));
            if (error != ErrorCode.Ok)
                throw new CryptoKeyNotFoundException($"Failed to load crypto key from '{path.PlusFile(GetCryptoKeyName())}'");

            return path;
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
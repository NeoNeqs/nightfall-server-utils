using Godot;

using ServersUtils.Exception;
using ServersUtils.Loaders;

using SharedUtils.Common;
using SharedUtils.Logging;
using SharedUtils.Networking;

namespace ServersUtils.Networking
{
    public abstract class NetworkedServer : NetworkedPeer
    {
        protected int RpcSenderId => CustomMultiplayer.GetRpcSenderId();
        protected string RpcSenderIp => GetIpAddressOfPeer(RpcSenderId);

        protected override void Create()
        {
            _ = _peer.CreateServer(GetPort(), GetMaxClients());
        }

        // TODO: tell the peer why it got disconnected.
        public void DisconnectPeer(int id, bool now = false)
        {
            _peer.DisconnectPeer(id, now);
        }

        /// Loads and sets certificate and key for secure ENet connection.
        protected override string SetupDTLS()
        {
            string path = base.SetupDTLS();

            _peer.SetDtlsKey(CryptoKeyLoader.Load(path, GetCryptoKeyName(), out ErrorCode error));
            if (error != ErrorCode.Ok)
                throw new CryptoKeyNotFoundException(path.PlusFile(GetCryptoKeyName()));

            return path;
        }

        protected string GetIpAddressOfPeer(int id)
        {
            return _peer.GetPeerAddress(id);
        }

        protected override void ConnectSignals()
        {
            CustomMultiplayer.Connect("network_peer_connected", this, nameof(PeerConnected));
            CustomMultiplayer.Connect("network_peer_disconnected", this, nameof(PeerDisconnected));
        }

        protected virtual void PeerConnected(int id)
        {
            Logger.Info($"Peer {id} has connected");
        }

        protected virtual void PeerDisconnected(int id)
        {
            Logger.Info($"Peer {id} has disconnected");
        }

        public new void Send(int peerId, object @object)
        {
            base.Send(peerId, @object);
        }

        protected abstract string GetCryptoKeyName();
    }
}
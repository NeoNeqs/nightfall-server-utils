using Godot;

using ServersUtils.Exceptions;
using ServersUtils.Loaders;

using SharedUtils.Common;
using SharedUtils.Logging;
using SharedUtils.Networking;
using SharedUtils.Services;

namespace ServersUtils.Services
{
    public abstract class NetworkedServer<T> : NetworkedPeer<T> where T : Node
    {
        protected int RpcSenderId => CustomMultiplayer.GetRpcSenderId();
        protected string RpcSenderIp => GetIpAddressOfPeer(RpcSenderId);

        protected override void Create()
        {
            _ = _peer.CreateServer(GetPort());
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

        protected abstract string GetCryptoKeyName();

        protected virtual void PeerConnected(int id)
        {
            Logger.Info($"Peer {id} has connected");
        }

        protected virtual void PeerDisconnected(int id)
        {
            Logger.Info($"Peer {id} has disconnected");
        }

        private void Send(int peerId, object[] args)
        {
            _ = RpcId(peerId, nameof(PacketReceived), args);
        }

        protected void Send(int peerId, PacketType packetType, object arg1)
        {
            Send(peerId, new[] { packetType, arg1 });
        }

        protected void Send(int peerId, PacketType packetType, object arg1, object arg2)
        {
            Send(peerId, new[] { packetType, arg1, arg2 });
        }

        protected void Send(int peerId, PacketType packetType, object arg1, object arg2, object arg3)
        {
            Send(peerId, new[] { packetType, arg1, arg2, arg3 });
        }

        protected void Send(int peerId, PacketType packetType, object arg1, object arg2, object arg3, object arg4)
        {
            Send(peerId, new[] { packetType, arg1, arg2, arg3, arg4 });
        }

        protected void Send(int peerId, PacketType packetType, object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            Send(peerId, new[] { packetType, arg1, arg2, arg3, arg4, arg5 });
        }
    }
}
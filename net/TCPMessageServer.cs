using Godot;
using Nightfall.SharedUtils.InfoCodes;
using Nightfall.SharedUtils.Logging;
using Nightfall.SharedUtils.Net.Messaging;
using System.Collections.Generic;

namespace Nightfall.ServerUtils.Net
{
    public sealed partial class TCPMessageServer : TCPServer
    {
        [Signal]
        public delegate void MessageReceived(NFGuid guid, Message message, long error);

        private readonly Dictionary<NFGuid, StreamPeerTCP> _peers = new();
        private readonly Queue<NFGuid> _peersToRemove = new();

        private readonly Thread _thread = new();
        private readonly Mutex _mutex = new();

        private static readonly Logger NetLogger = NFInstance.GetLogger("Net");

        private bool _stop;

        public NFError Listen(ushort port, Thread.Priority priority = Thread.Priority.Normal)
        {
            _stop = false;

            var error = base.Listen(port);

            _thread.Start(this, nameof(ThreadFunction), (byte) 0, priority);
            NetLogger.Info("Starting the thread function.");
            return error;
        }

        private void ThreadFunction(object obj)
        {
            while (true)
            {
                _mutex.Lock();

                if (_stop)
                {
                    NetLogger.Info("Stopping the thread function.");
                    
                    _mutex.Unlock();
                    return;
                }

                if (IsConnectionAvailable())
                {
                    var guid = NFGuid.NewGuid();
                    NetLogger.Info($"Peer {guid.Guid.ToString()} connected.");
                    _peers.Add(guid, TakeConnection());
                }

                foreach (var (nfGuid, peer) in _peers)
                {
                    // Little do you know that this method actually has side effects and does not simply return the current peer status (some polling is done).
                    _ = peer.GetStatus();
                    // This method on the other hand does return the correct "status" (technically a true-false state) and checks if socket (is open or not) thought connection needs to pooled before.
                    if (!peer.IsConnectedToHost())
                    {
                        NetLogger.Info($"Peer {nfGuid.Guid.ToString()} disconnected. Queueing its deletion.");
                        _peersToRemove.Enqueue(nfGuid);
                        continue;
                    }

                    if (peer.GetAvailableBytes() == 0)
                    {
                        continue;
                    }

                    var (message, error) = Message.Create(peer);
                    if (error == NFError.Ok)
                    {
                        NetLogger.Info($"Peer {nfGuid.Guid.ToString()} sent a message.");
                    }

                    EmitSignal(nameof(MessageReceived), nfGuid, message, (long) error);
                }

                for (var i = 0; i < _peersToRemove.Count; i++)
                {
                    NetLogger.Info($"Removing peer {_peersToRemove.Peek().Guid.ToString()}");
                    _ = _peers.Remove(_peersToRemove.Dequeue(), out var peer);
                    peer.DisconnectFromHost();
                }

                _mutex.Unlock();
            }
        }

        public NFError SendMessage(NFGuid nfGuid, Message message)
        {
            if (!_peers.ContainsKey(nfGuid)) return NFError.PeerNotFound;
            var (data, error) = message.Serialize();

            if (error == NFError.Ok)
            {
                _peers[nfGuid].PutData(data);
            }

            return error;
        }

        public new void Stop()
        {
            _mutex.Lock();
            _stop = true;
            _mutex.Unlock();

            _thread.WaitToFinish();
            base.Stop();
        }

        public event MessageReceived MessageReceivedEvent
        {
            add => Connect(nameof(MessageReceived), new Callable(value));
            remove => Disconnect(nameof(MessageReceived), new Callable(value));
        }
    }
}
using Godot;

namespace NightFallServersUtils.Scripts.Configurations
{
    public abstract class ServerConfiguration : Configuration
    {
        private const string Path = "user://config/config.ini";
       
        protected ServerConfiguration() : base()
        {
        }

        public override void _EnterTree()
        {
            LoadConfiguration(Path);
        }

        public int GetPort(int defaultPort)
        {
            return GetValue<int>("NETWORKING", "port", defaultPort);
        }

        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue<int>("NETWORKING", "max_clients", defaultMaxClients);
        }

        public override void _ExitTree()
        {
            SaveConfiguration(Path);
        }
    }
}
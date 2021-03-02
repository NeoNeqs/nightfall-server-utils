using Godot;

namespace NightFallServersUtils.Scripts.Configurations
{
    public abstract class StandartServerConfiguration : StandartConfiguration
    {
       
        protected StandartServerConfiguration() : base()
        {
        }

        public override void _EnterTree()
        {
            LoadConfiguration();
        }

        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue<int>("NETWORKING", "max_clients", defaultMaxClients);
        }

        public override void _ExitTree()
        {
            SaveConfiguration();
        }
    }
}
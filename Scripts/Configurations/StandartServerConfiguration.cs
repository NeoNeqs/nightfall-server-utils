using SharedUtils.Scripts.Configurations;

namespace ServersUtils.Scripts.Configurations
{
    public abstract class StandartServerConfiguration : StandartConfiguration
    {
       
        protected StandartServerConfiguration() : base()
        {
        }

        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue<int>("NETWORKING", "max_clients", defaultMaxClients);
        }
    }
}
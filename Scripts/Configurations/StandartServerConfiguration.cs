using SharedUtils.Configurations;

namespace ServersUtils.Configurations
{
    public abstract class StandartServerConfiguration : StandartConfiguration
    {
       
        protected StandartServerConfiguration() : base()
        {
        }

        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue("NETWORKING", "max_clients", defaultMaxClients);
        }
    }
}
using SharedUtils.Configuration;

namespace ServersUtils.Configurations
{
    public abstract class StandartServerConfiguration : StandartConfiguration
    {
        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue("NETWORKING", "max_clients", defaultMaxClients);
        }
    }
}
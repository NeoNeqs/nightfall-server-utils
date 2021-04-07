using Godot;
using SharedUtils.Configurations;

namespace ServersUtils.Configurations
{
    public abstract class StandartServerConfiguration<T> : StandartConfiguration<T> where T : Node
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
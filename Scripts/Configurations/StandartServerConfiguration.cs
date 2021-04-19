using Godot;
using SharedUtils.Configuration;

namespace ServersUtils.Configurations
{
    public abstract class StandartServerConfiguration<T> : StandartConfiguration<T> where T : Node
    {
        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue("NETWORKING", "max_clients", defaultMaxClients);
        }

        public abstract int GetMaxClients();
    }
}
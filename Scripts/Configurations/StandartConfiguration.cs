namespace NightFallServersUtils.Scripts.Configurations
{
    public abstract class StandartConfiguration : Configuration
    {
        protected StandartConfiguration() : base()
        {
        }

        public int GetPort(int defaultPort)
        {
            return GetValue<int>("NETWORKING", "port", defaultPort);
        }

    }
}
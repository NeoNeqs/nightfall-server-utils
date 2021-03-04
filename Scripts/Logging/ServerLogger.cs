using SharedUtils.Scripts.Logging;

namespace ServersUtils.Scripts.Logging
{
    public sealed class ServerLogger : BasicLogger
    {
        // one instance of LoggerFile for every Logger instance just in case
        private static ServerLogger _singleton = new ServerLogger();
        public static ServerLogger GetSingleton() => _singleton;

        public ServerLogger() : base("SERVER")
        {
        }
    }
}
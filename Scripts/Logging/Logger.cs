using SharedUtils.Scripts.Logging;

namespace ServersUtils.Scripts.Logging
{
    public sealed class Logger : SharedUtils.Scripts.Logging.Logger
    {
        // one instance of LoggerFile for every Logger instance just in case
        private static BasicLogger _server;
        public static BasicLogger Server => _server;

        public Logger()
        {
            // _loggerFile passed by reference to always write to the same file
            _server = new BasicLogger(ref _loggerFile, "SERVER");
        }
    }
}
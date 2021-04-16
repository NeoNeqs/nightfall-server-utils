using SharedUtils.Exception;

namespace ServersUtils.Exception
{
    public class CantCreateServerException : NightFallException
    {
        public CantCreateServerException(string message) : base(message) { }
    }
}
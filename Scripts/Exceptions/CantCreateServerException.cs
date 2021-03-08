using SharedUtils.Exceptions;

namespace ServersUtils.Exceptions
{
    public class CantCreateServerException : NightFallException
    {
        public CantCreateServerException(string message) : base(message)
        {
        }
    }
}
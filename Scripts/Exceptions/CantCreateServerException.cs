using SharedUtils.Scripts.Exceptions;

namespace ServersUtils.Scripts.Exceptions
{
    public class CantCreateServerException : NightFallException
    {
        public CantCreateServerException(string message) : base(message)
        {
        }
    }
}
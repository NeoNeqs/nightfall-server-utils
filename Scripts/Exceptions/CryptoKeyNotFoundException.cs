using SharedUtils.Exceptions;

namespace ServersUtils.Exceptions
{
    public class CryptoKeyNotFoundException : NightFallException
    {
        public CryptoKeyNotFoundException(string message) : base(message)
        {
        }        
    }
}
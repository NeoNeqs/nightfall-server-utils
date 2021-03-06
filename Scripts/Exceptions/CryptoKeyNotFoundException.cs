using SharedUtils.Scripts.Exceptions;

namespace ServersUtils.Scripts.Exceptions
{
    public class CryptoKeyNotFoundException : NightFallException
    {
        public CryptoKeyNotFoundException(string message) : base(message)
        {
        }        
    }
}
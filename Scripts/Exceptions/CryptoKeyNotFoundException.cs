using SharedUtils.Exceptions;

namespace ServersUtils.Exceptions
{
    public class CryptoKeyNotFoundException : NightFallException
    {
        public CryptoKeyNotFoundException(string path) : base($"Failed to load crypto key from '{path}'")
        {
        }        
    }
}
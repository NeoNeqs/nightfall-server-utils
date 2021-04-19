using SharedUtils.Exception;

namespace ServersUtils.Exception
{
    public class CryptoKeyNotFoundException : NightFallException
    {
        public CryptoKeyNotFoundException(string path) : base($"Failed to load crypto key from '{path}'") { }
    }
}
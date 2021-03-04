using System;

namespace ServersUtils.Scripts.Exceptions
{
    public class CryptoKeyNotFoundException : Exception
    {
        public CryptoKeyNotFoundException(string path) : base($"Failed to load crypto key from '{path}'")
        {
        }        
    }
}
using Godot;

using ServersUtils.Scripts.Logging;

namespace ServersUtils.Scripts.Loaders
{
    public sealed class CryptoKeyLoader
    {
        public static CryptoKey Load(string from, string what, out Error outError)
        {
            var cryptoKey = new CryptoKey();
            var keyFile = from.PlusFile(what);
            var error = cryptoKey.Load(keyFile);
            outError = error;
            return cryptoKey;
        }

    }
}
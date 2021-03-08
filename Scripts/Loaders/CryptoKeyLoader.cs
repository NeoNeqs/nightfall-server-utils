using Godot;

using SharedUtils.Common;

namespace ServersUtils.Loaders
{
    public sealed class CryptoKeyLoader
    {
        public static CryptoKey Load(string from, string what, out ErrorCode outError)
        {
            var cryptoKey = new CryptoKey();
            var keyFile = from.PlusFile(what);
            var error = cryptoKey.Load(keyFile);
            outError = (ErrorCode)((int)error);
            return cryptoKey;
        }

    }
}
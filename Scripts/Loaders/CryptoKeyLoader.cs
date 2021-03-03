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
            if (error != Error.Ok)
            {
                ServerLogger.GetLogger.Error($"Could not load key file {ProjectSettings.GlobalizePath(keyFile)}. Error code: {error}");
            }
            outError = error;
            return cryptoKey;
        }

    }
}
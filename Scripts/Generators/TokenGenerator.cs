using System.Security.Cryptography;
using System.Text;

/// <summary>
///     source: https://gist.github.com/diegojancic/9f78750f05550fa6039d2f6092e461e5
/// </summary>

namespace ServersUtils.Generators
{
    public sealed class TokenGenerator
    {
        public static string GetUniqueKey(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890.-".ToCharArray();
            byte[] data = new byte[length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                _ = result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }
    }
}

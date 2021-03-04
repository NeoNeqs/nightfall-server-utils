using Godot;
using System;

namespace ServersUtils.Scripts.Exceptions
{
    public class CryptoKeyNotFoundException : Exception
    {
        public CryptoKeyNotFoundException(string message) : base(message)
        {
        }        
    }
}
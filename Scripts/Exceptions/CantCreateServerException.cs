using System;
namespace ServersUtils.Scripts.Exceptions
{
    public class CantCreateServerException : Exception
    {
        public CantCreateServerException(int port) : base($"Failed to create server on port '{port}'")
        {
        }
    }
}
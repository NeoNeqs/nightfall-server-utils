using System;

namespace ServersUtils.Scripts.Exceptions
{
    public class EnvironmentVariableNotSetException : Exception
    {
        public EnvironmentVariableNotSetException(string message) : base(message)
        {

        }        
    }
}
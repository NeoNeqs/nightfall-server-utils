using SharedUtils.Scripts.Exceptions;

namespace ServersUtils.Scripts.Exceptions
{
    public class EnvironmentVariableNotSetException : NightFallException
    {
        public EnvironmentVariableNotSetException(string message) : base(message)
        {

        }        
    }
}
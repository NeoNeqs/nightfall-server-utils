using SharedUtils.Exceptions;

namespace ServersUtils.Exceptions
{
    public class EnvironmentVariableNotSetException : NightFallException
    {
        public EnvironmentVariableNotSetException(string message) : base(message)
        {

        }        
    }
}
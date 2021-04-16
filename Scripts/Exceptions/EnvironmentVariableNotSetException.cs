using SharedUtils.Exception;

namespace ServersUtils.Exceptions
{
    public class EnvironmentVariableNotSetException : NightFallException
    {
        public EnvironmentVariableNotSetException(string environmentVariable) : base($"Environment variable {environmentVariable} is not set.")
        {

        }        
    }
}
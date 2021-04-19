using ServersUtils.Exceptions;
using SharedUtils.Common;
using SharedUtils.Validation;

namespace ServersUtils.Validation
{
    public static class ValidatorService
    {
        public static void ValidateEnvironmentVariables(IValidatable<string> validable, string[] environmentVariables)
        {
            foreach (string environmentVariable in environmentVariables)
            {
                ErrorCode isValidEror = validable.IsValid(environmentVariable);

                if (!isValidEror)
                {
                    throw new EnvironmentVariableNotSetException(environmentVariable);
                }
            }
        }
    }
}
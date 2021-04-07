using ServersUtils.Exceptions;

using SharedUtils.Common;
using SharedUtils.Services.Validators;

namespace ServersUtils.Services.Validators
{
    public static class ValidatorService
    {
        public static void ValidateEnvironmentVariables(IValidatable<string> validable, string[] environmentVariables)
        {
            foreach (var environmentVariable in environmentVariables)
            {
                var isValidEror = validable.IsValid(environmentVariable);

                if (!isValidEror)
                {
                    throw new EnvironmentVariableNotSetException($"Environment variable {environmentVariable} is not set.");
                }
            }
        }
    }
}
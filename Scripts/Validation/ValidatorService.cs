using ServersUtils.Exceptions;

using SharedUtils.Validation;

namespace ServersUtils.Validation
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
                    throw new EnvironmentVariableNotSetException(environmentVariable);
                }
            }
        }
    }
}
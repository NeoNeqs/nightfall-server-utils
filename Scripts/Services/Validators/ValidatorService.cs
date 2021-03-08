using Godot;

using ServersUtils.Exceptions;

using SharedUtils.Common;
using SharedUtils.Services.Validators;


namespace ServersUtils.Services.Validators
{
    public sealed class ValidatorService : Node
    {
        public override void _Ready()
        {
            var environmentVariables = new[] { "GATEWAY_TOKEN", "GAME_SERVER_TOKEN" };
            ValidateEnvironmentVariables(new EnvironmentVariableValidator(), environmentVariables);
        }

        private void ValidateEnvironmentVariables(IValidable<string> validable, string[] environmentVariables)
        {
            foreach (var environmentVariable in environmentVariables)
            {
                var isValidEror = validable.IsValid(environmentVariable);
                if (isValidEror != ErrorCode.Ok)
                    throw new EnvironmentVariableNotSetException($"Environment variable {environmentVariable} is not set.");
            }
        }
    }
}
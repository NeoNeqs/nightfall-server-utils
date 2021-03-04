using Godot;

using ServersUtils.Scripts.Exceptions;

using SharedUtils.Scripts.Common;
using SharedUtils.Scripts.Services.Validators;


namespace ServersUtils.Scripts.Services.Validators
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
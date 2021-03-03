using Godot;

using ServersUtils.Scripts.Logging;

using SharedUtils.Scripts.Services;
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
                if (isValidEror != Error.Ok)
#if DEBUG
                    GD.PushError($"Environment variable {environmentVariable} is not set. Abording...");
#endif
            }
        }
    }
}
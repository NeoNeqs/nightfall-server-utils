using Godot;
using NightFallServersUtils.Scripts.Logging;

namespace NightFallServersUtils.Scripts.Services.Validators
{
    public sealed class ValidatorService : Service
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
                {
                    Logger.Server.Error($"Environment variable {environmentVariable} is not set. Abording...");
                    Quit((int)isValidEror);
                }
            }
        }
    }
}
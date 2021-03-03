using Godot;

using SharedUtils.Scripts.Common;
using SharedUtils.Scripts.Services.Validators;

namespace ServersUtils.Scripts.Services.Validators
{
    public class EnvironmentVariableValidator : IValidable<string>
    {

        public ErrorCode IsValid(string toValidate)
        {
            if (OS.GetEnvironment(toValidate).Length == 0) return ErrorCode.EnvironmentVariableNotSet;
            return ErrorCode.Ok;
        }
    }
}
using Godot;

using SharedUtils.Common;
using SharedUtils.Services.Validators;

namespace ServersUtils.Services.Validators
{
    public class EnvironmentVariableValidator : IValidatable<string>
    {
        public ErrorCode IsValid(string toValidate)
        {
            if (OS.GetEnvironment(toValidate).Length == 0) return ErrorCode.EnvironmentVariableNotSet;
            return ErrorCode.Ok;
        }
    }
}
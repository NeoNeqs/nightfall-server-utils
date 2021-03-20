using Godot;

using SharedUtils.Common;
using SharedUtils.Services.Validators;

namespace ServersUtils.Services.Validators
{
    public class UserInputValidator : IValidatable<string>
    {
        public ErrorCode IsValid(string toValidate)
        {
            if (toValidate.Length > GlobalDefines.MaxInputLength) return ErrorCode.DataTooLong;
            string escaped = toValidate.CEscape();
            foreach (var c in escaped)
            {
                if (c >= '!' && c <= '~') continue;
                return ErrorCode.InvalidData;
            }
            return ErrorCode.Ok;
        }
    }
}
using Godot;

using SharedUtils.Common;
using SharedUtils.Validation;

using static SharedUtils.Factory.SharedSingletonFactory;

namespace ServersUtils.Validation
{
    public class UserInputValidator : IValidatable<string>
    {
        public ErrorCode IsValid(string toValidate)
        {
            if (toValidate.Length > SharedGlobalDefinesInstance.MaxInputLength) return ErrorCode.DataTooLong;
            string escaped = toValidate.CEscape();
            foreach (char c in escaped)
            {
                if (c >= '!' && c <= '~') continue;
                return ErrorCode.InvalidData;
            }
            return ErrorCode.Ok;
        }
    }
}
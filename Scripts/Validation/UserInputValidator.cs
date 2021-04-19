
using SharedUtils.Common;
using SharedUtils.Validation;


namespace ServersUtils.Validation
{
    public class UserInputValidator : IValidatable<string>
    {
        public ErrorCode IsValid(string toValidate)
        {
            if (toValidate.Length > GlobalDefines.MaxInputLength) return ErrorCode.DataTooLong;

            foreach (char c in toValidate)
            {
                if (c >= '!' && c <= '~') continue;
                return ErrorCode.InvalidData;
            }
            return ErrorCode.Ok;
        }
    }
}
using Godot;

namespace NightFallServersUtils.Scripts.Services.Validators
{
    public class EnvironmentVariableValidator : IValidable<string>
    {

        public Error IsValid(string toValidate)
        {
            if (OS.GetEnvironment(toValidate).Length == 0) return Error.PrinterOnFire;
            return Error.Ok;
        }
    }
}
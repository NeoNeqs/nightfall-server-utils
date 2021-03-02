
using Godot;

namespace NightFallServersUtils.Scripts.Services.Validators
{
    public interface IValidable<T>
    {
        Error IsValid(T toValidate);
    }
}
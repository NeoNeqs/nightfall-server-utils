using Godot;
using NightFallServersUtils.Scripts.Common;

namespace NightFallServersUtils.Scripts.Services.Validators
{
    public class PathValidator : IValidable<string>
    {
        public Error IsValid(string toValidate)
        {
            if (!(toValidate.IsAbsPath() || toValidate.IsRelPath())) return Error.FileBadPath;
            if (!DirectoryUtils.DirExists(toValidate)) return Error.DoesNotExist;
            return Error.Ok;
        }
    }
}
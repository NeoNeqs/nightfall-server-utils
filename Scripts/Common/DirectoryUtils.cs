using Godot;

namespace NightFallServersUtils.Scripts.Common
{
    public sealed class DirectoryUtils
    {
        public static bool DirExists(string path)
        {
            var dir = new Directory();
            return dir.DirExists(path);
        }

        public static void MakeDirRecursive(string path)
        {
            var dir = new Directory();
            var baseDir = path.GetBaseDir();
            if (!dir.DirExists(baseDir)) dir.MakeDirRecursive(baseDir);
        }

        public static void Rename(string from, string to)
        {
            var dir = new Directory();
            dir.Rename(from, to);
        }
    }
}
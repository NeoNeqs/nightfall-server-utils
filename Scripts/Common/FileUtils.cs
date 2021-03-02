using Godot;

namespace NightFallServersUtils.Scripts.Common
{
    public sealed class FileUtils
    {
        public static void CreateFileIfNotExists(string pathToFile)
        {
            var file = new File();
            if (!file.FileExists(pathToFile))
            {
                file.Open(pathToFile, File.ModeFlags.Write);
            }
            file.Close();
        }

        public static long GetLength(string pathToFile)
        {
            var file = new File();
            if (file.FileExists(pathToFile))
            {
                file.Open(pathToFile, File.ModeFlags.Read);
                var length = file.GetLen();
                file.Close();
                return length;
            }
            return -1L;
        }
    }
}
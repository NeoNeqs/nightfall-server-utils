using NightFallAuthenticationServer.Scripts.AutoLoad;
using Godot;

namespace NightFallServersUtils.Scripts.AutoLoad
{
    public sealed class ServerConfiguration : Node
    {
        public static Configuration Singleton => _singleton;
        private static Configuration _singleton;

        private const string Path = "user://config/config.ini";
        private readonly ConfigFile _configFile;
        private bool _isLoaded;


        private Configuration()
        {
            _singleton = this;
            _configFile = new ConfigFile();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            MakeDirRecursive();
            CreateConfigFileIfNotExist();
            var error = _configFile.Load(Path);
            if (error != Error.Ok)
            {
                _isLoaded = false;
                Logger.Error($"Could not load configuration file {ProjectSettings.GlobalizePath(Path)}. Error code: {error}");
                return;
            }
            _isLoaded = true;
        }

        private T GetValue<T>(string section, string key, T @default)
        {
            if (!_isLoaded) return @default;
            return (T)_configFile.GetValue(section, key, @default);
        }

        private void SetValue<T>(string section, string key, T value)
        {
            if (!_isLoaded) return;
            return _configFile.SetValue(section, key, value);
        }

        private void MakeDirRecursive()
        {
            var dir = new Directory();
            var baseDir = Path.GetBaseDir();
            if (!dir.DirExists(baseDir)) dir.MakeDirRecursive(baseDir);
        }

        private void CreateConfigFileIfNotExist()
        {
            var file = new File();
            if (!file.FileExists(Path))
            {
                file.Open(Path, File.ModeFlags.Write);
            }
            file.Close();
        }

        private void SaveConfiguration()
        {
            if (!_isLoaded) return;
            var error = _configFile.Save(Path);
            if (error != Error.Ok)
            {
                Logger.Error($"Could not save configuration file {ProjectSettings.GlobalizePath(Path)}. Error code: {error}");
            }
        }

        public override void _ExitTree()
        {
            SaveConfiguration();
        }
    }
}
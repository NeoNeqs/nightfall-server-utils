using Godot;
using NightFallServersUtils.Scripts.Common;
using NightFallServersUtils.Scripts.Logging;

namespace NightFallServersUtils.Scripts.Configurations
{
    public abstract class Configuration : Node
    {
        private readonly ConfigFile _configFile;
        private bool _isLoaded;


        protected Configuration()
        {
            _configFile = new ConfigFile();
        }

        protected void LoadConfiguration(string path)
        {
            DirectoryUtils.MakeDirRecursive(path);
            FileUtils.CreateFileIfNotExists(path);
            var error = _configFile.Load(path);
            if (error != Error.Ok)
            {
                _isLoaded = false;
                Logger.Error($"Could not load configuration file {ProjectSettings.GlobalizePath(path)}. Error code: {error}");
                return;
            }
            _isLoaded = true;
        }

        protected T GetValue<T>(string section, string key, T @default)
        {
            if (!_isLoaded) return @default;
            return (T)_configFile.GetValue(section, key, @default);
        }

        protected void SetValue<T>(string section, string key, T value)
        {
            if (!_isLoaded) return;
            _configFile.SetValue(section, key, value);
        }

        protected void SaveConfiguration(string path)
        {
            if (!_isLoaded) return;
            var error = _configFile.Save(path);
            if (error != Error.Ok)
            {
                Logger.Error($"Could not save configuration file {ProjectSettings.GlobalizePath(path)}. Error code: {error}");
            }
        }
    }
}
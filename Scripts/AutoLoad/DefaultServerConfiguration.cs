using Godot;

namespace NightFallServersUtils.Scripts.AutoLoad
{
    public abstract class DefaultServerConfiguration : Node
    {
        private const string Path = "user://config/config.ini";
        private readonly ConfigFile _configFile;
        private bool _isLoaded;


        protected DefaultServerConfiguration()
        {
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

        public int GetPort(int defaultPort)
        {
            return GetValue<int>("NETWORKING", "port", defaultPort);
        }

        public int GetMaxClients(int defaultMaxClients)
        {
            return GetValue<int>("NETWORKING", "max_clients", defaultMaxClients);
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
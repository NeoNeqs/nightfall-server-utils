using Godot;
using System;

using NightFallServersUtils.Scripts.Services.Validators;
using NightFallServersUtils.Scripts.Common;

namespace NightFallServersUtils.Scripts.Logging.Internal
{
    /// Wrapper around Godot.File class. Manages creation, rotation and deletion of files. Does cyclic writes of information to a disk.
    public sealed class LoggerFile
    {
        private const string FileName = "latest.log";
        private readonly File _fileHandle;
        private readonly string path;
        private int _writeCount;
        private const int MaxWriteCount = 100;


        public LoggerFile(string path)
        {
            _writeCount = 0;
            _fileHandle = new File();
            this.path = path;

            MakeDirs();
            RotateFiles();
        }

        /// Opens a logger file handle in write-only mode.
        public void Open()
        {
            _fileHandle.Open(path.PlusFile(FileName), File.ModeFlags.Write);
        }

        /// Flushes gathered output from LoggerFile.Write.
        public void Flush()
        {
            _fileHandle.Flush();
        }

        /// Writes output to a file handle. Flushed the buffer when LoggerFile._writeCount is exceeded.
        public void Write(string output)
        {
            _fileHandle.StoreLine(output);

            if (_writeCount++ >= MaxWriteCount)
            {
                Flush();
                _writeCount = 0;
            }
        }

        public void Close()
        {
            _fileHandle.Close();
        }

        /// Checks if LoggerFile.path is valid and creates directories recursively.
        /// Returns false if LoggerFile.path is invalid otherwise true.
        private void MakeDirs()
        {
            var isValid = new PathValidator().IsValid(path);
            if (isValid != Error.Ok) return;
            new Directory().MakeDirRecursive(path);
        }

        /// Makes sure that LoggerFile always writes to new empty file called LoggerFile.FileName.
        /// If said file is not empty, a rotation is performed.
        /// Rotation is a process of renaming main logger file to `log-YYYY-MM-DD-n.log` where n is a unique unsigned integer.
        private void RotateFiles()
        {
            var fullFilePath = path.PlusFile(FileName);
            if (!_fileHandle.FileExists(fullFilePath)) return;

            var length = FileUtils.GetLength(fullFilePath);

            if (length == 0) return;
            
            var date = GetCurrentLocalDateTimeFromUnixTime(_fileHandle.GetModifiedTime(fullFilePath));

            var newFileBaseName = GetLoggerFileFormat(date.Year, date.Month, date.Day);
            if (_fileHandle.FileExists(path.PlusFile(newFileBaseName + "-0.log")))
            {
                var index = GetHighestIndexOfLoggerFile(newFileBaseName);
                DirectoryUtils.Rename(path.PlusFile(FileName), path.PlusFile(newFileBaseName + "-" + (index + 1).ToString() + ".log"));
                return;
            }
            DirectoryUtils.Rename(path.PlusFile(FileName), path.PlusFile(newFileBaseName + "-0.log"));
        }

        private DateTime GetCurrentLocalDateTimeFromUnixTime(ulong unixTimestamp)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var newDate = date.AddSeconds(unixTimestamp);
            return newDate.ToLocalTime();
        }

        private string GetLoggerFileFormat(int year, int month, int day)
        {
            return $"log-{year}-{month.ToString().PadLeft(2, '0')}-{day.ToString().PadLeft(2, '0')}";
        }

        /// Finds the highest index `n` of logger file `log-YYYY-MM-DD-n.log` in directory LoggerFile.path
        private int GetHighestIndexOfLoggerFile(string loggerFileBaseName)
        {
            var dir = new Directory();

            dir.Open(path);
            dir.ListDirBegin(skipNavigational: true, skipHidden: true);
            var length = 0;
            var begin = 0;
            var max = 0;
            string fileName;
            while ((fileName = dir.GetNext()).Length != 0)
            {
                if (!fileName.Match(loggerFileBaseName + "-*.log")) continue;

                begin = loggerFileBaseName.Length + 1;
                length = fileName.Length - begin - 4;

                int index;
                if (!System.Int32.TryParse(fileName.Substr(begin, length), out index)) continue;
                if (index <= max) continue;
                max = index;
            }
            dir.ListDirEnd();

            return max;
        }
    }
}
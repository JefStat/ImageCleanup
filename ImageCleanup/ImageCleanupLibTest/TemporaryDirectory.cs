using System;
using System.IO;

namespace ImageCleanupLib
{
    public class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo _directoryInfo;

        public TemporaryDirectory()
        {
            _directoryInfo = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            _directoryInfo.Create();
        }

        public DirectoryInfo DirectoryInfo { get { return _directoryInfo; } }

        #region IDisposable Members
        public void Dispose()
        {
            try
            {
                _directoryInfo.Delete(true);
            }
            catch (IOException)
            {
                // ignore on purpose
            }
        }
        #endregion
    }
}

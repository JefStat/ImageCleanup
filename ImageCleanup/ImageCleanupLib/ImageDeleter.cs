namespace ImageCleanupLib
{
    using System;
    using System.IO;

    internal class ImageDeleter : IImageDeleter
    {
        #region Fields

        private readonly DateTime cutoffTime;

        private readonly DirectoryInfo rootDirectory;

        #endregion

        #region Constructors and Destructors

        public ImageDeleter(DateTime cutoffTime, DirectoryInfo rootDirectory)
        {
            this.cutoffTime = cutoffTime;
            this.rootDirectory = rootDirectory;
        }

        #endregion

        #region Public Methods and Operators

        public void Run()
        {
        }

        #endregion
    }
}
namespace ImageCleanupLib
{
    using System;

    public interface IImageDeleter
    {
        #region Public Methods and Operators

        void Run(DateTime cutoffTime, string rootDirectory);

        #endregion
    }
}
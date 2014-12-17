namespace ImageCleanupLib
{
    using System;

    public interface IImageDeleter
    {
        void Run(DateTime cutoffTime, string rootDirectory);
    }
}
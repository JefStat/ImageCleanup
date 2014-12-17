namespace ImageCleanup
{
    using System.ServiceProcess;

    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] servicesToRun = { new ImageCleanup() };
            ServiceBase.Run(servicesToRun);
        }

        #endregion
    }
}
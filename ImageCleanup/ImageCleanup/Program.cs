namespace ImageCleanup
{
    using System;
    using System.ServiceProcess;

    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase[] servicesToRun = { new ImageCleanup() };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                var imageCleanup = new ImageCleanup();
                imageCleanup.InternalStart(args);
                Console.WriteLine("ImageCleanup running.  Press 'q' or 'Q' to exit.");

                char key = ' ';
                while (key != 'q' && key != 'Q')
                {
                    key = Console.ReadKey().KeyChar;
                }

                imageCleanup.InternalStop();
            }
        }

        #endregion
    }
}
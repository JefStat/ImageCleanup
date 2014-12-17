namespace ImageCleanup
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.ServiceProcess;
    using System.Timers;

    using log4net;

    public partial class ImageCleanup : ServiceBase
    {
        #region Constants

        private const string ConfigKeyImageDirectory = "ROOT_IMAGE_DIRECTORY";

        private const string ConfigKeyPeriodHours = "PERIOD_HOURS";

        private const string ConfigKeyRetentionPeriodHours = "RETENTION_PERIOD_HOURS";

        #endregion

        #region Static Fields

        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageCleanup));

        #endregion

        #region Constructors and Destructors

        public ImageCleanup()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void OnStart(string[] args)
        {
            Log.Info("starting");
            var settings = ConfigurationManager.AppSettings;

            var rootDirectory = settings.Get(ConfigKeyImageDirectory);
            var rootDirectoryFileInfo = new FileInfo(rootDirectory);
            if (!rootDirectoryFileInfo.Exists)
            {
                Log.ErrorFormat(
                    "Could not parse value: {1}  for configuration key: {0}", 
                    ConfigKeyImageDirectory, 
                    rootDirectory);
                return;
            }

            var period = settings.Get(ConfigKeyPeriodHours);
            double periodDouble;
            if (double.TryParse(period, out periodDouble))
            {
                var interval = TimeSpan.FromHours(periodDouble);

                // var timer = new Timer { Interval = interval.TotalMilliseconds };
                var timer = new Timer { Interval = 60000 };
                timer.Elapsed += this.OnTimer;
                timer.Start();
            }
            else
            {
                Log.ErrorFormat("Could not parse value: {1}  for configuration key: {0}", ConfigKeyPeriodHours, period);
            }
        }

        protected override void OnStop()
        {
            Log.Info("stopping");
        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            var settings = ConfigurationManager.AppSettings;
            var period = settings.Get(ConfigKeyRetentionPeriodHours);
            double periodDouble;
            if (double.TryParse(period, out periodDouble))
            {
                var cutoffTime = DateTime.Now.AddHours(periodDouble);

                // TODO
                Log.InfoFormat("Beging deleting of images older than {0}", cutoffTime);
            }
            else
            {
                Log.ErrorFormat(
                    "Could not parse value: {1}  for configuration key: {0}", 
                    ConfigKeyRetentionPeriodHours, 
                    period);
            }
        }

        #endregion
    }
}
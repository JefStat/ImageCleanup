namespace ImageCleanup
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.ServiceProcess;
    using System.Timers;

    using ImageCleanupLib;

    using log4net;

    using Microsoft.Practices.Unity;

    public partial class ImageCleanup : ServiceBase
    {
        #region Constants

        private const string ConfigKeyImageDirectory = "ROOT_IMAGE_DIRECTORY";

        private const string ConfigKeyPeriodTimespan = "PERIOD_TIMESPAN";

        private const string ConfigKeyRetentionPeriodTimespan = "RETENTION_PERIOD_TIMESPAN";

        private const string InvalidValueForConfigurationKey = "Could not parse value: {1}  for configuration key: {0}";

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

        internal void InternalStart(string[] args)
        {
            this.OnStart(args);
        }

        internal void InternalStop()
        {
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("starting");

            GetRootDirectory();

            var interval = GetConfigurationParameter(
                ConfigKeyPeriodTimespan, 
                s =>
                    {
                        TimeSpan timeSpan;
                        TimeSpan.TryParse(s, out timeSpan);
                        return timeSpan;
                    });
            var timer = new Timer { Interval = interval.TotalMilliseconds };
            timer.Elapsed += this.OnTimer;
            timer.Start();
        }

        protected override void OnStop()
        {
            Log.Info("stopping");
        }

        private static T GetConfigurationParameter<T>(string keyName, Func<string, T> converter)
        {
            var stringValue = ConfigurationManager.AppSettings.Get(keyName);
            T value;
            try
            {
                value = converter(stringValue);
            }
            catch (Exception e)
            {
                Log.ErrorFormat(InvalidValueForConfigurationKey, keyName, stringValue);
                throw new ConfigurationErrorsException(keyName, e);
            }

            return value;
        }

        private static DirectoryInfo GetRootDirectory()
        {
            var rootDirectory = GetConfigurationParameter(
                ConfigKeyImageDirectory, 
                s =>
                    {
                        var result = new DirectoryInfo(s);

                        if (!result.Exists)
                        {
                            throw new ConfigurationErrorsException(ConfigKeyImageDirectory);
                        }

                        return result;
                    });
            return rootDirectory;
        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            var retentionTimeSpan = GetConfigurationParameter(
                ConfigKeyRetentionPeriodTimespan, 
                s =>
                    {
                        TimeSpan timeSpan;
                        TimeSpan.TryParse(s, out timeSpan);
                        return timeSpan;
                    });

            var cutoffTime = DateTime.Now.Add(retentionTimeSpan);
            var rootDirectory = GetRootDirectory();

            var container = ContainerManager.GetContainer();
            var deleter = container.Resolve<IImageDeleter>();

            Log.InfoFormat("Beggining deletion of images older than {0} from root {1}", cutoffTime, rootDirectory);

            deleter.Run(cutoffTime, rootDirectory.FullName);
        }

        #endregion
    }
}
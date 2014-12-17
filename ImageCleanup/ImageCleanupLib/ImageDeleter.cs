namespace ImageCleanupLib
{
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;

    using log4net;

    public class ImageDeleter : IImageDeleter
    {
        #region Static Fields

        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageDeleter));

        #endregion

        #region Fields

        private IFileSystem fileSystem;

        #endregion

        #region Constructors and Destructors

        public ImageDeleter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ImageDeleter()
            : this(new FileSystem())
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Run(DateTime cutoffTime, string rootDirectory)
        {
            this.DeleteFromImagesDirectoryTree(
                cutoffTime, 
                this.fileSystem.DirectoryInfo.FromDirectoryName(rootDirectory));

            // this.rootDirectory.GetFiles("*.*", SearchOption.AllDirectories)
            // .Where(f => f.CreationTime < this.cutoffTime)
            // .ForEach(
            // f =>
            // {
            // Log.InfoFormat("Deleting, {0}", f.FullName);
            // f.Delete();
            // });
        }

        #endregion

        #region Methods

        private static bool GetDay(string value, out int day)
        {
            if (!int.TryParse(value, out day) || 31 <= day || day < 1)
            {
                Log.DebugFormat("Directory name {0} is not a day", value);
                return true;
            }

            return false;
        }

        private static bool GetHour(string value, out int hour)
        {
            if (!int.TryParse(value, out hour) || 24 <= hour || hour < 1)
            {
                Log.DebugFormat("Directory name {0} is not an hour", value);
                return true;
            }

            return false;
        }

        private static bool GetMonth(string value, out int month)
        {
            if (!int.TryParse(value, out month) || 12 <= month || month < 1)
            {
                Log.DebugFormat("Directory name {0} is not a month", value);
                return true;
            }

            return false;
        }

        private static bool GetYear(string value, out int year)
        {
            if (!int.TryParse(value, out year) || year <= 0)
            {
                Log.DebugFormat("Directory name {0} is not a year", year);
                return true;
            }

            return false;
        }

        private void DeleteFromImagesDirectoryTree(DateTime cutoffTime, DirectoryInfoBase rootDirectory)
        {
            var folderWithoutSubfolder =
                rootDirectory.GetDirectories("*.*", SearchOption.AllDirectories).Where(d => !d.GetDirectories().Any());
            foreach (var directoryInfo in folderWithoutSubfolder)
            {
                var dayInfo = directoryInfo.Parent;
                if (dayInfo == null)
                {
                    Log.Debug("Missing day parent directory");
                    continue;
                }

                var monthInfo = dayInfo.Parent;
                if (monthInfo == null)
                {
                    Log.Debug("Missing month parent directory");
                    continue;
                }

                var yearInfo = monthInfo.Parent;
                if (yearInfo == null)
                {
                    Log.Debug("Missing year parent directory");
                    continue;
                }

                int year;
                if (GetYear(yearInfo.Name, out year))
                {
                    continue;
                }

                int month;
                if (GetMonth(monthInfo.Name, out month))
                {
                    continue;
                }

                int day;
                if (GetDay(dayInfo.Name, out day))
                {
                    continue;
                }

                int hour;
                if (GetHour(directoryInfo.Name, out hour))
                {
                    continue;
                }

                var date = new DateTime(year, month, day, hour, 0, 0);
                if (date < cutoffTime)
                {
                    directoryInfo.Delete(true);
                }
            }
        }

        #endregion
    }
}
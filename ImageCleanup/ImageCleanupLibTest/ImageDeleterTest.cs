namespace ImageCleanupLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ImageDeleterTest
    {
        #region Constants

        private const string RootDirectoryName = "/";

        #endregion

        #region Fields

        private Mock<DirectoryInfoBase> dayDirectory;

        private Mock<DirectoryInfoBase> hourDirectory;

        private Mock<IDirectoryInfoFactory> mockDirectoryFactory;

        private Mock<DirectoryInfoBase> mockRootDirectory;

        private Mock<DirectoryInfoBase> monthDirectory;

        private ImageDeleter target;

        private Mock<DirectoryInfoBase> yearDirectory;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void InvalidDateTest()
        {
            this.SetupYMDH("2013", "12", "31", "24");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.SetupYMDH("2013", "11", "31", "23");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.SetupYMDH("2013", "12", "31", "-1");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.SetupYMDH("2013", "12", "0", "0");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.SetupYMDH("2013", "0", "31", "0");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.SetupYMDH("0", "12", "31", "0");
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);

            this.hourDirectory.Verify(m => m.Delete(It.IsAny<bool>()), Times.Never);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var filesystem = new Mock<IFileSystem>(MockBehavior.Strict);
            this.mockDirectoryFactory = new Mock<IDirectoryInfoFactory>(MockBehavior.Strict);
            filesystem.Setup(m => m.DirectoryInfo).Returns(this.mockDirectoryFactory.Object);

            this.mockRootDirectory = new Mock<DirectoryInfoBase>(MockBehavior.Strict);
            this.mockDirectoryFactory.Setup(m => m.FromDirectoryName(RootDirectoryName))
                .Returns(() => this.mockRootDirectory.Object);

            this.yearDirectory = new Mock<DirectoryInfoBase>(MockBehavior.Strict);
            this.yearDirectory.Setup(m => m.Parent).Returns((DirectoryInfoBase)null);
            this.yearDirectory.Setup(m => m.Name).Returns("2013");
            this.monthDirectory = new Mock<DirectoryInfoBase>(MockBehavior.Strict);
            this.monthDirectory.Setup(m => m.Parent).Returns(this.yearDirectory.Object);
            this.monthDirectory.Setup(m => m.Name).Returns("12");
            this.dayDirectory = new Mock<DirectoryInfoBase>(MockBehavior.Strict);
            this.dayDirectory.Setup(m => m.Parent).Returns(this.monthDirectory.Object);
            this.dayDirectory.Setup(m => m.Name).Returns("31");
            this.hourDirectory = new Mock<DirectoryInfoBase>(MockBehavior.Strict);
            this.hourDirectory.Setup(m => m.Parent).Returns(this.dayDirectory.Object);
            this.hourDirectory.Setup(m => m.Name).Returns("24");
            this.hourDirectory.Setup(m => m.GetDirectories()).Returns(new DirectoryInfoBase[0]);

            var directoryList = new List<Mock<DirectoryInfoBase>> { this.hourDirectory };

            this.mockRootDirectory.Setup(m => m.GetDirectories(It.IsAny<string>(), SearchOption.AllDirectories))
                .Returns(directoryList.Select(i => i.Object).ToArray());

            this.target = new ImageDeleter(filesystem.Object);
        }

        [TestMethod]
        public void ValidDeleteTest()
        {
            this.SetupYMDH("2013", "12", "31", "23");
            this.hourDirectory.Setup(m => m.Delete(true)).Verifiable();
            this.target.Run(new DateTime(2014, 12, 31, 12, 0, 0), RootDirectoryName);
            this.hourDirectory.Verify();
        }

        #endregion

        #region Methods

        private void SetupYMDH(string s, string s1, string s2, string s3)
        {
            this.yearDirectory.Setup(m => m.Name).Returns(s);
            this.monthDirectory.Setup(m => m.Name).Returns(s1);
            this.dayDirectory.Setup(m => m.Name).Returns(s2);
            this.hourDirectory.Setup(m => m.Name).Returns(s3);
        }

        #endregion
    }
}
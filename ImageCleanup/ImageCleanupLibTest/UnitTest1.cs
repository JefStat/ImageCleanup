using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageCleanupLib
{
    using System.IO;
    using System.IO.Abstractions;

    using Moq;

    [TestClass]
    public class UnitTest1
    {
        private ImageDeleter target;

        [TestInitialize]
        public void TestInitialize()
        {
            var factory = new Mock<IFileSystem>(MockBehavior.Strict);
            this.target = new ImageDeleter(factory.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
            target.Run(DateTime.Now,"/");
        }
    }
}

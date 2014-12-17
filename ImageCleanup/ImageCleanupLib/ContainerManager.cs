namespace ImageCleanupLib
{
    using System;

    using Microsoft.Practices.Unity;

    public static class ContainerManager
    {
        #region Static Fields

        private static readonly Lazy<IUnityContainer> Instance = new Lazy<IUnityContainer>(CreateContainer);

        #endregion

        #region Public Methods and Operators

        public static IUnityContainer GetContainer()
        {
            return Instance.Value;
        }

        #endregion

        #region Methods

        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IImageDeleter, ImageDeleter>();

            return container;
        }

        #endregion
    }
}
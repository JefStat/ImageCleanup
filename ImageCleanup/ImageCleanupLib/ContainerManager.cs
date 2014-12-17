using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCleanupLib
{
    using Microsoft.Practices.Unity;

    public class ContainerManager
    {
        private static readonly Lazy<IUnityContainer> _instance = new Lazy<IUnityContainer>(CreateContainer);

        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IImageDeleter, ImageDeleter>();

            return container;
        }

        public static IUnityContainer GetContainer()
        {
            return _instance.Value;
        }
    }
}

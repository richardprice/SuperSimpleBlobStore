using System;
using log4net.Config;
using SuperSimpleBlobStore.Common.Logging;
using Topshelf;

namespace SuperSimpleBlobStore.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += Logging.CurrentDomain_UnhandledException;
            XmlConfigurator.Configure();

            RunService();
        }

        private static void RunService()
        {
            HostFactory.Run(x =>
            {
                x.Service<BlobStoreService>(s =>
                {
                    s.ConstructUsing(service => new BlobStoreService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });

                x.RunAsNetworkService();

                x.SetDescription("Blob Store");
                x.SetDisplayName("BlobStore");
                x.SetServiceName("BlobStore");
            });
        }
    }
}

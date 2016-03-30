using System;
using log4net;
using Microsoft.Owin.Hosting;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.Api
{
    public class BlobStoreService
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        protected IDisposable WebAppHolder { get; set; }

        public void Start()
        {
            if (WebAppHolder == null)
            {
                _logger.Info("Starting Service");
                WebAppHolder = WebApp.Start<Startup>("http://+:" + ConfigurationProvider.ServicePort);
                _logger.Info("Service Started");
            }
        }

        public void Stop()
        {
            if (WebAppHolder != null)
            {
                _logger.Info("Stopping Service");
                WebAppHolder.Dispose();
                WebAppHolder = null;
                _logger.Info("Service Stopped");
            }
        }
    }
}

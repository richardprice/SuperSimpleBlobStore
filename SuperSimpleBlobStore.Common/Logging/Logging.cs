using System;
using Mindscape.Raygun4Net;

namespace SuperSimpleBlobStore.Common.Logging
{
    public static class Logging
    {
        public static RaygunClient Raygun = new RaygunClient("xxx");

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Raygun.Send(e.ExceptionObject as Exception);
        }
    }
}

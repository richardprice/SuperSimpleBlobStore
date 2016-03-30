using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace SuperSimpleBlobStore.DataAccess.Tests
{
    public class TestBase
    {
        #region "Infrastructure"
        public static string DbName;
        public static string LocalPath;
        #endregion "Infrastructure"
    }
}

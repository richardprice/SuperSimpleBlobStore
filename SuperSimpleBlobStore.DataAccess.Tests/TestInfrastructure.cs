using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.DataAccess.Tests
{
    [SetUpFixture]
    public class TestInfrastructure
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            // Only do the set up if something else hasnt yet done it
            if (string.IsNullOrWhiteSpace(TestBase.DbName))
            {
                TestBase.DbName = Guid.NewGuid().ToString("N");

                var connection =
                    new SqlConnection(ConfigurationProvider.BlobStoreDatabaseConnectionString);
                var command = new SqlCommand("create database [unit_" + TestBase.DbName + "];", connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not set up unit test database", ex);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }

                connection =
                    new SqlConnection("Database=unit_" + TestBase.DbName + ";" +
                                      ConfigurationProvider.BlobStoreDatabaseConnectionString);

                TestBase.LocalPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

                if (TestBase.LocalPath == null)
                    return;

                var location = new Uri(TestBase.LocalPath);

                var sql = File.ReadAllText(Path.Combine(location.LocalPath, "database.sql"));

                command = new SqlCommand(sql, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not set up unit test database", ex);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (String.IsNullOrWhiteSpace(TestBase.DbName))
                return;

            var connection = new SqlConnection("Database=master;" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var command = new SqlCommand("ALTER DATABASE [unit_" + TestBase.DbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [unit_" + TestBase.DbName + "];", connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not set up unit test database", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}

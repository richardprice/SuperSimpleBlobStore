using System;
using System.Data.SqlClient;
using log4net.Appender;
using log4net.Core;

namespace SuperSimpleBlobStore.Common.Logging
{
    public class SqlAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationProvider.LoggingConnectionString))
            {
                const string sqlStatement = @"INSERT INTO 
                                        LogEntry (TimeStamp, 
                                                    ApplicationName,
                                                    Environment,
                                                    LoggerName,
                                                    LoggerIdentity,
                                                    LoggerDomain,
                                                    Level,
                                                    Message,
                                                    Exception,
                                                    ServerName) 
                                        VALUES (@TimeStamp, 
                                                @ApplicationName,
                                                @Environment,
                                                @LoggerName,
                                                @LoggerIdentity,
                                                @LoggerDomain,
                                                @Level,
                                                @Message,
                                                @Exception,
                                                @ServerName)";

                using (var sqlCommand = new SqlCommand(sqlStatement))
                {
                    sqlCommand.Parameters.AddWithValue("@TimeStamp", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@ApplicationName", ConfigurationProvider.ApplicationName);
                    sqlCommand.Parameters.AddWithValue("@Environment", ConfigurationProvider.EnvironmentType.ToString());
                    sqlCommand.Parameters.AddWithValue("@LoggerName", loggingEvent.LoggerName);
                    sqlCommand.Parameters.AddWithValue("@LoggerIdentity", loggingEvent.Identity);
                    sqlCommand.Parameters.AddWithValue("@LoggerDomain", loggingEvent.Domain);
                    sqlCommand.Parameters.AddWithValue("@Level", loggingEvent.Level.ToString());
                    sqlCommand.Parameters.AddWithValue("@Message", loggingEvent.RenderedMessage);
                    sqlCommand.Parameters.AddWithValue("@Exception", loggingEvent.ExceptionObject != null ? loggingEvent.ExceptionObject.ToString() : String.Empty);
                    sqlCommand.Parameters.AddWithValue("@ServerName", Environment.MachineName);

                    sqlCommand.Connection = sqlConnection;

                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly string _connectionString;

        public ContainerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Container GetValidContainer(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[Id] = @Id
                                AND 
                                    [dbo].[Containers].[ValidFrom] < @TodaysDate
                                AND
	                                (
                                        [dbo].[Containers].[ValidTo] is null 
                                    OR 
                                        [dbo].[Containers].[ValidTo] > @TodaysDate
                                    )";

                try
                {
                    return _connection.Query<Container>(sql, new {Id = id, TodaysDate = DateTime.Now}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetContainer(int id): could not get container", ex);
                }
            }
        }

        public Container GetValidContainer(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[ContainerIdentity] = @Id
                                AND 
                                    [dbo].[Containers].[ValidFrom] < @TodaysDate
                                AND
	                                (
                                        [dbo].[Containers].[ValidTo] is null 
                                    OR 
                                        [dbo].[Containers].[ValidTo] > @TodaysDate
                                    )";

                try
                {
                    return _connection.Query<Container>(sql, new {Id = id, TodaysDate = DateTime.Now}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetContainer(int id): could not get container", ex);
                }
            }
        }

        public Container GetContainer(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[Id] = @Id";

                try
                {
                    return _connection.Query<Container>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetContainer(int id): could not get container", ex);
                }
            }
        }

        public Container GetContainer(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[Id] = @Id";

                try
                {
                    return _connection.Query<Container>(sql, new {ContainerIdentity = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetContainer(Guid id): could not get container", ex);
                }
            }
        }

        public IEnumerable<Container> GetContainersForOwner(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[CreatedBy] = @Id";

                try
                {
                    return _connection.Query<Container>(sql, new {Id = id}).AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetContainersForOwner(Guid id): could not get containers", ex);
                }
            }
        }

        public Container CreateContainer(Container container)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO [dbo].[Containers]
                                       ([ContainerIdentity]
                                       ,[Name]
                                       ,[CreatedBy]
                                       ,[CreatedOn]
                                       ,[ValidFrom]
                                       ,[ValidTo]
                                       ,[Etag]
                                       ,[LastModified])
                                 VALUES
                                       (
                                        @ContainerIdentity
                                       ,@Name
                                       ,@CreatedBy
                                       ,@CreatedOn
                                       ,@ValidFrom
                                       ,@ValidTo
                                       ,@Etag
                                       ,@LastModified);
                                SELECT
                                    [Id]
                                   ,[ContainerIdentity]
                                   ,[Name]
                                   ,[CreatedBy]
                                   ,[CreatedOn]
                                   ,[ValidFrom]
                                   ,[ValidTo]
                                   ,[Etag]
                                   ,[LastModified]
                                FROM 
                                    [dbo].[Containers]
                                WHERE
                                    [dbo].[Containers].[Id] = CAST(SCOPE_IDENTITY() as int)";

                try
                {
                    var result = _connection.Query<Container>(sql, container).Single();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateContainer: could not create container", ex);
                }
            }
        }

        public bool UpdateContainer(Container container)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                container.LastModified = DateTime.Now;

                const string sql = @"
                UPDATE [dbo].[Containers]
                    SET [ContainerIdentity] = @ContainerIdentity
                  ,[Name] = @Name
                  ,[CreatedBy] = @CreatedBy
                  ,[CreatedOn] = @CreatedOn
                  ,[ValidFrom] = @ValidFrom
                  ,[ValidTo] = @ValidTo
                  ,[Etag] = @Etag
                  ,[LastModified] = @LastModified
                WHERE [dbo].[Containers].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, container);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateContainer: could not update container", ex);
                }
            }
        }

        public bool DeleteContainer(Container container)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"
                UPDATE [dbo].[Containers]
                    SET [ValidTo] = @ValidTo
                WHERE [dbo].[Containers].[Id] = @Id";
                try
                {
                    var result = _connection.Execute(sql, new {container.Id, ValidTo = DateTime.Now});

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("DeleteContainer: could not delete container", ex);
                }
            }
        }
    }
}

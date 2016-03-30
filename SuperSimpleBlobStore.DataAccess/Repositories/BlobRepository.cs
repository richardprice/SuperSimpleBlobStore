using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public class BlobRepository : IBlobRepository
    {
        private readonly string _connectionString;

        public BlobRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region "Aggregate Blob"

        public Blob GetLatestValidBlob(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"select [Blobs].[BlobId]
                                        ,[Blobs].[CreatedBy] as BlobCreatedBy
                                        ,[Blobs].[CreatedOn] as BlobCreatedOn
                                        ,[Blobs].[ValidFrom] as BlobValidFrom
                                        ,[Blobs].[ValidTo] as BlobValidTo
                                        ,[Blobs].[LastModified] as BlobLastModified
                                        ,[Blobs].[LastModifiedBy] as BlobLastModifiedBy
	                                    ,[Containers].[ContainerIdentity]
                                        ,[BlobVersions].[VersionId]
                                        ,[BlobVersions].[CreatedOn] as VersionCreatedOn
                                        ,[BlobVersions].[CreatedBy] as VersionCreatedBy
                                        ,[BlobVersions].[ValidFrom] as VersionValidFrom
                                        ,[BlobVersions].[ValidTo] as VersionValidTo
                                        ,[BlobVersions].[LastModified] as VersionLastModified
                                        ,[BlobVersions].[LastModifiedBy] as VersionLastModifiedBy
                                        ,[BlobVersions].[BlobContentsId]
                                        ,[BlobVersions].[Etag]
                                        ,[BlobVersions].[ContentLength]
                                        ,[BlobVersions].[ContentType]
                                        ,[BlobVersions].[FileName]
                                        ,[BlobVersions].[BlobVariantHash]
                                    FROM 
                                        [dbo].[Blobs] left join [dbo].[Containers] on [Blobs].[ContainerId] = [Containers].[Id]
                                                      left join [dbo].[BlobVersions] on [BlobVersions].[BlobId] = [Blobs].[Id]
                                    where [Blobs].[BlobId] = @Id
                                    and [Blobs].[ValidTo] is null
                                    and [BlobVersions].[ValidTo] is null
                                    order by [BlobVersions].[ValidFrom] desc";

                try
                {
                    return _connection.Query<Blob>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetLatestValidBlob(Guid id): could not get blob", ex);
                }
            }
        }

        public byte[] GetLatestValidBlobContents(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"select [BlobContents].[Content]
                                  FROM [dbo].[Blobs] left join [dbo].[BlobVersions] on [Blobs].[Id] = [BlobVersions].[BlobId]
                                  left join [dbo].[BlobContents] on [BlobVersions].[BlobContentsId] = [BlobContents].[Id]
                                  where [Blobs].[BlobId] = @Id
                                  and [Blobs].[ValidTo] is null
                                  and [BlobVersions].[ValidTo] is null
                                  order by [BlobVersions].[ValidFrom] desc";

                try
                {
                    return _connection.Query<byte[]>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetLatestValidBlobContents(Guid id): could not get blob", ex);
                }
            }
        }

        public Blob GetSpecificBlobVersion(Guid id, Guid versionId)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"select [Blobs].[BlobId]
                                        ,[Blobs].[CreatedBy] as BlobCreatedBy
                                        ,[Blobs].[CreatedOn] as BlobCreatedOn
                                        ,[Blobs].[ValidFrom] as BlobValidFrom
                                        ,[Blobs].[ValidTo] as BlobValidTo
                                        ,[Blobs].[LastModified] as BlobLastModified
                                        ,[Blobs].[LastModifiedBy] as BlobLastModifiedBy
	                                    ,[Containers].[ContainerIdentity]
                                        ,[BlobVersions].[VersionId]
                                        ,[BlobVersions].[CreatedOn] as VersionCreatedOn
                                        ,[BlobVersions].[CreatedBy] as VersionCreatedBy
                                        ,[BlobVersions].[ValidFrom] as VersionValidFrom
                                        ,[BlobVersions].[ValidTo] as VersionValidTo
                                        ,[BlobVersions].[LastModified] as VersionLastModified
                                        ,[BlobVersions].[LastModifiedBy] as VersionLastModifiedBy
                                        ,[BlobVersions].[BlobContentsId]
                                        ,[BlobVersions].[Etag]
                                        ,[BlobVersions].[ContentLength]
                                        ,[BlobVersions].[ContentType]
                                        ,[BlobVersions].[FileName]
                                        ,[BlobVersions].[BlobVariantHash]
                                    FROM [dbo].[Blobs] left join [dbo].[Containers] on [Blobs].[ContainerId] = [Containers].[Id], [dbo].[BlobVersions]
                                    where [Blobs].[BlobId] = @Id
                                    and [BlobVersions].[VersionId] = @VersionId";

                try
                {
                    return _connection.Query<Blob>(sql, new {Id = id, VersionId = versionId}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetSpecificBlobVersion(Guid id, Guid versionId): could not get blob", ex);
                }
            }
        }

        public Blob GetBlobForDate(Guid id, DateTime date)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"select [Blobs].[BlobId]
                                        ,[Blobs].[CreatedBy] as BlobCreatedBy
                                        ,[Blobs].[CreatedOn] as BlobCreatedOn
                                        ,[Blobs].[ValidFrom] as BlobValidFrom
                                        ,[Blobs].[ValidTo] as BlobValidTo
                                        ,[Blobs].[LastModified] as BlobLastModified
                                        ,[Blobs].[LastModifiedBy] as BlobLastModifiedBy
	                                    ,[Containers].[ContainerIdentity]
                                        ,[BlobVersions].[VersionId]
                                        ,[BlobVersions].[CreatedOn] as VersionCreatedOn
                                        ,[BlobVersions].[CreatedBy] as VersionCreatedBy
                                        ,[BlobVersions].[ValidFrom] as VersionValidFrom
                                        ,[BlobVersions].[ValidTo] as VersionValidTo
                                        ,[BlobVersions].[LastModified] as VersionLastModified
                                        ,[BlobVersions].[LastModifiedBy] as VersionLastModifiedBy
                                        ,[BlobVersions].[BlobContentsId]
                                        ,[BlobVersions].[Etag]
                                        ,[BlobVersions].[ContentLength]
                                        ,[BlobVersions].[ContentType]
                                        ,[BlobVersions].[FileName]
                                        ,[BlobVersions].[BlobVariantHash]
                                    FROM 
                                        [dbo].[Blobs] left join [dbo].[Containers] on [Blobs].[ContainerId] = [Containers].[Id]
                                                      left join [dbo].[BlobVersions] on [BlobVersions].[BlobId] = [Blobs].[Id]
                                    where [Blobs].[BlobId] = @Id
                                    and [Blobs].[ValidTo] is null
                                    and [BlobVersions].[ValidFrom] < @Period
                                    and ([BlobVersions].[ValidTo] is null or [BlobVersions].[ValidTo] > @Period)
                                    order by [BlobVersions].[ValidFrom] desc";

                try
                {
                    return _connection.Query<Blob>(sql, new {Id = id, Period = date}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetLatestValidBlob(Guid id): could not get blob", ex);
                }
            }
        }
        #endregion "Aggregate Blob"

        #region "Blob Details"
        public BlobDetails GetBlobDetails(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                    ,[BlobId]
                                    ,[CreatedBy]
                                    ,[CreatedOn]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                FROM [dbo].[Blobs]
                                WHERE
                                    [dbo].[Blobs].[Id] = @Id";

                try
                {
                    return _connection.Query<BlobDetails>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobDetails(int id): could not get blob details", ex);
                }
            }
        }

        public BlobDetails GetBlobDetails(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                    ,[BlobId]
                                    ,[CreatedBy]
                                    ,[CreatedOn]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                FROM [dbo].[Blobs]
                                WHERE
                                    [dbo].[Blobs].[BlobId] = @Id";

                try
                {
                    return _connection.Query<BlobDetails>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobDetails(Guid id): could not get blob details", ex);
                }
            }
        }

        public IEnumerable<BlobDetails> GetBlobsForOwner(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                    ,[BlobId]
                                    ,[CreatedBy]
                                    ,[CreatedOn]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                FROM [dbo].[Blobs]
                                WHERE
                                    [dbo].[Blobs].[CreatedBy] = @Id";

                try
                {
                    return _connection.Query<BlobDetails>(sql, new {Id = id}).AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobForOwner(Guid id): could not get blob details", ex);
                }
            }
        }

        public IEnumerable<BlobDetails> GetBlobsForContainer(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT
                                    [Id]
                                    ,[BlobId]
                                    ,[CreatedBy]
                                    ,[CreatedOn]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                FROM [dbo].[Blobs]
                                WHERE
                                    [dbo].[Blobs].[ContainerId] = @Id";

                try
                {
                    return _connection.Query<BlobDetails>(sql, new {Id = id}).AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobForContainer(Guid id): could not get blob details", ex);
                }
            }
        }

        public BlobDetails CreateBlobDetails(BlobDetails blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO [dbo].[Blobs]
                                       ([BlobId]
                                       ,[CreatedBy]
                                       ,[CreatedOn]
                                       ,[ValidFrom]
                                       ,[ValidTo]
                                       ,[ContainerId]
                                       ,[LastModified]
                                       ,[LastModifiedBy])
                                 VALUES
                                       (@BlobId
                                       ,@CreatedBy
                                       ,@CreatedOn
                                       ,@ValidFrom
                                       ,@ValidTo
                                       ,@ContainerId
                                       ,@LastModified
                                       ,@LastModifiedBy);
                                SELECT
                                    [Id]
                                    ,[BlobId]
                                    ,[CreatedBy]
                                    ,[CreatedOn]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                FROM [dbo].[Blobs]
                                WHERE
                                    [dbo].[Blobs].[Id] = CAST(SCOPE_IDENTITY() as int)
                                ";

                try
                {
                    var result = _connection.Query<BlobDetails>(sql, blob).Single();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateBlobDetails: could not create blob", ex);
                }
            }
        }

        public bool UpdateBlobDetails(BlobDetails blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[Blobs]
                                   SET [BlobId] = @BlobId
                                      ,[CreatedBy] = @CreatedBy
                                      ,[CreatedOn] = @CreatedOn
                                      ,[ValidFrom] = @ValidFrom
                                      ,[ValidTo] = @ValidTo
                                      ,[ContainerId] = @ContainerId
                                      ,[LastModified] = @LastModified
                                      ,[LastModifiedBy] = @LastModifiedBy
                                WHERE [dbo].[Blobs].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, blob);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateBlobDetails: could not update blob", ex);
                }
            }
        }

        public bool DeleteBlobDetails(BlobDetails blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[Blobs]
                                   SET [ValidTo] = @ValidTo
                                WHERE [dbo].[Blobs].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, new {blob.Id, ValidTo = DateTime.Now});

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("DeleteBlobDetails: could not delete blob", ex);
                }
            }
        }
        #endregion "Blob Details"

        #region "Blob Versions"
        public BlobVersion GetBlobVersion(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[VersionId]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                    ,[BlobContentsId]
                                    ,[Etag]
                                    ,[ContentLength]
                                    ,[ContentType]
                                    ,[FileName]
                                    ,[BlobId]
                                    ,[BlobVariantHash]
                                FROM [dbo].[BlobVersions]
                                WHERE
                                    [dbo].[BlobVersions].[Id] = @Id";

                try
                {
                    return _connection.Query<BlobVersion>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobVersion(int id): could not get blob details", ex);
                }
            }
        }

        public BlobVersion GetBlobVersion(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[VersionId]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                    ,[BlobContentsId]
                                    ,[Etag]
                                    ,[ContentLength]
                                    ,[ContentType]
                                    ,[FileName]
                                    ,[BlobId]
                                    ,[BlobVariantHash]
                                FROM [dbo].[BlobVersions]
                                WHERE
                                    [dbo].[BlobVersions].[VersionId] = @Id";

                try
                {
                    return _connection.Query<BlobVersion>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobVersion(Guid id): could not get blob details", ex);
                }
            }
        }

        public BlobVersion CreateBlobVersion(BlobVersion blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO [dbo].[BlobVersions]
                                       ([VersionId]
                                       ,[CreatedOn]
                                       ,[CreatedBy]
                                       ,[ValidFrom]
                                       ,[ValidTo]
                                       ,[LastModified]
                                       ,[LastModifiedBy]
                                       ,[BlobContentsId]
                                       ,[Etag]
                                       ,[ContentLength]
                                       ,[ContentType]
                                       ,[FileName]
                                       ,[BlobId]
                                       ,[BlobVariantHash])
                                 VALUES
                                       (@VersionId
                                       ,@CreatedOn
                                       ,@CreatedBy
                                       ,@ValidFrom
                                       ,@ValidTo
                                       ,@LastModified
                                       ,@LastModifiedBy
                                       ,@BlobContentsId
                                       ,@Etag
                                       ,@ContentLength
                                       ,@ContentType
                                       ,@FileName
                                       ,@BlobId
                                       ,@BlobVariantHash);
                                SELECT [Id]
                                    ,[VersionId]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[LastModified]
                                    ,[LastModifiedBy]
                                    ,[BlobContentsId]
                                    ,[Etag]
                                    ,[ContentLength]
                                    ,[ContentType]
                                    ,[FileName]
                                    ,[BlobId]
                                    ,[BlobVariantHash]
                                FROM [dbo].[BlobVersions]
                                WHERE
                                    [dbo].[BlobVersions].[Id] = CAST(SCOPE_IDENTITY() as int)
                                    ";

                try
                {
                    var result = _connection.Query<BlobVersion>(sql, blob).Single();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateBlobVersion: could not create blob", ex);
                }
            }
        }

        public bool UpdateBlobVersion(BlobVersion blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[BlobVersions]
                                SET [VersionId] = @VersionId
                                    ,[CreatedOn] = @CreatedOn
                                    ,[CreatedBy] = @CreatedBy
                                    ,[ValidFrom] = @ValidFrom
                                    ,[ValidTo] = @ValidTo
                                    ,[LastModified] = @LastModified
                                    ,[LastModifiedBy] = @LastModifiedBy
                                    ,[BlobContentsId] = @BlobContentsId
                                    ,[Etag] = @Etag
                                    ,[ContentLength] = @ContentLength
                                    ,[ContentType] = @ContentType
                                    ,[FileName] = @FileName
                                    ,[BlobId] = @BlobId
                                    ,[BlobVariantHash] = @BlobVariantHash
                                WHERE [dbo].[BlobVersions].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, blob);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateBlobVersion: could not update blob", ex);
                }
            }
        }

        public bool DeleteBlobVersion(BlobVersion blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[BlobVersions]
                                   SET [ValidTo] = @ValidTo
                                WHERE [dbo].[BlobVersions].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, new {blob.Id, ValidTo = DateTime.Now});

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("DeleteBlobVersion: could not delete blob", ex);
                }
            }
        }
        #endregion

        #region "Blob Contents"
        public BlobContents GetBlobContents(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                        ,[Content]
                                FROM [dbo].[BlobContents]
                                WHERE
                                    [dbo].[BlobContents].[Id] = @Id";

                try
                {
                    return _connection.Query<BlobContents>(sql, new {Id = id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetBlobContents(int id): could not get blob contents", ex);
                }
            }
        }

        public BlobContents CreateBlobContents(BlobContents blob)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO [dbo].[BlobContents]
                                       ([Content])
                                 VALUES
                                       (@Content);
                                ;
                                SELECT [Id]
                                        ,[Content]
                                FROM [dbo].[BlobContents]
                                WHERE
                                    [dbo].[BlobContents].[Id] = CAST(SCOPE_IDENTITY() as int)";

                try
                {
                    var result = _connection.Query<BlobContents>(sql, blob).Single();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateBlobContents: could not create blob", ex);
                }
            }
        }
        #endregion "Blob Contents"
    }
}

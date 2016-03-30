using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString;

        public TokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AuthenticationToken GetToken(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[Id] = @Id";

                try
                {
                    return _connection.Query<AuthenticationToken>(sql, new {Id = id}).Single();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetToken(int id): could not get token", ex);
                }
            }
        }

        public AuthenticationToken GetToken(Guid publicKey)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[PublicKey] = @Pk";

                try
                {
                    return _connection.Query<AuthenticationToken>(sql, new {Pk = publicKey}).Single();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetToken(Guid publicKey): could not get token", ex);
                }
            }
        }

        public AuthenticationToken GetValidToken(Guid publicKey)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[PublicKey] = @Pk
                                AND 
                                    [dbo].[AuthenticationTokens].[ValidFrom] < @TodaysDate
                                AND
	                                (
                                        [dbo].[AuthenticationTokens].[ValidTo] is null 
                                    OR 
                                        [dbo].[AuthenticationTokens].[ValidTo] > @TodaysDate
                                    )";

                try
                {
                    return
                        _connection.Query<AuthenticationToken>(sql, new {Pk = publicKey, TodaysDate = DateTime.Now})
                            .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetValidToken(Guid publicKey): could not get token", ex);
                }
            }
        }

        public bool IsTokenRestrictedToContainer(Guid publicKey)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[PublicKey] = @Pk
                                AND 
                                    [dbo].[AuthenticationTokens].[ValidFrom] < @TodaysDate
                                AND
	                                (
                                        [dbo].[AuthenticationTokens].[ValidTo] is null 
                                    OR 
                                        [dbo].[AuthenticationTokens].[ValidTo] > @TodaysDate
                                    )";

                try
                {
                    var token =
                        _connection.Query<AuthenticationToken>(sql, new {Pk = publicKey, TodaysDate = DateTime.Now})
                            .FirstOrDefault();

                    if (token == null)
                        throw new Exception("GetValidToken(Guid publicKey): could not get token");

                    return token.ContainerId != null && token.ContainerId != 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("GetValidToken(Guid publicKey): could not get token", ex);
                }
            }
        }

        public IEnumerable<AuthenticationToken> GetTokensForOwner(Guid id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[CreatedBy] = @Id";

                try
                {
                    return _connection.Query<AuthenticationToken>(sql, new {Id = id}).AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetTokensForOwner(Guid id): could not get tokens", ex);
                }
            }
        }

        public AuthenticationToken CreateToken(AuthenticationToken token)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"INSERT INTO [dbo].[AuthenticationTokens]
                                       ([PublicKey]
                                       ,[PrivateKey]
                                       ,[CreatedOn]
                                       ,[CreatedBy]
                                       ,[ValidFrom]
                                       ,[ValidTo]
                                       ,[ContainerId]
                                       ,[Description])
                                 VALUES
                                       (@PublicKey
                                       ,@PrivateKey
                                       ,@CreatedOn
                                       ,@CreatedBy
                                       ,@ValidFrom
                                       ,@ValidTo
                                       ,@ContainerId
                                       ,@Description);
                                SELECT [Id]
                                    ,[PublicKey]
                                    ,[PrivateKey]
                                    ,[CreatedOn]
                                    ,[CreatedBy]
                                    ,[ValidFrom]
                                    ,[ValidTo]
                                    ,[ContainerId]
                                    ,[Description]
                                FROM [dbo].[AuthenticationTokens]
                                WHERE
                                    [dbo].[AuthenticationTokens].[Id] = CAST(SCOPE_IDENTITY() as int)";

                try
                {
                    var result = _connection.Query<AuthenticationToken>(sql, token).Single();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("CreateToken: could not create token", ex);
                }
            }
        }

        public bool UpdateToken(AuthenticationToken token)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[AuthenticationTokens]
                                   SET [PublicKey] = @PublicKey
                                      ,[PrivateKey] = @PrivateKey
                                      ,[CreatedOn] = @CreatedOn
                                      ,[CreatedBy] = @CreatedBy
                                      ,[ValidFrom] = @ValidFrom
                                      ,[ValidTo] = @ValidTo
                                      ,[ContainerId] = @ContainerId
                                      ,[Description] = @Description
                                 WHERE [dbo].[AuthenticationTokens].[Id] = @Id";

                try
                {
                    var result = _connection.Execute(sql, token);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("UpdateToken: could not update token", ex);
                }
            }
        }

        public bool DeleteToken(AuthenticationToken token)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE [dbo].[AuthenticationTokens]
                    SET [ValidTo] = @ValidTo
                WHERE [dbo].[AuthenticationTokens].[Id] = @Id";
                try
                {
                    var result = _connection.Execute(sql, new {token.Id, ValidTo = DateTime.Now});

                    return result > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("DeleteToken: could not delete token", ex);
                }
            }
        }
    }
}

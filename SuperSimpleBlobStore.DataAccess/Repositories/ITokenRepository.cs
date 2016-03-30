using System;
using System.Collections.Generic;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public interface ITokenRepository
    {
        AuthenticationToken GetToken(int id);
        AuthenticationToken GetToken(Guid publicKey);
        AuthenticationToken GetValidToken(Guid publicKey);
        bool IsTokenRestrictedToContainer(Guid publicKey);
        IEnumerable<AuthenticationToken> GetTokensForOwner(Guid id);
        AuthenticationToken CreateToken(AuthenticationToken token);
        bool DeleteToken(AuthenticationToken token);
    }
}
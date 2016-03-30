using System;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public interface ITokens
    {
        Api.ViewModel.Tokens.AuthenticationToken CreateToken(Guid containerId, Guid creatorId, string description);
        Api.ViewModel.Tokens.AuthenticationToken GetToken(Guid publicKey);
        bool CanAccessContainer(Guid publicKey, Guid containerKey);
    }
}
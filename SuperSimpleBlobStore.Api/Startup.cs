using System;
using System.Linq;
using Nancy.TinyIoc;
using Owin;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.Domain.Providers;
using Thinktecture.IdentityModel.Hawk.Core;
using Thinktecture.IdentityModel.Hawk.Core.Helpers;
using Thinktecture.IdentityModel.Hawk.Owin;
using Thinktecture.IdentityModel.Hawk.Owin.Extensions;

namespace SuperSimpleBlobStore.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var ioCContainer = new TinyIoCContainer();

            ioCContainer.AutoRegister();
            
            ioCContainer.Register<ITokenRepository>(new TokenRepository(ConfigurationProvider.BlobStoreDatabaseConnectionString));
            ioCContainer.Register<IContainerRepository>(new ContainerRepository(ConfigurationProvider.BlobStoreDatabaseConnectionString));
            ioCContainer.Register<IBlobRepository>(new BlobRepository(ConfigurationProvider.BlobStoreDatabaseConnectionString));
            ioCContainer.Register<ITokens, Tokens>();
            ioCContainer.Register<IBlobs, Blobs>();
            ioCContainer.Register<IContainers, Containers>();

            var hawkOptions = new Options()
            {
                ClockSkewSeconds = 60,
                LocalTimeOffsetMillis = 0,
                CredentialsCallback = (id) =>
                {
                    var tokenProvider = ioCContainer.Resolve<ITokens>();

                    if (tokenProvider == null)
                        return null;

                    Guid publicKey;

                    if (!Guid.TryParse(id, out publicKey))
                        return null;

                    var token = tokenProvider.GetToken(publicKey);

                    if (token == null)
                        return null;

                    return new Credential
                    {
                        Id = token.PublicKey.ToString(),
                        Algorithm = SupportedAlgorithms.SHA256,
                        User = token.Description,
                        Key = token.PrivateKey.ToByteArray()
                    };
                },
                ResponsePayloadHashabilityCallback = (r) => true,
                VerificationCallback = (request, ext) =>
                {
                    if (String.IsNullOrEmpty(ext))
                        return true;

                    string name = ConfigurationProvider.TokenHeaderName;
                    return ext.Equals(name + ":" + request.Headers[name].First());
                }
            };

            var nancyBootstrapper = new Bootstrapper(ioCContainer);
            app
                .UseHawkAuthentication(new HawkAuthenticationOptions(hawkOptions))
                .UseNancy(options => options.Bootstrapper = nancyBootstrapper);
        }
    }
}

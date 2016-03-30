using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Cryptography;
using Nancy.Security;
using Nancy.Session;
using Nancy.TinyIoc;

namespace SuperSimpleBlobStore.Api
{
    public class Bootstrapper : CustomNancyBootstrapper
    {
        private readonly CryptographyConfiguration _cryptographyConfiguration = new CryptographyConfiguration(
            new RijndaelEncryptionProvider(
                new PassphraseKeyGenerator("{7F528BC6-567F-4CA5-83A0-E97A8E76A582}",
                    Guid.Parse("74CE4E75-5965-4CDE-8190-AC4ED8A03DC2").ToByteArray())
                ),
            new DefaultHmacProvider(
                new PassphraseKeyGenerator("{FE12EA1C-EA73-4FBA-B261-7CEBE07E9A0D}",
                    Guid.Parse("74CE4E75-5965-4CDE-8190-AC4ED8A03DC2").ToByteArray()
                    ))
            );

        protected override CryptographyConfiguration CryptographyConfiguration
        {
            get { return _cryptographyConfiguration; }
        }

        public Bootstrapper(TinyIoCContainer ioCContainer) : base(ioCContainer)
        {
            
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Csrf.Enable(pipelines);
            CookieBasedSessions.Enable(pipelines, _cryptographyConfiguration);
            StaticConfiguration.EnableHeadRouting = true;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<INancyBootstrapper, Bootstrapper>();
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "http://localhost:8089")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                    .WithHeader("Access-Control-Allow-Credentials", "true")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            });
        }
    }
}

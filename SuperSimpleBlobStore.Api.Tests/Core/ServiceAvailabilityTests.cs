using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.Modules;

namespace SuperSimpleBlobStore.Api.Tests.Core
{
    [TestFixture]
    public class ServiceAvailabilityTests
    {
        [Test]
        public void CanAccessService()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                with.Module<DefaultModule>();

            }));

            var result = browser.Get("/", with => {
                with.HttpRequest();
            });

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}

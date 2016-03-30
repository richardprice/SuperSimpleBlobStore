using Nancy;

namespace SuperSimpleBlobStore.Api.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/"] = _ =>
            {
                return "Ok";
            };
        }
    }
}

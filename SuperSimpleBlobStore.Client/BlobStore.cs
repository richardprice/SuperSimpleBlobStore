using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperSimpleBlobStore.Client.Exceptions;
using Thinktecture.IdentityModel.Hawk.Client;
using Thinktecture.IdentityModel.Hawk.Core;
using Thinktecture.IdentityModel.Hawk.Core.Helpers;

namespace SuperSimpleBlobStore.Client
{
    public class BlobStore
    {
        private BlobStoreConfiguration _configuration;

        public BlobStore(BlobStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Blob CreateBlob(Blob blob)
        {
            var credential = new Credential
            {
                Id = _configuration.PublicKey,
                Algorithm = SupportedAlgorithms.SHA256,
                User = "Not Used",
                Key = Guid.Parse(_configuration.PrivateKey).ToByteArray()
            };

            var options = GetOptions();

            options.CredentialsCallback = () => credential;

            var handler = new HawkValidationHandler(options);

            HttpClient client = HttpClientFactory.Create(handler);
            client.DefaultRequestHeaders.Add(
                                   _configuration.HeaderTagName, "secret");

            var multipart = new MultipartFormDataContent
            {
                {new StringContent(blob.ContainerIdentity.ToString()), "ContainerIdentity"},
                {new StringContent(blob.FileName), "FileName"},
                {new StringContent(blob.ContentType), "ContentType"},
                {new StringContent(blob.Etag), "Etag"},
                {new StreamContent(blob.Contents), "test", "test.jpg"}
            };

            var response = client.PostAsync(new Uri(_configuration.StoreAddress), multipart).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new FailedToCreateBlobException(response.ToString());
            }

            var result = JsonConvert.DeserializeObject<BlobStoreResponse>(response.Content.ReadAsStringAsync().Result);

            blob.BlobIdentity = result.BlobIdentity;

            return blob;
        }

        public Blob UpdateBlob(Blob blob)
        {
            var credential = new Credential
            {
                Id = _configuration.PublicKey,
                Algorithm = SupportedAlgorithms.SHA256,
                User = "Not Used",
                Key = Guid.Parse(_configuration.PrivateKey).ToByteArray()
            };

            var options = GetOptions();

            options.CredentialsCallback = () => credential;

            var handler = new HawkValidationHandler(options);

            HttpClient client = HttpClientFactory.Create(handler);
            client.DefaultRequestHeaders.Add(
                                   _configuration.HeaderTagName, "secret");

            var multipart = new MultipartFormDataContent
            {
                {new StringContent(blob.BlobIdentity.ToString()), "BlobId"},
                {new StringContent(blob.ContainerIdentity.ToString()), "ContainerIdentity"},
                {new StringContent(blob.FileName), "FileName"},
                {new StringContent(blob.ContentType), "ContentType"},
                {new StringContent(blob.Etag), "Etag"},
                {new StreamContent(blob.Contents), "test", "test.jpg"}
            };

            var response = client.PutAsync(new Uri(_configuration.StoreAddress), multipart).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new FailedToCreateBlobException(response.ToString());
            }

            var result = JsonConvert.DeserializeObject<BlobStoreResponse>(response.Content.ReadAsStringAsync().Result);

            blob.BlobIdentity = result.BlobIdentity;
            return blob;
        }

        private ClientOptions GetOptions()
        {
            return new ClientOptions()
            {
                RequestPayloadHashabilityCallback = (r) => true,
                NormalizationCallback = (req) =>
                {
                    string name = _configuration.HeaderTagName;
                    return req.Headers.ContainsKey(name) ?
                                name + ":" + req.Headers[name].First()
                                                              : null;
                }
            };
        }
    }
}

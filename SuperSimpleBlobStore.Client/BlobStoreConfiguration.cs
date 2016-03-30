namespace SuperSimpleBlobStore.Client
{
    public class BlobStoreConfiguration
    {
        public BlobStoreConfiguration()
        {
            HeaderTagName = "X-Storage-Token";
        }

        public string StoreAddress { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string HeaderTagName { get; set; }
    }
}

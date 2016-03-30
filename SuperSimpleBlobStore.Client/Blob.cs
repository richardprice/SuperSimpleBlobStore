using System;
using System.IO;
using SuperSimpleBlobStore.Client.Exceptions;

namespace SuperSimpleBlobStore.Client
{
    public class Blob
    {
        public Blob()
        {
            
        }

        public Blob(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new NoSuchFileExistsException();
            }

            Contents = File.OpenRead(filePath);
            Contents.Position = 0;
        }

        public Guid? BlobIdentity { get; set; }
        public Guid ContainerIdentity { get; set; }
        public string FileName { get; set; }
        public string Etag { get; set; }
        public string ContentType { get; set; }
        public Stream Contents { get; set; }
    }
}

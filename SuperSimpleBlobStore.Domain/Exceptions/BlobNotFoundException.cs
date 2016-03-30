using System;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class BlobNotFoundException : Exception
    {
        public BlobNotFoundException() : base()
        {
            
        }

        public BlobNotFoundException(string message) : base(message)
        {
            
        }

        public BlobNotFoundException(string message, Exception ex) : base(message, ex)
        {
            
        }
    }
}

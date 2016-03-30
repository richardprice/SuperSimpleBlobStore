using System;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotCreateBlobVersionException : Exception
    {
        public CouldNotCreateBlobVersionException() : base()
        {

        }

        public CouldNotCreateBlobVersionException(string message) : base(message)
        {

        }

        public CouldNotCreateBlobVersionException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

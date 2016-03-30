using System;
namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotDeleteBlobException : Exception
    {
        public CouldNotDeleteBlobException() : base()
        {

        }

        public CouldNotDeleteBlobException(string message) : base(message)
        {

        }

        public CouldNotDeleteBlobException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

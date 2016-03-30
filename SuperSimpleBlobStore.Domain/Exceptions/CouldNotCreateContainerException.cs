using System;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotCreateContainerException : Exception
    {
        public CouldNotCreateContainerException() : base()
        {

        }

        public CouldNotCreateContainerException(string message) : base(message)
        {

        }

        public CouldNotCreateContainerException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

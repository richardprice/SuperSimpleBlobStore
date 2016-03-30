using System;

namespace SuperSimpleBlobStore.Client.Exceptions
{
    public class NoSuchFileExistsException : Exception
    {
        public NoSuchFileExistsException() : base()
        {
            
        }

        public NoSuchFileExistsException(string message) : base(message)
        {

        }

        public NoSuchFileExistsException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Client.Exceptions
{
    public class FailedToCreateBlobException : Exception
    {
        public FailedToCreateBlobException() : base()
        {

        }

        public FailedToCreateBlobException(string message) : base(message)
        {

        }

        public FailedToCreateBlobException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

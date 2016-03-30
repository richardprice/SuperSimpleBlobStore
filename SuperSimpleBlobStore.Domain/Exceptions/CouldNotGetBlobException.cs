using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotGetBlobException : Exception
    {
        public CouldNotGetBlobException() : base()
        {

        }

        public CouldNotGetBlobException(string message) : base(message)
        {

        }

        public CouldNotGetBlobException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

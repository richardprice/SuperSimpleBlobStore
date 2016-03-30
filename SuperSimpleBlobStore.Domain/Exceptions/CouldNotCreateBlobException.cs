using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotCreateBlobException : Exception
    {
        public CouldNotCreateBlobException() : base()
        {

        }

        public CouldNotCreateBlobException(string message) : base(message)
        {

        }

        public CouldNotCreateBlobException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

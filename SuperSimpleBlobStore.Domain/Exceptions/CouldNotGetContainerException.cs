using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotGetContainerException : Exception
    {
        public CouldNotGetContainerException() : base()
        {

        }

        public CouldNotGetContainerException(string message) : base(message)
        {

        }

        public CouldNotGetContainerException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

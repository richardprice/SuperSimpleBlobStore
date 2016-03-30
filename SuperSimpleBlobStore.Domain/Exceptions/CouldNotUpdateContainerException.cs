using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotUpdateContainerException : Exception
    {
        public CouldNotUpdateContainerException() : base()
        {

        }

        public CouldNotUpdateContainerException(string message) : base(message)
        {

        }

        public CouldNotUpdateContainerException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

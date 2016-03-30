using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotUpdateTokenException : Exception
    {
        public CouldNotUpdateTokenException() : base()
        {

        }

        public CouldNotUpdateTokenException(string message) : base(message)
        {

        }

        public CouldNotUpdateTokenException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotGetTokenException : Exception
    {
        public CouldNotGetTokenException() : base()
        {

        }

        public CouldNotGetTokenException(string message) : base(message)
        {

        }

        public CouldNotGetTokenException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
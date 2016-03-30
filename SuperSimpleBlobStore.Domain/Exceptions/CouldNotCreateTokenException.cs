﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Domain.Exceptions
{
    public class CouldNotCreateTokenException : Exception
    {
        public CouldNotCreateTokenException() : base()
        {

        }

        public CouldNotCreateTokenException(string message) : base(message)
        {

        }

        public CouldNotCreateTokenException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}

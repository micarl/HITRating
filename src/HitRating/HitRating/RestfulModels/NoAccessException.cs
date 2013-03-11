using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HitRating.RestfulModels
{
    public class NoAccessException : Exception
    {
        public NoAccessException()
            : base()
        { 
        
        }

        public NoAccessException(string message)
            : base(message)
        { 
        
        }

        public NoAccessException(string message, Exception innerException)
            : base(message, innerException)
        { 
        
        }
    }
}
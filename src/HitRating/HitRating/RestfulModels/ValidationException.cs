using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HitRating.RestfulModels
{
    public class ValidationException : Exception
    {
        public IList<string> ValidationErrors { get; set; }

        public ValidationException() 
            : base("Validation Errors")
        {
            ValidationErrors = new List<string>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HitRating.RestfulJsonProccessor
{
    public class RestfulOption
    {
        public string Object { get; set; }
        public object ObjectId { get; set; }
        public byte ActionType { get; set; }
        public string HttpMethod { get; set; }
        public string Uri { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public string ActionDefinition { get; set; }
    }
}
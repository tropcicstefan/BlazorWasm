using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.Client.Services
{
    public class Response
    {
        public bool Success { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}

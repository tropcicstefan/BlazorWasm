using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWebAssemblyApp.Shared
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public UserForDetailedDto User { get; set; }
    }
}

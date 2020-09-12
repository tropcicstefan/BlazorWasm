using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorWebAssemblyApp.Shared
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public UserForDetailedDto User { get; set; }
    }
}

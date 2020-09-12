using BlazorWebAssemblyApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(UserForLoginDto loginModel);
        Task Logout();
        Task<RegisterResult> Register(UserForRegisterDto registerModel);
    }
}

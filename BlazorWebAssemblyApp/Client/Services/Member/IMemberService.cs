using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWebAssemblyApp.Shared;

namespace MeetMe.Client.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<UserForListDto>> GetUsers(UserParams userParams);
        Task<UserForDetailedDto> GetUser(string id);
        Task<Response> SendLike(string recipientId);

        Task<Response> UpdateUser(UserForUpdateDto user);
    }
}

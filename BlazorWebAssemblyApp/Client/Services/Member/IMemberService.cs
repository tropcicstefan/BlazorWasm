using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWebAssemblyApp.Shared;

namespace MeetMe.Client.Services
{
    public interface IMemberService
    {
        Task<(IEnumerable<UserForListDto>, PaginationHeader)> GetUsers(UserParams userParams);
        Task<UserForDetailedDto> GetUser(string id);
        Task<Response> SendLike(string recipientId);
        Task<Response> UpdateUser(UserForUpdateDto user);
        Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId);
        Task<IEnumerable<MessageToReturnDto>> GetMessages(int userId, MessageParams messageParams);
        Task<Response> MarkMessageAsRead(int userId, int messageId);
        Task<Response> SendMessage(MessageForCreationDto message);
        Task<Response> DeleteMessage(int userId, int messageId);
    }
}

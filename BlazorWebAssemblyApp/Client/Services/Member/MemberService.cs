using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorWebAssemblyApp.Shared;
using MeetMe.Client.Helpers;

namespace MeetMe.Client.Services
{
    public class MemberService : IMemberService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;


        public MemberService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<UserForListDto>> GetUsers(UserParams userParams)
        {
            var user = await _localStorage.GetItemAsync<UserForDetailedDto>("user");
            userParams.UserId = user.ID;
            string userParamsAsJson = userParams.ToQueryString();
            var result = await _httpClient.GetAsync($"api/users/?{userParamsAsJson}");
            var usersResult = JsonSerializer.Deserialize<IEnumerable<UserForListDto>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result.IsSuccessStatusCode)
            {
                return usersResult;
            }

            return null;
        }
        
        public async Task<UserForDetailedDto> GetUser(string id)
        {
            var result = await _httpClient.GetAsync($"api/users/{id}");
            var userResult = JsonSerializer.Deserialize<UserForDetailedDto>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result.IsSuccessStatusCode)
            {
                return userResult;
            }

            return null;
        }
        
        public async Task<Response> SendLike(string recipientId)
        {
            try
            {
                var user = await _localStorage.GetItemAsync<UserForDetailedDto>("user");
                var result = await _httpClient.PostAsync($"api/users/{user.ID}/like/{recipientId}", null);
                string message = result.Content.ReadAsStringAsync().Result;
                var response = new Response
                {
                    Success = result.IsSuccessStatusCode,
                    StatusCode = result.StatusCode.ToString(),
                    ErrorMessage = message
                };
                return response;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<Response> UpdateUser(UserForUpdateDto user)
        {
            string userAsJson = JsonSerializer.Serialize(user);
            var result = await _httpClient.PutAsync($"api/users/{user.ID}", new StringContent(userAsJson, Encoding.UTF8, "application/json"));

            var response = JsonSerializer.Deserialize<Response>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return response;

        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId)
        {
            var result = await _httpClient.GetAsync($"api/users/{userId}/messages/thread/{recipientId}");
            var messages = JsonSerializer.Deserialize<IEnumerable<MessageToReturnDto>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result.IsSuccessStatusCode)
            {
                return messages.OrderBy(m => m.MessageSent);
            }

            return null;
        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessages(int userId, MessageParams messageParams)
        {
            string messageParamsAsJson = messageParams.ToQueryString();
            var result = await _httpClient.GetAsync($"api/users/{userId}/messages/?{messageParamsAsJson}");
            var messages = JsonSerializer.Deserialize<IEnumerable<MessageToReturnDto>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result.IsSuccessStatusCode)
            {
                return messages;
            }

            return null;
        }

        public async Task<Response> MarkMessageAsRead(int userId, int messageId)
        {
            var result = await _httpClient.PostAsync($"api/users/{userId}/messages/{messageId}/read", null);
            string message = result.Content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
            {
                var response = new Response
                {
                    Success = result.IsSuccessStatusCode,
                    StatusCode = result.StatusCode.ToString(),
                    ErrorMessage = message
                };

                return response;
            }
            return null;
        }
        
        public async Task<Response> SendMessage(MessageForCreationDto message)
        {
            string messageAsJson = JsonSerializer.Serialize(message);
            var result = await _httpClient.PostAsync($"api/users/{message.SenderID}/messages", new StringContent(messageAsJson, Encoding.UTF8, "application/json"));
            
            if (result.IsSuccessStatusCode)
            {
                var response = new Response
                {
                    Success = result.IsSuccessStatusCode,
                    StatusCode = result.StatusCode.ToString(),
                    ErrorMessage = result.Content.ReadAsStringAsync().Result
                };

                return response;
            }
            return null;
        }
        
        public async Task<Response> DeleteMessage(int userId, int messageId)
        {
            var result = await _httpClient.PostAsync($"api/users/{userId}/messages/{messageId}", null);            

            var response = new Response
            {
                Success = result.IsSuccessStatusCode,
                StatusCode = result.StatusCode.ToString(),
                ErrorMessage = result.Content.ReadAsStringAsync().Result
            };

            return response;
        }
    }
}

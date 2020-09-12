using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MeetMe.Client.Services
{
    public class AlertifyService : IAlertifyService
    {
        private readonly IJSRuntime _jSRuntime;

        public AlertifyService(IJSRuntime jSRuntime)
        {
            this._jSRuntime = jSRuntime;
        }

        public async Task Error(string message)
        {
            await _jSRuntime.InvokeVoidAsync("error", message);
        }

        public async Task Success(string message)
        {
            await _jSRuntime.InvokeVoidAsync("success", message);
        }
    }
}

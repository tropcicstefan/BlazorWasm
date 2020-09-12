using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.Client.Services
{
    public interface IAlertifyService
    {
        Task Success(string message);
        Task Error(string message);
    }
}

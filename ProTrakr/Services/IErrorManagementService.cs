using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProTrakr.Services
{
    public interface IErrorManagementService
    {
        Dictionary<string, Action> KnownExceptions { get; }
        Task HandleError(string message);
        Task HandleError(Exception ex);
        Task HandleError(string message, Exception ex);
    }
}
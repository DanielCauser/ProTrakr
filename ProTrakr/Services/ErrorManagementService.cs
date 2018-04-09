using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Logging;

namespace ProTrakr.Services
{
    public class ErrorManagementService : IErrorManagementService
    {
        public Dictionary<string, Action> KnownExceptions { get; } = new Dictionary<string, Action>();

        private readonly ILoggerFacade _loggerFacade;

        public ErrorManagementService(ILoggerFacade loggerFacade)
        {
            _loggerFacade = loggerFacade;
        }

        public virtual Task HandleError(Exception ex)
        {
            if (HandleKnownException(ex)) return Task.CompletedTask;
            _loggerFacade.Log(ex.ToString(), Category.Exception, Priority.High);
            return Task.CompletedTask;
        }

        public virtual Task HandleError(string message, Exception ex)
        {
            if (HandleKnownException(ex)) return Task.CompletedTask;
            _loggerFacade.Log($"{message}{Environment.NewLine}{ex}", Category.Exception, Priority.High);
            return Task.CompletedTask;
        }

        public virtual Task HandleError(string message)
        {
            _loggerFacade.Log(message, Category.Exception, Priority.High);
            return Task.CompletedTask;
        }

        protected bool HandleKnownException(Exception exception)
        {
            exception = exception is AggregateException ? exception.InnerException : exception;

            var exceptionName = exception.GetType().Name;
            if (KnownExceptions.ContainsKey(exceptionName))
            {
                var handler = KnownExceptions[exceptionName];
                handler.Invoke();
                return true;
            }
            return false;
        }
    }
}
using System;
using Prism.Logging;

namespace ProTrakr.Services
{
    public class ConsoleLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            string messageToLog = $"{message}: {category.ToString().ToUpper()}. Priority: {priority}. Timestamp: {DateTime.Now:u}";
            Console.WriteLine(messageToLog);
        }
    }
}

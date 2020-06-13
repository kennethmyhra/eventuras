using System;

namespace EventManagement.Services.Converto
{
    internal class ConvertoClientException : Exception
    {
        public ConvertoClientException(string message) : base(message)
        {
        }

        public ConvertoClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

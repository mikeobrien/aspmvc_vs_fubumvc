using System;

namespace Core.Infrastructure.Logging
{
    public interface ILogger
    {
        void LogException(Exception exception);
    }
}
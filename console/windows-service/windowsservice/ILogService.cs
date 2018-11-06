using NLog;

namespace windowsservice
{
    interface ILogService
    {
        ILogger Logger { get; }
    }
}

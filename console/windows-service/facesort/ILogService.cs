using NLog;

namespace FaceSort
{
    public interface ILogService
    {
        ILogger Logger { get; }
    }
}
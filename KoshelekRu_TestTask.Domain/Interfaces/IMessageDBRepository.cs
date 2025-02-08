using KoshelekRu_TestTask.Domain.Models;

namespace KoshelekRu_TestTask.Domain.Interfaces
{
    public interface IMessageDBRepository : IDisposable
    {
        Task<OperationResult> Write(string message);
        Task<Message> GetByTime(TimeSpan timeInterval);
    }
}
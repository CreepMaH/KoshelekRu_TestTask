using TestTask.Domain.Models;

namespace TestTask.Domain.Interfaces
{
    public interface IMessageDBRepository : IDisposable
    {
        Task<OperationResult> Write(string message);
        Task<Message> GetByTime(TimeSpan timeInterval);
    }
}
using TestTask.Domain.Models;

namespace TestTask.Domain.Interfaces
{
    public interface IMessageDBRepository : IDisposable
    {
        Task<OperationResult> Write(Message message);
        Task<IEnumerable<Message>> GetByTime(TimeSpan timeInterval);
    }
}
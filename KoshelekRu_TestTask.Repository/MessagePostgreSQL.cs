using KoshelekRu_TestTask.Domain.Interfaces;
using KoshelekRu_TestTask.Domain.Models;

namespace KoshelekRu_TestTask.Repository
{
    public class MessagePostgreSQL : IMessageDBRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetByTime(TimeSpan timeInterval)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Write(string message)
        {
            throw new NotImplementedException();
        }
    }
}

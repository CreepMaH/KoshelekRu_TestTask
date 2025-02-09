using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Repository
{
    public class MessagePostgreSQL : IMessageDBRepository
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<Message> GetByTime(TimeSpan timeInterval)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Write(string message)
        {
            return Task.FromResult(new OperationResult { IsSuccess = true });
            //throw new NotImplementedException();
        }
    }
}

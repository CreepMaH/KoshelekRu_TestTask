using KoshelekRu_TestTask.Domain.Models;

namespace KoshelekRu_TestTask.Interfaces
{
    public interface IMessageClient
    {
        Task<string> Receive(string message);
    }
}

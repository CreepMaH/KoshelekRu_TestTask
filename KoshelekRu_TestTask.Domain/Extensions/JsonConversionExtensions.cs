using System.Text.Json;
using TestTask.Domain.Models;

namespace TestTask.Domain.Extensions
{
    public static class JsonConversionExtensions
    {
        public static string ToJsonString(this Message message)
        {
            return JsonSerializer.Serialize(message);
        }

        public static Message JsonStringToMessage(this string message)
        {
            return JsonSerializer.Deserialize<Message>(message)
                ?? new Message();
        }
    }
}

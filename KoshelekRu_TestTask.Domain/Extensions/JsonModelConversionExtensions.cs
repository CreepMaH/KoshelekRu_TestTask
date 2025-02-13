using System.Text.Json;
using TestTask.Domain.Models;

namespace TestTask.Domain.Extensions
{
    /// <summary>
    /// Used to serialize and deserialize Message model
    /// </summary>
    public static class JsonModelConversionExtensions
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

using System.ComponentModel.DataAnnotations;

namespace KoshelekRu_TestTask.Domain.Models
{
    public class Message
    {
        public ulong Id { get; set; }

        public ulong IndexNumber { get; set; }

        [StringLength(128)]
        public string? Text { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}

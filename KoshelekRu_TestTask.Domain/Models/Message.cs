using System.ComponentModel.DataAnnotations;

namespace TestTask.Domain.Models
{
    public class Message
    {
        /// <summary>
        /// ID in database
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// Number set up by client
        /// </summary>
        public ulong IndexNumber { get; set; }

        [StringLength(128)]
        public string? Text { get; set; }

        /// <summary>
        /// Server timestamp
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}

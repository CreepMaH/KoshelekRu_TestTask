using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Clients.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class GetMessagesController(IMessageDBRepository dBRepository)
        : ControllerBase
    {
        private readonly IMessageDBRepository _dBRepository = dBRepository;

        /// <summary>
        /// Returns 10 last messages from DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Message>> GetLast10Messages()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(10);
            return await _dBRepository.GetByTime(timeSpan);
        }
    }
}

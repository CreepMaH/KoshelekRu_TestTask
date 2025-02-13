using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.Clients.Services;
using TestTask.Domain.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Clients.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GetMessagesController(IMessageDBRepository dBRepository)
        : ControllerBase
    {
        private readonly IMessageDBRepository _dBRepository = dBRepository;

        [HttpGet]
        public async Task<IEnumerable<Message>> GetLast10Messages()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(10);
            return await _dBRepository.GetByTime(timeSpan);
        } 
    }
}

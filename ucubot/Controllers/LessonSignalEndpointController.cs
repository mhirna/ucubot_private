using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;
using ucubot.Databases;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILessonSignalRepository _repository;

        public LessonSignalEndpointController(IConfiguration configuration, ILessonSignalRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");

            return _repository.ShowSignals(connectionString);
            
        }

        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            LessonSignalDto signal = _repository.ShowSignal(connectionString, id);
            
            if (signal == null){Response.StatusCode = 404;}
            
            return signal;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            
            int result = _repository.CreateSignal(connectionString, message);

            if (result == 0)
            {
                return BadRequest();
            }

            return Accepted();
        }
               
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");      
            
            _repository.RemoveSignal(connectionString, id);
            
            return Accepted();
        }
    }
}

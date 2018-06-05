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
        private readonly ILessonSignalRepository _repository;

        public LessonSignalEndpointController(ILessonSignalRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            return _repository.ShowSignals();   
        }

        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            LessonSignalDto signal = _repository.ShowSignal(id);
            
            if (signal == null){Response.StatusCode = 404;}
            
            return signal;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            int result = _repository.CreateSignal(message);

            if (result == 0)
            {
                return BadRequest();
            }

            return Accepted();
        }
               
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            _repository.RemoveSignal(id);
            
            return Accepted();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;
using ucubot.Databases;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IStudentRepository _repository;

        public StudentEndpointController(IConfiguration configuration, IStudentRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }
        
        [HttpGet]
        public IEnumerable<Student> ShowStudent()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            
            return _repository.ShowStudents(connectionString);
        }

        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");

            Student student = _repository.ShowStudent(connectionString, id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return student;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentDet info)
        {

            var connectionString = _configuration.GetConnectionString("BotDatabase");
            int result = _repository.CreateStudent(connectionString, info);
            if (result == 409){return StatusCode(409);}
            
            return Accepted();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {

            var connectionString = _configuration.GetConnectionString("BotDatabase");
      
            int result = _repository.UpdateStudent(connectionString, student);
            if (result == 409){return StatusCode(409);}
            if (result == 404){return StatusCode(404);}      
            
            return Accepted();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");

            int result = _repository.RemoveSignal(connectionString, id);
            if (result == 409){return StatusCode(409);}
            
            return Accepted();
        }
    }
}

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
        private readonly IStudentRepository _repository;

        public StudentEndpointController(IStudentRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IEnumerable<Student> ShowStudent()
        {
            
            return _repository.ShowStudents();
        }

        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {

            Student student = _repository.ShowStudent(id);

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
            int result = _repository.CreateStudent(info);
            if (result == 409){return StatusCode(409);}
            
            return Accepted();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
      
            int result = _repository.UpdateStudent(student);
            if (result == 409){return StatusCode(409);}
            if (result == 404){return StatusCode(404);}      
            
            return Accepted();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            int result = _repository.RemoveSignal(id);
            if (result == 409){return StatusCode(409);}
            
            return Accepted();
        }
    }
}

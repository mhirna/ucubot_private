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
    public class StudentSignalsEndpointController : Controller
    {
        private readonly IStudentSignalRepository _repository;

        public StudentSignalsEndpointController(IStudentSignalRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IEnumerable<StudentSignal> ShowStudent()
        {
            return _repository.ShowStudentSignals();
        }
    }
}

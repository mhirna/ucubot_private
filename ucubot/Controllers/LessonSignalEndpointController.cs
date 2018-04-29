﻿using System;
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

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        public LessonSignalEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT lesson_signal.Id, lesson_signal.SignalType, " +
                              "lesson_signal.Timestamp, student.user_id AS UserId FROM lesson_signal " +
                              "INNER JOIN student ON lesson_signal.student_id=student.id;";
                List<LessonSignalDto> signals = conn.Query<LessonSignalDto>(command).ToList();
                return signals;
            }
        }

        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT lesson_signal.Id, lesson_signal.SignalType, " +
                                               "lesson_signal.Timestamp, student.user_id AS UserId FROM lesson_signal " +
                                               "INNER JOIN student ON lesson_signal.student_id=student.id " +
                                               "WHERE lesson_signal.Id=@id;";
                LessonSignalDto signal = conn.Query<LessonSignalDto>(command, new {id}).SingleOrDefault();
                if (signal == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }                
                return signal;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();

            var connectionString = _configuration.GetConnectionString("BotDatabase");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var check_com = "SELECT id FROM student WHERE user_id=@UserId";
                var num_stud = conn.ExecuteScalar<object>(check_com, new {userId});
                if (num_stud == null)
                {
                    return BadRequest();
                }
                else
                {
                    var id = Convert.ToInt32(num_stud);
                    var command = "INSERT INTO lesson_signal (SignalType, student_id) VALUES (@SignalType, @Id);";
                    conn.Execute(command,
                        new
                        {
                            signalType,
                            id
                        });
                    conn.Close();
                }
            }
            return Accepted();
}
               
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            //TODO: add delete command to remove signal
            var connectionString = _configuration.GetConnectionString("BotDatabase");

      
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = new MySqlCommand("DELETE FROM lesson_signal WHERE Id = @id;", conn);
                command.Parameters.Add(new MySqlParameter("Id", id));
                command.ExecuteNonQuery();
                conn.Close();
            }
            
            return Accepted();
        }
    }
}

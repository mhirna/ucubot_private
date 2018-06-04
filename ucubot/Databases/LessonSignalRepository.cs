using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Databases
{
    public class LessonSignalRepository: ILessonSignalRepository
    {
        public IEnumerable<LessonSignalDto> ShowSignals(string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT lesson_signal.Id, lesson_signal.SignalType as Type, " +
                              "lesson_signal.Timestamp, student.user_id AS UserId FROM lesson_signal " +
                              "INNER JOIN student ON lesson_signal.student_id=student.id;";
                List<LessonSignalDto> signals = conn.Query<LessonSignalDto>(command).ToList();
                return signals;
            };
        }

        public LessonSignalDto ShowSignal(string connectionString, long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT lesson_signal.Id, lesson_signal.SignalType AS Type, " +
                              "lesson_signal.Timestamp, student.user_id AS UserId FROM lesson_signal " +
                              "INNER JOIN student ON lesson_signal.student_id=student.id " +
                              "WHERE lesson_signal.Id=@Id;";
                LessonSignalDto signal = conn.Query<LessonSignalDto>(command, new {Id = id}).SingleOrDefault();
                if (signal == null)
                {
                    return null;
                }                
                return signal;
            }
        }

        public int CreateSignal(string connectionString, SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = Convert.ToInt32(message.text.ConvertSlackMessageToSignalType());

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var check_com = "SELECT id FROM student WHERE user_id=@UserId";
                var num_stud = conn.ExecuteScalar<object>(check_com, new {userId});
                if (num_stud == null)
                {
                    return 0;
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
            return 1;

        }

        public void RemoveSignal(string connectionString, long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = new MySqlCommand("DELETE FROM lesson_signal WHERE Id = @id;", conn);
                command.Parameters.Add(new MySqlParameter("Id", id));
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Databases
{
    public class StudentSignalRepository: IStudentSignalRepository
    {
        public IEnumerable<StudentSignal> ShowStudentSignals(string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT * FROM student_signals;";
                List<StudentSignal> signals = conn.Query<StudentSignal>(command).ToList();
                return signals;
            }
        }
    }
}
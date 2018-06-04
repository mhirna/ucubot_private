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
                var command = "SELECT any_value(student_signals.first_name) AS FirstName, any_value(student_signals.last_name) AS LastName, " +
                              "any_value(student_signals.signal_type) AS SignalType, any_value(student_signals.count) AS Count FROM student_signals;";                
                List<StudentSignal> signals = conn.Query<StudentSignal>(command).ToList();
                return signals;
            }
        }
    }
}

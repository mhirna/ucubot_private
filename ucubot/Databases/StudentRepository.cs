using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Databases
{
    public class StudentRepository: IStudentRepository
    {
        public IEnumerable<Student> ShowStudents(string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT student.id AS Id, student.first_name AS FirstName, " +
                              "student.last_name AS LastName, student.user_id AS UserId FROM student;";
                List<Student> signals = conn.Query<Student>(command).ToList();
                return signals;
            }
        }

        public Student ShowStudent(string connectionString, long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var command = "SELECT student.id AS Id, student.first_name AS FirstName, " +
                              "student.last_name AS LastName, student.user_id AS UserId FROM student WHERE student.id=@id;";
                Student student = conn.Query<Student>(command, new {id}).SingleOrDefault();
                if (student == null)
                {
                    return null;
                }

                return student;
            }

        }

        public int CreateStudent(string connectionString, StudentDet info)
        {
            var firstName = info.FirstName;
            var lastName = info.LastName;
            var userId = info.UserId;
            
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var countSql = "SELECT COUNT(*) FROM student WHERE user_id=@UserId";
                var num_stud = conn.ExecuteScalar<int>(countSql, new {userId});
                if (num_stud == 1)
                {
                    return 409;
                }
                else{
                    var sqlQuery = "INSERT INTO student(first_name, last_name, user_id) VALUES (@FirstName,@LastName,@UserId)";
                    conn.Execute(sqlQuery,
                        new
                        {
                            firstName,
                            lastName,
                            userId
                        });
                }
            }
            return 0;
        }

        public int UpdateStudent(string connectionString, Student student)
        {
            var firstName = student.FirstName;
            var lastName = student.LastName;
            var userId = student.UserId;
            var Id = student.Id;
            
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var countSql1 = "SELECT COUNT(*) FROM student WHERE user_id=@UserId";
                var num_stud = conn.ExecuteScalar<int>(countSql1, new {userId});
                var countSql2 = "SELECT COUNT(*) FROM lesson_signal WHERE student_id=@Id";
                var num_sig = conn.ExecuteScalar<int>(countSql2, new {Id});

                
                if (num_stud == 0)
                {
                    return 404;
                }

                if (num_sig > 0)
                {
                    return 409;
                }
                else
                {
                    var sqlQuery =
                        "UPDATE student SET first_name=@FirstName, last_name=@LastName, user_id=@UserId WHERE id=@Id;";
                    conn.Execute(sqlQuery,
                        new
                        {
                            firstName,
                            lastName,
                            userId,
                            Id,
                        });
                }
            }
            return 0;
        }

        public int RemoveSignal(string connectionString, long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var countSql = "SELECT COUNT(*) FROM lesson_signal WHERE student_id=@Id";
                var num_sig = conn.ExecuteScalar<int>(countSql, new {id});
                if (num_sig > 0)
                {
                    return 409;
                }
                else
                {
                    var sqlQuery = "DELETE FROM student WHERE id=@Id;";
                    conn.Execute(sqlQuery,
                        new {id});
                }
            }

            return 0;

        }
    }
}
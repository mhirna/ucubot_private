using System.Collections.Generic;
using ucubot.Model;

namespace ucubot.Databases
{
    public interface IStudentRepository
    {
        IEnumerable<Student> ShowStudents(string connectionString);
        Student ShowStudent(string connectionString, long id);
        int CreateStudent(string connectionString, StudentDet info);
        int UpdateStudent(string connectionString, Student student);
        int RemoveSignal(string connectionString, long id);
    }
}
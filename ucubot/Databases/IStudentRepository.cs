using System.Collections.Generic;
using ucubot.Model;

namespace ucubot.Databases
{
    public interface IStudentRepository
    {
        IEnumerable<Student> ShowStudents();
        Student ShowStudent(long id);
        int CreateStudent(StudentDet info);
        int UpdateStudent(Student student);
        int RemoveSignal(long id);
    }
}

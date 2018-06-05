using System.Collections.Generic;
using ucubot.Model;

namespace ucubot.Databases
{
    public interface IStudentSignalRepository
    {
        IEnumerable<StudentSignal> ShowStudentSignals();
    }
}

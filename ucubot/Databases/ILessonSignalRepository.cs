using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ucubot.Model;

namespace ucubot.Databases
{
    public interface ILessonSignalRepository
    {
        IEnumerable<LessonSignalDto> ShowSignals(string connectionString);
        LessonSignalDto ShowSignal(string connectionString, long id);
        int CreateSignal(string connectionString, SlackMessage message);
        void RemoveSignal(string connectionString, long id);
    }
}
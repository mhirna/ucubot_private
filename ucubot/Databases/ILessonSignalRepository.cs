using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ucubot.Model;

namespace ucubot.Databases
{
    public interface ILessonSignalRepository
    {
        IEnumerable<LessonSignalDto> ShowSignals();
        LessonSignalDto ShowSignal(long id);
        int CreateSignal(SlackMessage message);
        void RemoveSignal(long id);
    }
}

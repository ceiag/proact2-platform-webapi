using Proact.Services.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices.Surveys.Scheduler;

public interface ISurveySchedulerDispatcherService : IDataEditorService {
    public Task SendSurveyToPatientsNow( List<SurveyScheduler> schedulers );
    public Task SendSurveyToPatientsForToday();
    public Task SendReminderForExpiringSurveys();
}

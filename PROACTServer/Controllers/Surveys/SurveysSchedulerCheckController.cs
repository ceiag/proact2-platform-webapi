using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using Proact.Services.QueriesServices.Surveys.Scheduler;

namespace Proact.Services.Controllers.Surveys;

[ApiController]
[Route( ProactRouteConfiguration.DefaultRoute )]
public class SurveysSchedulerCheckController : Controller {
    private readonly ISurveySchedulerDispatcherService _surveySchedulerDispatcherService;
    public SurveysSchedulerCheckController(
        ISurveySchedulerDispatcherService surveySchedulerDispatcherService ) {
        _surveySchedulerDispatcherService = surveySchedulerDispatcherService;
    }

    /// <summary>
    /// Send notifications to scheduled surveys
    /// </summary>
    [HttpPost]
    [SwaggerResponse( (int)HttpStatusCode.OK )]
    public async Task<IActionResult> CheckScheduledSurveys() {
        await _surveySchedulerDispatcherService.SendSurveyToPatientsForToday();
        return Ok();
    }

    /// <summary>
    /// Send reminder for unanswered scheduled surveys
    /// </summary>
    [HttpPost]
    [Route( "reminder" )]
    [SwaggerResponse( (int)HttpStatusCode.OK )]
    public async Task<IActionResult> CheckReminderScheduledSurveys() {
        await _surveySchedulerDispatcherService.SendReminderForExpiringSurveys();
        return Ok();
    }
}

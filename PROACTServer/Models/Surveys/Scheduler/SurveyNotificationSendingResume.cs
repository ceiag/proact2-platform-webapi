using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Proact.Services.Models;
public class SurveyNotificationSendingResume {
    public HttpResponseMessage HttpResponseMessage { get; set; }
    public List<SurveySchedulerModel> SurveySchedulersExecuted { get; set; }
        = new List<SurveySchedulerModel>();
    public List<SurveyAssignationModel> SurveyAssignation { get; set; }
    public List<Device> Devices { get; set; } = new List<Device>();
    public List<Guid> PlayerIds { get; set; } = new List<Guid>();

    public DateTime ExecutionTimeUtc { get; set; }

    public string Serialized {
        get {
            return $"PlayerIds Sent: {PlayerIds.Count}, " +
                $"Scheduler Executed: {SurveySchedulersExecuted.Count}, " +
                $"Surveys assigned: {SurveyAssignation.Count}, " +
                $"HttpResponse: {HttpResponseMessage?.StatusCode}";
        }
    }
}

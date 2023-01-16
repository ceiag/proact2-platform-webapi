using Proact.Services.Models.Exportations;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Surveys.Stats;
using System;

namespace Proact.Services.Exporters;

public class ProactDataExporterService : IProactDataExporterService {
    private readonly ISurveyStatsOverTimeQueriesService _surveyStatsOverTimeQueriesService;
    private readonly IPatientQueriesService _patientQueriesService;
    private readonly IMessageAnalysisQueriesService _messageAnalysisQueriesService;
    private readonly IProjectQueriesService _projectQueriesService;

    public ProactDataExporterService( 
        ISurveyStatsOverTimeQueriesService surveyStatsOverTimeQueriesService,
        IPatientQueriesService patientQueriesService,
        IMessageAnalysisQueriesService messageAnalysisQueriesService,
        IProjectQueriesService projectQueriesService ) {
        _surveyStatsOverTimeQueriesService = surveyStatsOverTimeQueriesService;
        _patientQueriesService = patientQueriesService;
        _messageAnalysisQueriesService = messageAnalysisQueriesService;
        _projectQueriesService = projectQueriesService;
    }

    public SurveyStatsExportResult ExportPatientSurveyAnswers( Guid surveyId, Guid userId, ISurveyAnswersExporter exporter ) {
        var surveyStats = _surveyStatsOverTimeQueriesService.Get( surveyId, userId );
        var patient = _patientQueriesService.Get( userId );

        return exporter.Export( patient.Code, surveyStats );
    }

    public AnalysisExportResult ExportMessagesFromPatient( Guid userId, IAnalysisExporter exporter ) {
        var analysis = _messageAnalysisQueriesService.GetAllAnalysisFromPatient( userId );
        var project = _projectQueriesService.GetProjectsWhereUserIsAssociated( userId )[0];

        return exporter.Export( project.ProjectProperties.Lexicon, analysis );
    }
}

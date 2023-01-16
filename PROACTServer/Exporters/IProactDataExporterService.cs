using Proact.Services.Models.Exportations;
using System;

namespace Proact.Services.Exporters;

public interface IProactDataExporterService {
    public SurveyStatsExportResult ExportPatientSurveyAnswers( 
        Guid surveyId, 
        Guid userId, 
        ISurveyAnswersExporter exporter );
    public AnalysisExportResult ExportMessagesFromPatient(
        Guid userId,
        IAnalysisExporter exporter );
}

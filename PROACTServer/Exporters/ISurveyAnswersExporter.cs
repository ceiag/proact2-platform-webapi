using Proact.Services.Models.Exportations;
using Proact.Services.Models.SurveyStats;

namespace Proact.Services.Exporters;

public interface ISurveyAnswersExporter {
    public SurveyStatsExportResult Export( string userCode, SurveyStatsResumeByTime surveyStats );
}

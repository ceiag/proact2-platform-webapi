using Proact.Services.Models.Exportations;
using Proact.Services.Models.SurveyStats;
using System;

namespace Proact.Services.Exporters;

public class CsvFormatSurveyExporter : ISurveyAnswersExporter {
    public SurveyStatsExportResult Export( string userCode, SurveyStatsResumeByTime surveyStats ) {
        string csvResult = "Title;Description;Version;\n";
        csvResult += $"{surveyStats.Title};{surveyStats.Description};{surveyStats.Version}\n;\n";

        csvResult += "UserCode;Question;Answer;Time\n";

        foreach ( var question in surveyStats.Questions ) {
            int i = 0;
            foreach ( var answer in question.Answers ) {
                csvResult += $"{userCode};{question.Question};";
                csvResult += $"{answer.Answers[i]};{answer.Date}";
                csvResult += "\n";
            }

            csvResult += "\n";
        }

        return new SurveyStatsExportResult( "csv", csvResult );
    }
}

using Proact.Services.Entities;
using Proact.Services.Exporters;
using Proact.Services.Models.SurveyStats;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Exporters;
public class SurveysCsvExporterCreate {
    [Fact( DisplayName = "Export Surveys in CSV format, check correctness" )]
    public void ExportInCsvFormat() {
        var survey = new SurveyStatsResumeByTime() {
            Title = "Survey One",
            Description = "Amazing Description",
            Version = "1.0",
            StartTime = DateTime.Parse( "21/10/2022 10:24:08" ),
            ExpireTime = DateTime.Parse( "21/10/2022 10:24:08" ),
            Questions = new List<SurveyStatsQuestion>() {
                new SurveyStatsQuestion() {
                    Id = Guid.NewGuid(),
                    Type = SurveyQuestionType.OPEN_ANSWER,
                    Question = "question 1",
                    Answers = new List<SurveyStatsAnswer>() {
                        new SurveyStatsAnswer() {
                            Date = DateTime.Parse("21/10/2022 10:24:08"),
                            Answers = new List<string>() {
                                "answer"
                            }
                        }
                    }
                },
                new SurveyStatsQuestion() {
                    Id = Guid.NewGuid(),
                    Type = SurveyQuestionType.RATING,
                    Question = "question 2",
                    Answers = new List<SurveyStatsAnswer>() {
                        new SurveyStatsAnswer() {
                            Date = DateTime.Parse("21/10/2022 10:24:08"),
                            Answers = new List<string>() {
                                "1"
                            }
                        }
                    }
                }
            }
        };

        var csvResult = new CsvFormatSurveyExporter().Export( "1900x", survey );

        string expctedResult = "Title;Description;Version;\nSurvey One;Amazing Description;1.0\n;\nUserCode;Question;Answer;Time\n1900x;question 1;answer;21/10/2022 10:24:08\n1900x;question 2;1;21/10/2022 10:24:08\n";
        Assert.Equal( expctedResult, csvResult.Value );
    }
}

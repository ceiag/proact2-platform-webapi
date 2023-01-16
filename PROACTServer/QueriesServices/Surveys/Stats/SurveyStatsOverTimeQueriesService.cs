using Proact.Services.Models;
using Proact.Services.Models.SurveyStats;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices.Surveys.Stats;

public class SurveyStatsOverTimeQueriesService : ISurveyStatsOverTimeQueriesService {
    private readonly ISurveyAssignationQueriesService _surveyAssignationQueriesService;
    private readonly ISurveyQueriesService _surveyQueriesService;

    public SurveyStatsOverTimeQueriesService(
        ISurveyAssignationQueriesService surveyAssignationQueriesService,
        ISurveyQueriesService surveyQueriesService ) {
        _surveyAssignationQueriesService = surveyAssignationQueriesService;
        _surveyQueriesService = surveyQueriesService;
    }

    public SurveyStatsResumeByTime? Get( Guid surveyId, Guid userId ) {
        var survey = _surveyQueriesService.Get( surveyId );

        var surveyStatsResumeByTime = new SurveyStatsResumeByTime();
        surveyStatsResumeByTime.Title = survey.Title;
        surveyStatsResumeByTime.Description = survey.Description;
        surveyStatsResumeByTime.Version = survey.Version;
        surveyStatsResumeByTime.StartTime = survey.StartTime;
        surveyStatsResumeByTime.ExpireTime = survey.ExpireTime;

        var compiledSurveys = SurveyCompiledEntityMapper
            .Map( _surveyAssignationQueriesService
                .GetCompletedFromPatients( surveyId, userId ) );

        if ( compiledSurveys is null || compiledSurveys.Count == 0 )
            return surveyStatsResumeByTime;

        var firstCompiledSurvey = compiledSurveys[0];

        FillQuestionsContainers( firstCompiledSurvey, surveyStatsResumeByTime );
        FillAnswers( compiledSurveys, surveyStatsResumeByTime );

        return surveyStatsResumeByTime;
    }

    private readonly SurveyQuestionComposerProvider _surveyQuestionComposerProvider
            = new SurveyQuestionComposerProvider();
    private void FillQuestionsContainers(
        SurveyCompiledModel compiledSurvey, SurveyStatsResumeByTime surveyStatsResume ) {
        foreach ( var question in compiledSurvey.Questions ) {
            var statsQuestion = new SurveyStatsQuestion();
            statsQuestion.Question = question.Question;
            statsQuestion.Title = question.Title;
            statsQuestion.Type = question.Type;
            statsQuestion.Properties = question.Properties;
            statsQuestion.Id = question.QuestionId;
            surveyStatsResume.Questions.Add( statsQuestion );
        }
    }

    private void FillAnswers(
         List<SurveyCompiledModel> compiledSurveys,
         SurveyStatsResumeByTime surveyStatsResume ) {
        foreach ( var compiledSurvey in compiledSurveys ) {
            int i = 0;
            foreach ( var question in compiledSurvey.Questions ) {

                var answer = new SurveyStatsAnswer();
                answer.Date = (DateTime)compiledSurvey.CompletedDateTime;

                foreach ( var answerItem in question.CompiledAnswers ) {
                    answer.Answers.Add( answerItem.Value );
                }

                surveyStatsResume.Questions[i].Answers.Add( answer );
                ++i;
            }
        }
    }
}

using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.SurveyStats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices.Surveys.Stats;
public class SurveysStatsQueriesService : ISurveysStatsQueriesService {
    private readonly ISurveyAssignationQueriesService _surveyAssignationQueriesService;
    private readonly ISurveyQueriesService _surveyQueriesService;
    private readonly IPatientQueriesService _patientQueriesService;

    public SurveysStatsQueriesService(
        ISurveyAssignationQueriesService surveyAssignationQueriesService,
        ISurveyQueriesService surveyQueriesService,
        IPatientQueriesService patientQueriesService ) {
        _surveyAssignationQueriesService = surveyAssignationQueriesService;
        _surveyQueriesService = surveyQueriesService;
        _patientQueriesService = patientQueriesService;
    }

    private void SetSurveryResumeHeader(
        SurveyStatsResume statsResume, List<SurveyCompiledModel> compiledSurveys, int participants ) {
        statsResume.Title = compiledSurveys[0].Title;
        statsResume.Description = compiledSurveys[0].Description;
        statsResume.ExpireTime = compiledSurveys[0].ExpireTime;
        statsResume.StartTime = compiledSurveys[0].StartTime;
        statsResume.Version = compiledSurveys[0].Version;
        statsResume.Completed = compiledSurveys.Count;
        statsResume.Participants = participants;
    }

    private QuestionWithAnswers CreateQuestionIfNotExist(
        SurveyStatsResume statsResume, SurveyCompiledQuestionModel compiledQuestion ) {
        var statQuestion = statsResume.QuestionWithAnswers.FirstOrDefault(
            x => x.Title == compiledQuestion.Title && x.Question == compiledQuestion.Question );

        if ( statQuestion == null ) {
            statQuestion = new QuestionWithAnswers() {
                Title = compiledQuestion.Title,
                Question = compiledQuestion.Question,
            };
        }

        statQuestion.SerializedProperties = compiledQuestion.Properties;
        return statQuestion;
    }

    private void InsertOrIncreaseAnswerCountToQuestion(
        QuestionWithAnswers statQuestion, SurveyCompiledQuestionModel compiledQuestion ) {
        foreach ( var answer in compiledQuestion.CompiledAnswers ) {
            var statQuestionAnswer = statQuestion.Answers
                .FirstOrDefault( x => x.Value == answer.Value );

            if ( statQuestionAnswer == null ) {
                statQuestionAnswer = new QuestionAnswer() {
                    Value = answer.Value
                };

                statQuestion.Answers.Add( statQuestionAnswer );
            }

            statQuestionAnswer.Count++;
        }
    }

    private void CreateQuestionsFromSurveyModel(
        SurveyStatsResume statsResume, Guid surveyId ) {
        var originalSurvey = _surveyQueriesService.Get( surveyId );

        foreach ( var question in originalSurvey.Questions ) {
            var statQuestion = new QuestionWithAnswers() {
                Title = question.Question.Title,
                Question = question.Question.Question,
                Type = question.Question.Type
            };

            if ( statQuestion.Type == SurveyQuestionType.BOOLEAN ) {
                statQuestion.Answers.Add( new QuestionAnswer() {
                    Value = "true",
                    Count = 0
                } );

                statQuestion.Answers.Add( new QuestionAnswer() {
                    Value = "false",
                    Count = 0
                } );
            }
            else if ( statQuestion.Type == SurveyQuestionType.RATING ) {
                var props = JsonConvert.DeserializeObject<SurveyMinMaxQuestionProperties>(
                    question.Question.Answers[0].Answer.SerializedProperties );

                for ( int i = props.Min; i <= props.Max; ++i ) {
                    statQuestion.Answers.Add( new QuestionAnswer() {
                        Value = i.ToString(),
                        Count = 0
                    } );
                }

            }
            else if ( statQuestion.Type == SurveyQuestionType.MOOD ) {
                foreach ( var enumMood in Enum.GetValues( typeof( PatientMood ) ) ) {
                    if ( (PatientMood)enumMood != PatientMood.None ) {
                        statQuestion.Answers.Add( new QuestionAnswer() {
                            Value = ( (int)enumMood ).ToString(),
                            Count = 0
                        } );
                    }
                }
            }
            else if ( statQuestion.Type == SurveyQuestionType.SINGLE_ANSWER
                || statQuestion.Type == SurveyQuestionType.MULTIPLE_ANSWERS ) {
                foreach ( var answer in question.Question.Answers ) {
                    statQuestion.Answers.Add( new QuestionAnswer() {
                        Value = answer.Answer.LabelId,
                        Count = 0
                    } );
                }
            }

            statsResume.QuestionWithAnswers.Add( statQuestion );
        }
    }

    public SurveyStatsResume GetStatsResumeForSurvey( Guid surveyId ) {
        var assignations = _surveyAssignationQueriesService.GetFromSurveyId( surveyId );
        var compiledSurveys = SurveyCompiledEntityMapper
            .Map( assignations.Where( x => x.Completed ).ToList() );

        if ( !compiledSurveys.Any() ) {
            return new SurveyStatsResume();
        }

        var statsResume = new SurveyStatsResume();
        SetSurveryResumeHeader( statsResume, compiledSurveys, assignations.Count );
        CreateQuestionsFromSurveyModel( statsResume, surveyId );

        foreach ( var compiledSurvey in compiledSurveys ) {
            foreach ( var question in compiledSurvey.Questions ) {
                var statQuestion = CreateQuestionIfNotExist( statsResume, question );
                InsertOrIncreaseAnswerCountToQuestion( statQuestion, question );
            }
        }

        return statsResume;
    }

    public List<SurveyModel> GetAllSurveysWithAssignedPatients( Guid projectId ) {
        var surveys = _surveyQueriesService.GetsAll( projectId );
        var surveyModels = SurveysEntityMapper.Map( surveys );

        for ( int i = 0; i < surveys.Count; ++i ) {
            var usersGrouped = surveys[i]
                .AssignationRelations
                .Select( x => x.User )
                .GroupBy( x => x.Id )
                .ToList();

            foreach ( var userGroup in usersGrouped ) {
                foreach ( var user in userGroup ) {
                    surveyModels[i].AssignedPatients.Add( 
                        PatientEntityMapper.Map( 
                            _patientQueriesService.Get( user.Id ), false ) );
                    break;
                }
            }
        }

        return surveyModels;
    }
}

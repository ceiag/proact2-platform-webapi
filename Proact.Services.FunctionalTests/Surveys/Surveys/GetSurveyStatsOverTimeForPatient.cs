using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Surveys.Assignations;
using Proact.Services.Models.SurveyStats;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;
using System;

namespace Proact.Services.FunctionalTests.Surveys.Surveys;
public class GetSurveyStatsOverTimeForPatient {
    private User instituteAdmin = null;
    private Institute institute = null;
    private Project project = null;
    private MedicalTeam medicalTeam = null;
    private Medic medic = null;
    private Patient patient = null;
    private SurveyQuestionsSet questionsSet = null;
    private Survey survey = null;
    private List<SurveysAssignationRelation> surveyAssignations_0;
    private List<SurveysAssignationRelation> surveyAssignations_1;
    private SurveyQuestionModel openQuestion = null;
    private SurveyQuestionModel boolQuestion = null;
    private SurveyQuestionModel ratingQuestion = null;
    private SurveyQuestionModel moodQuestion = null;
    private SurveyQuestionModel singleChoiceQuestion = null;
    private SurveyQuestionModel multipleChoiceQuestion = null;
    private SurveyAnswersBlock answersBlock = null;

    [Fact( DisplayName = "Check the correctness of SurveyStatsResumeByTime" )]
    public void _ConsistencyCheck() {
        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddPatientWithRandomValues( medicalTeam, out patient )
            .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
            .AddSurveyWithRandomValues( project, out survey )
            .AddOpenQuestionToQuestionsSet( questionsSet, out openQuestion )
            .AddBooleanQuestionToQuestionsSet( questionsSet, out boolQuestion )
            .AddRatingQuestionToQuestionsSet( questionsSet, out ratingQuestion )
            .AddMoodQuestionToQuestionsSet( questionsSet, out moodQuestion )
            .AddAnswerBlockToQuestionsSet( questionsSet,
                new List<string>() { "choice_0", "choice_1", "choice_2" }, out answersBlock )
            .AddSingleChoiceQuestionToQuestionsSet( questionsSet, answersBlock, out singleChoiceQuestion )
            .AddMultipleChoiceQuestionToQuestionsSet( questionsSet, answersBlock, out multipleChoiceQuestion )
            .AddQuestionsFromQuestionsSet( survey, questionsSet )
            .AddSurveyAssignationsToPatients(
                new List<Patient> { patient }, survey, out surveyAssignations_0 )
            .AddSurveyAssignationsToPatients(
                new List<Patient> { patient }, survey, out surveyAssignations_1 );

        var request_0 = GenerateCompileRequest(
            "open answer",
            "true",
            "4",
            "2",
            answersBlock.Answers[0].Id,
            new List<Guid>() { answersBlock.Answers[0].Id, answersBlock.Answers[2].Id } );

        var request_1 = GenerateCompileRequest(
            "open answer again",
            "false",
            "5",
            "1",
            answersBlock.Answers[1].Id,
            new List<Guid>() { answersBlock.Answers[1].Id, answersBlock.Answers[2].Id } );

        new SurveyAssignationControllerProvider( servicesProvider, patient.User, Roles.Patient )
            .Controller.SetCompiledSurvey( survey.Id, surveyAssignations_0[0].Id, request_0 );
        new SurveyAssignationControllerProvider( servicesProvider, patient.User, Roles.Patient )
            .Controller.SetCompiledSurvey( survey.Id, surveyAssignations_1[0].Id, request_1 );

        var provider = new SurveyControllerProvider( 
            servicesProvider, patient.User, Roles.MedicalProfessional );
        var surveyStatsResult = ( provider.Controller
            .GetSurveyStatsOverTimeForPatient( survey.Id, patient.UserId )
                as OkObjectResult ).Value as SurveyStatsResumeByTime;

        Assert.NotNull( surveyStatsResult );
        Assert.Equal( surveyStatsResult.Title, survey.Title );
        Assert.Equal( surveyStatsResult.Description, survey.Description );
        Assert.Equal( surveyStatsResult.Version, survey.Version );
        Assert.Equal( surveyStatsResult.Questions.Count, survey.Questions.Count );

        Assert.Equal(
            request_0.QuestionsCompiled[0].Answers[0].Value,
            surveyStatsResult.Questions[0].Answers[0].Answers[0] );
        Assert.Equal(
            request_1.QuestionsCompiled[0].Answers[0].Value,
            surveyStatsResult.Questions[0].Answers[1].Answers[0] );

        Assert.Equal(
            request_0.QuestionsCompiled[1].Answers[0].Value,
            surveyStatsResult.Questions[1].Answers[0].Answers[0] );
        Assert.Equal(
            request_1.QuestionsCompiled[1].Answers[0].Value,
            surveyStatsResult.Questions[1].Answers[1].Answers[0] );

        Assert.Equal(
            request_0.QuestionsCompiled[2].Answers[0].Value,
            surveyStatsResult.Questions[2].Answers[0].Answers[0] );
        Assert.Equal(
            request_1.QuestionsCompiled[2].Answers[0].Value,
            surveyStatsResult.Questions[2].Answers[1].Answers[0] );

        Assert.Equal(
            request_0.QuestionsCompiled[3].Answers[0].Value,
            surveyStatsResult.Questions[3].Answers[0].Answers[0] );
        Assert.Equal(
            request_1.QuestionsCompiled[3].Answers[0].Value,
            surveyStatsResult.Questions[3].Answers[1].Answers[0] );

        Assert.Equal(
            answersBlock.Answers[0].LabelId,
            surveyStatsResult.Questions[4].Answers[0].Answers[0] );
        Assert.Equal(
            answersBlock.Answers[1].LabelId,
            surveyStatsResult.Questions[4].Answers[1].Answers[0] );

        Assert.Equal(
            answersBlock.Answers[0].LabelId,
            surveyStatsResult.Questions[5].Answers[0].Answers[0] );
        Assert.Equal(
            answersBlock.Answers[2].LabelId,
            surveyStatsResult.Questions[5].Answers[0].Answers[1] );
        Assert.Equal(
            answersBlock.Answers[1].LabelId,
            surveyStatsResult.Questions[5].Answers[1].Answers[0] );
        Assert.Equal(
            answersBlock.Answers[2].LabelId,
            surveyStatsResult.Questions[5].Answers[1].Answers[1] );
    }

    private SurveyCompileRequest GenerateCompileRequest(
        string openQuestionAnswer, 
        string boolQuestionAnswer, 
        string ratingQuestionAnswer, 
        string moodQuestionAnswer,
        Guid singleChoiceQuestionId, 
        List<Guid> multipleChoiceQuestionIds ) {
        var request = new SurveyCompileRequest() {
            QuestionsCompiled = new List<SurveyQuestionCompileRequest>() {
                    new SurveyQuestionCompileRequest() {
                        QuestionId = openQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = openQuestionAnswer
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = boolQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = boolQuestionAnswer
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = ratingQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = ratingQuestionAnswer
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = moodQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = moodQuestionAnswer
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = singleChoiceQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                AnswerId = singleChoiceQuestionId
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = multipleChoiceQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                AnswerId = multipleChoiceQuestionIds[0]
                            },
                            new SurveyAnswerCompileRequest() {
                                AnswerId = multipleChoiceQuestionIds[1]
                            }
                        }
                    }
                }
        };

        return request;
    }
}

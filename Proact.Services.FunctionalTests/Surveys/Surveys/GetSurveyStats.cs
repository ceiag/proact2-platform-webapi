using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Surveys.Assignations;
using Proact.Services.Models;
using Proact.Services.Models.SurveyStats;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Surveys;
public class GetSurveyStats {
    [Fact]
    public void SetCompiledSurvey_ConsistencyCheck() {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        Medic medic = null;
        Patient patient_0 = null;
        Patient patient_1 = null;
        SurveyQuestionsSet questionsSet = null;
        Survey survey = null;
        List<SurveysAssignationRelation> surveyAssignations;
        SurveyQuestionModel openQuestion = null;
        SurveyQuestionModel boolQuestion = null;
        SurveyQuestionModel ratingQuestion = null;
        SurveyQuestionModel moodQuestion = null;
        SurveyQuestionModel singleChoiceQuestion = null;
        SurveyQuestionModel multipleChoiceQuestion = null;
        SurveyAnswersBlock answersBlock = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddPatientWithRandomValues( medicalTeam, out patient_0 )
            .AddPatientWithRandomValues( medicalTeam, out patient_1 )
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
                new List<Patient> { patient_0, patient_1 }, survey, out surveyAssignations );

        var request = new SurveyCompileRequest() {
            QuestionsCompiled = new List<SurveyQuestionCompileRequest>() {
                    new SurveyQuestionCompileRequest() {
                        QuestionId = openQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = "my answer"
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = boolQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = "true"
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = ratingQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = "3"
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = moodQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                Value = "2"
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = singleChoiceQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                AnswerId = answersBlock.Answers[0].Id
                            }
                        }
                    },
                    new SurveyQuestionCompileRequest() {
                        QuestionId = multipleChoiceQuestion.Id,
                        Answers = new List<SurveyAnswerCompileRequest>() {
                            new SurveyAnswerCompileRequest() {
                                AnswerId = answersBlock.Answers[0].Id
                            },
                            new SurveyAnswerCompileRequest() {
                                AnswerId = answersBlock.Answers[2].Id
                            }
                        }
                    }
                }
        };

        new SurveyAssignationControllerProvider( servicesProvider, patient_0.User, Roles.Patient )
            .Controller.SetCompiledSurvey( survey.Id, surveyAssignations[0].Id, request );

        var provider = new SurveyControllerProvider( servicesProvider, patient_0.User, Roles.MedicalProfessional );
        var apiResult = ( provider.Controller.GetSurveyStats( survey.Id )
            as OkObjectResult ).Value as SurveyStatsResume;

        Assert.NotNull( apiResult );
        Assert.Equal( request.QuestionsCompiled.Count, apiResult.QuestionWithAnswers.Count );
        Assert.Equal( 2, apiResult.Participants );
        Assert.Equal( 1, apiResult.Completed );

        //open answer
        Assert.Equal( SurveyQuestionType.OPEN_ANSWER, apiResult.QuestionWithAnswers[0].Type );
        Assert.Equal( "my answer", apiResult.QuestionWithAnswers[0].Answers[0].Value );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[0].Answers[0].Count );

        //boolean
        Assert.Equal( SurveyQuestionType.BOOLEAN, apiResult.QuestionWithAnswers[1].Type );
        Assert.Equal( 2, apiResult.QuestionWithAnswers[1].Answers.Count );
        Assert.Equal( "true", apiResult.QuestionWithAnswers[1].Answers[0].Value );
        Assert.Equal( "false", apiResult.QuestionWithAnswers[1].Answers[1].Value );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[1].Answers[0].Count );

        //rating
        Assert.Equal( SurveyQuestionType.RATING, apiResult.QuestionWithAnswers[2].Type );
        Assert.Equal( 10, apiResult.QuestionWithAnswers[2].Answers.Count );
        Assert.Equal( "3", apiResult.QuestionWithAnswers[2].Answers[2].Value );

        //mood
        Assert.Equal( SurveyQuestionType.MOOD, apiResult.QuestionWithAnswers[3].Type );
        Assert.Equal( Enum.GetValues( typeof( PatientMood ) ).Length - 1,
            apiResult.QuestionWithAnswers[3].Answers.Count );
        Assert.Equal( "2", apiResult.QuestionWithAnswers[3].Answers[2].Value );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[3].Answers[2].Count );

        //singleChoice
        Assert.Equal( SurveyQuestionType.SINGLE_ANSWER, apiResult.QuestionWithAnswers[4].Type );
        Assert.Equal( 3, apiResult.QuestionWithAnswers[4].Answers.Count );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[4].Answers[0].Count );
        Assert.Equal( 0, apiResult.QuestionWithAnswers[4].Answers[1].Count );
        Assert.Equal( 0, apiResult.QuestionWithAnswers[4].Answers[2].Count );

        //multipleChoice
        Assert.Equal( SurveyQuestionType.MULTIPLE_ANSWERS, apiResult.QuestionWithAnswers[5].Type );
        Assert.Equal( 3, apiResult.QuestionWithAnswers[5].Answers.Count );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[5].Answers[0].Count );
        Assert.Equal( 0, apiResult.QuestionWithAnswers[5].Answers[1].Count );
        Assert.Equal( 1, apiResult.QuestionWithAnswers[5].Answers[2].Count );
    }
}

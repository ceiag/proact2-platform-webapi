using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Assignations {
    public class SetCompiledSurvey {
        [Fact]
        public void SetCompiledSurvey_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            Patient patient = null;
            SurveyQuestionsSet questionsSet = null;
            Survey survey = null;
            List<SurveysAssignationRelation> surveyAssignations;
            SurveyQuestionModel openQuestion = null;
            SurveyQuestionModel boolQuestion = null;

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
                .AddQuestionsFromQuestionsSet( survey, questionsSet )
                .AddSurveyAssignationsToPatient( patient, survey, out surveyAssignations );

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
                    }
                }
            };

            var provider = new SurveyAssignationControllerProvider(
                servicesProvider, patient.User, Roles.Patient );
            var apiResult = provider.Controller
                .SetCompiledSurvey( survey.Id, surveyAssignations[0].Id, request );

            var surveyResult = provider.GetCompiledSurvey( surveyAssignations[0].Id );

            Assert.Equal( 200, ( apiResult as OkResult ).StatusCode );
            Assert.Equal( request.QuestionsCompiled[0].Answers[0].Value, 
                surveyResult.Questions[0].CompiledAnswers[0].Value );
            Assert.Equal( 1, surveyResult.Questions[0].Order );
            Assert.Equal( SurveyQuestionType.OPEN_ANSWER, surveyResult.Questions[0].Type );

            Assert.Equal( request.QuestionsCompiled[1].Answers[0].Value,
                surveyResult.Questions[1].CompiledAnswers[0].Value );
            Assert.Equal( 2, surveyResult.Questions[1].Order );
            Assert.Equal( SurveyQuestionType.BOOLEAN, surveyResult.Questions[1].Type );
        }
    }
}

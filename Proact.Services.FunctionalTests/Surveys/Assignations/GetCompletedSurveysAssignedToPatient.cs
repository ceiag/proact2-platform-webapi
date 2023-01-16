using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Assignations;
public class GetCompletedSurveysAssignedToPatient {
    private void CreateDatabaseSnapshot(
        ProactServicesProvider servicesProvider, 
        out Medic medic, out Nurse nurse, out Researcher researcher, out Patient patient ) {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        SurveyQuestionsSet questionsSet = null;
        Survey survey = null;
        List<SurveysAssignationRelation> surveyAssignations;
        SurveyQuestionModel openQuestion = null;
        SurveyQuestionModel boolQuestion = null;

        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddNurseWithRandomValues( medicalTeam, out nurse )
            .AddNurseToMedicalTeam( medicalTeam, nurse )
            .AddResearcherWithRandomValues( medicalTeam, out researcher )
            .AddResearcherToMedicalTeam( medicalTeam, researcher )
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
    }

    [Fact]
    public void _ConsistencyCheck_AsMedic() {
        Medic medic = null;
        Nurse nurse = null;
        Researcher researcher = null;
        Patient patient = null;

        var servicesProvider = new ProactServicesProvider();
        CreateDatabaseSnapshot( servicesProvider, out medic, out nurse, out researcher, out patient );

        var provider = new SurveyAssignationControllerProvider(
            servicesProvider, medic.User, Roles.MedicalProfessional );

        var assignationsCompiled = ( provider.Controller
            .GetCompletedSurveysAssignedToPatient( patient.UserId ) as OkObjectResult )
            .Value as List<SurveyAssignationModel>;

        Assert.NotNull( assignationsCompiled );
    }

    [Fact]
    public void _ConsistencyCheck_AsNurse() {
        Medic medic = null;
        Nurse nurse = null;
        Researcher researcher = null;
        Patient patient = null;

        var servicesProvider = new ProactServicesProvider();
        CreateDatabaseSnapshot( servicesProvider, out medic, out nurse, out researcher, out patient );

        var provider = new SurveyAssignationControllerProvider(
            servicesProvider, nurse.User, Roles.Nurse );

        var assignationsCompiled = ( provider.Controller
            .GetCompletedSurveysAssignedToPatient( patient.UserId ) as OkObjectResult )
            .Value as List<SurveyAssignationModel>;

        Assert.NotNull( assignationsCompiled );
    }

    [Fact]
    public void _ConsistencyCheck_AsResearcher() {
        Medic medic = null;
        Nurse nurse = null;
        Researcher researcher = null;
        Patient patient = null;

        var servicesProvider = new ProactServicesProvider();
        CreateDatabaseSnapshot( servicesProvider, out medic, out nurse, out researcher, out patient );

        var provider = new SurveyAssignationControllerProvider(
            servicesProvider, researcher.User, Roles.Researcher );

        var assignationsCompiled = ( provider.Controller
            .GetCompletedSurveysAssignedToPatient( patient.UserId ) as OkObjectResult )
            .Value as List<SurveyAssignationModel>;

        Assert.NotNull( assignationsCompiled );
    }
}

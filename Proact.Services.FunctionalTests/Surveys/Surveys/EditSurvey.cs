using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Surveys {
    public class EditSurvey {
        [Fact]
        public void _ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            SurveyQuestionsSet questionsSet = null;
            SurveyQuestionModel openQuestion = null;
            
            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
                .AddOpenQuestionToQuestionsSet( questionsSet, out openQuestion );

            var creationRequest = new SurveyCreationRequest() {
                Title = "my questions set",
                Description = "a description",
                Version = "1.0",
                QuestionsSetId = questionsSet.Id,
                QuestionsIds = new List<Guid>() { openQuestion.Id }
            };

            var provider = new SurveyControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var creationResult = provider.Controller.CreateSurvey( project.Id, creationRequest );
            var surveyCreated = ( creationResult as OkObjectResult ).Value as SurveyModel;

            var editRequest = new SurveyEditRequest() {
                Title = "my questions set!",
                Description = "a description!",
                Version = "1.1",
                QuestionsSetId = questionsSet.Id,
                QuestionsIds = new List<Guid>() { openQuestion.Id }
            };

            var result = provider.Controller.EditSurvey( surveyCreated.Id, editRequest );
            Assert.Equal( 200,( result as OkResult).StatusCode );

            Assert.NotNull( questionsSet );
            var surveyRetrieved = provider.Get( surveyCreated.Id );

            Assert.Equal( editRequest.Title, surveyRetrieved.Title );
            Assert.Equal( editRequest.Description, surveyRetrieved.Description );
            Assert.Equal( editRequest.Version, surveyRetrieved.Version );
        }
    }
}

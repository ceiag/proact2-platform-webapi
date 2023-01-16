using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers.MessageAnalysis;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.Services.UnitTests.MessageAnalysis;
using Proact.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.IntegrationTests.MessageAnalysis.Controller {
    public class MessageAnalysisControllerIntegrationTests {
        private Project _project;
        private ProjectProperties _projectProperties;
        private MedicalTeam _medicalTeam;
        private User _userPatient;
        private User _userMedic_0;
        private User _userMedic_1;
        private Patient _patient;
        private Medic _medic_0;
        private Medic _medic_1;
        private MessageModel _message;
        private AnalysisResumeModel _scenarioAnalysisResume;

        private MessageAnalysisController CreateMessageAnalysisController(
            MockDatabaseUnitTestHelper mockHelper, string userRole, User user ) {
            var mockChangeTrackingService = new ChangesTrackingService(
                mockHelper.ServicesProvider.Database,
                mockHelper.ServicesProvider.GetQueriesService<IUserQueriesService>() );

            var messageAnalysisController = new MessageAnalysisController(
                mockHelper.ServicesProvider.GetQueriesService<IMessageAnalysisQueriesService>(),
                mockChangeTrackingService,
                mockHelper.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( messageAnalysisController, user, userRole );

            return messageAnalysisController;
        }

        private void PrepareTestScenarioWithTwoAnalysis( 
            MockDatabaseUnitTestHelper mockHelper, bool medicCanSeeOtherMedicsAnalysis ) {
            _project = mockHelper.CreateDummyProject();
            _medicalTeam = mockHelper.CreateDummyMedicalTeam( _project );
            _projectProperties = mockHelper.CreateDummyProjectProperties( 
            _project, medicCanSeeOtherMedicsAnalysis, 0, 0, 0 );
            _userPatient = mockHelper.CreateDummyUser();
            _userMedic_0 = mockHelper.CreateDummyUser();
            _userMedic_1 = mockHelper.CreateDummyUser();
            _patient = mockHelper.CreateDummyPatient( _userPatient );
            _medic_0 = mockHelper.CreateDummyMedic( _userMedic_0 );
            _medic_1 = mockHelper.CreateDummyMedic( _userMedic_1 );
            _message = mockHelper.CreateDummyNewTopicMessage( _userPatient, _medicalTeam );

            mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>()
                .AddToMedicalTeam( _userMedic_0.Id, _medicalTeam.Id );
            mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>()
                .AddToMedicalTeam( _userMedic_1.Id, _medicalTeam.Id );

            mockHelper.ServicesProvider.SaveChanges();

            var analysisCreationRequest = LexiconCreatorHelper
                .GetDummyAnalysisCreationRequest( mockHelper, _message );

            var analysisControllerForMedic_0 = CreateMessageAnalysisController(
                mockHelper, Roles.MedicalProfessional, _userMedic_0 );
            var analysisControllerForMedic_1 = CreateMessageAnalysisController(
                mockHelper, Roles.MedicalProfessional, _userMedic_1 );

            var resultOfAddAnalysis_0 = analysisControllerForMedic_0.AddAnalysisToMessage(
                _project.Id, _medicalTeam.Id, analysisCreationRequest ) as OkObjectResult;
            var resultAnalysisModel_0 = resultOfAddAnalysis_0.Value as AnalysisModel;

            Assert.Equal( 200, resultOfAddAnalysis_0.StatusCode );

            var resultOfAddAnalysis_1 = analysisControllerForMedic_1.AddAnalysisToMessage(
                _project.Id, _medicalTeam.Id, analysisCreationRequest ) as OkObjectResult;
            var resultAnalysisModel_1 = resultOfAddAnalysis_0.Value as AnalysisModel;

            Assert.Equal( 200, resultOfAddAnalysis_1.StatusCode );

            mockHelper.ServicesProvider.SaveChanges();

            var resultOfGetMessage = analysisControllerForMedic_0.GetMessageAnalysisResume(
                    _project.Id, _medicalTeam.Id, _message.MessageId ) as OkObjectResult;
            _scenarioAnalysisResume = resultOfGetMessage.Value as AnalysisResumeModel;
        }

        [Fact]
        public void CreateMessageAnalysis_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                PrepareTestScenarioWithTwoAnalysis( mockHelper, true );

                var analysisController = CreateMessageAnalysisController(
                    mockHelper, Roles.MedicalProfessional, _userMedic_0 );

                var resultOfGetMessage = analysisController.GetMessageAnalysisResume(
                    _project.Id, _medicalTeam.Id, _message.MessageId ) as OkObjectResult;
                var resultAnalysisResumeModel = resultOfGetMessage.Value as AnalysisResumeModel;

                Assert.NotNull( resultAnalysisResumeModel );
                Assert.Equal( 2, resultAnalysisResumeModel.AnalysisCount );
                Assert.Equal( 4, resultAnalysisResumeModel.Analysis[0].Results.Count );
                Assert.Equal( 4, resultAnalysisResumeModel.Analysis[1].Results.Count );
                Assert.Equal( _userMedic_0.Name, resultAnalysisResumeModel.LastUpdateBy.Name );
                Assert.Equal( resultAnalysisResumeModel.Analysis[0].Results[0].CategoryName, 
                    resultAnalysisResumeModel.Analysis[0].ResultsGroupByCategories[0].CategoryName );
                Assert.Equal( resultAnalysisResumeModel.Analysis[0].Results[1].CategoryName,
                    resultAnalysisResumeModel.Analysis[0].ResultsGroupByCategories[1].CategoryName );
                Assert.Equal( resultAnalysisResumeModel.Analysis[0].Results[2].CategoryName,
                    resultAnalysisResumeModel.Analysis[0].ResultsGroupByCategories[2].CategoryName );
                Assert.Single( resultAnalysisResumeModel.Analysis[0]
                    .ResultsGroupByCategories[0].Results );
                Assert.Single( resultAnalysisResumeModel.Analysis[0]
                    .ResultsGroupByCategories[1].Results );
                Assert.Equal( 2, resultAnalysisResumeModel.Analysis[0]
                    .ResultsGroupByCategories[2].Results.Count );
                Assert.False( resultAnalysisResumeModel.Analysis[0].IsMine );
                Assert.True( resultAnalysisResumeModel.Analysis[1].IsMine );
                Assert.True( resultAnalysisResumeModel.Analysis[0]
                    .ResultsGroupByCategories[0].Order <
                        resultAnalysisResumeModel.Analysis[0]
                            .ResultsGroupByCategories[1].Order );
                Assert.True( resultAnalysisResumeModel.Analysis[0]
                    .ResultsGroupByCategories[0].Order <
                        resultAnalysisResumeModel.Analysis[1]
                            .ResultsGroupByCategories[2].Order );
            }
        }

        [Fact]
        public void CreateMessageAnalysis_GetOnlyMine_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                PrepareTestScenarioWithTwoAnalysis( mockHelper, false );
                
                var analysisController = CreateMessageAnalysisController(
                    mockHelper, Roles.MedicalProfessional, _userMedic_0 );

                var resultOfGetMessage = analysisController.GetMessageAnalysisResume(
                    _project.Id, _medicalTeam.Id, _message.MessageId ) as OkObjectResult;
                var resultAnalysisResumeModel = resultOfGetMessage.Value as AnalysisResumeModel;

                Assert.NotNull( resultAnalysisResumeModel );
                Assert.Equal( 2, resultAnalysisResumeModel.AnalysisCount );
                Assert.False( resultAnalysisResumeModel.Analysis[0].ResultsVisible );
                Assert.True( resultAnalysisResumeModel.Analysis[1].ResultsVisible );
                Assert.NotEmpty( resultAnalysisResumeModel.Analysis[1].Results );
                Assert.Empty( resultAnalysisResumeModel.Analysis[0].Results );
                Assert.True( resultAnalysisResumeModel.Analysis[0].CreationDate.Ticks >
                    resultAnalysisResumeModel.Analysis[1].CreationDate.Ticks );
            }
        }

        [Fact]
        public void DeleteMessageAnalysis_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                PrepareTestScenarioWithTwoAnalysis( mockHelper, true );

                var analysisController = CreateMessageAnalysisController(
                    mockHelper, Roles.MedicalProfessional, _userMedic_0 );

                var resultOfDeleteAnalysis = analysisController.DeleteAnalysisFromMessage(
                    _project.Id, _medicalTeam.Id, _scenarioAnalysisResume.Analysis[1].AnalysisId ) as OkResult;

                Assert.NotNull( resultOfDeleteAnalysis );
                Assert.Equal( 200, resultOfDeleteAnalysis.StatusCode );

                mockHelper.ServicesProvider.SaveChanges();

                var resultMessageAfterAnalysisDeleted = analysisController.GetMessageAnalysisResume(
                    _project.Id, _medicalTeam.Id, _message.MessageId ) as OkObjectResult;
                var resultMessageAfterAnalysisDeletedModel
                    = resultMessageAfterAnalysisDeleted.Value as AnalysisResumeModel;

                Assert.NotNull( resultMessageAfterAnalysisDeletedModel );
                Assert.Equal( 1, resultMessageAfterAnalysisDeletedModel.AnalysisCount );
            }
        }

        [Fact]
        public void UpdateMessageAnalysis_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                PrepareTestScenarioWithTwoAnalysis( mockHelper, true );

                var analysisController = CreateMessageAnalysisController(
                    mockHelper, Roles.MedicalProfessional, _userMedic_0 );

                var lexicon = LexiconCreatorHelper.CreateDummyLexicon( mockHelper );

                var analysisUpdateRequest = new AnalysisCreationRequest() {
                    MessageId = _message.MessageId,
                    AnalysisResults = new List<AnalysisResultCreationRequest>() {
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[0].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[1].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[1].Id,
                        },
                        new AnalysisResultCreationRequest() {
                            LabelId = lexicon.Categories[2].Labels[2].Id,
                        }
                    }
                };

                var resultOfUpdatedAnalysis = analysisController.UpdateAnalysisFromMessage(
                    _project.Id, _medicalTeam.Id, 
                    _scenarioAnalysisResume.Analysis[1].AnalysisId,
                    analysisUpdateRequest ) as OkResult;

                mockHelper.ServicesProvider.SaveChanges();

                var resultMessageAfterAnalysisUpdate = analysisController.GetMessageAnalysisResume(
                    _project.Id, _medicalTeam.Id, _message.MessageId ) as OkObjectResult;
                var resultMessageAfterAnalysisUpdatedModel
                    = resultMessageAfterAnalysisUpdate.Value as AnalysisResumeModel;

                Assert.NotNull( resultMessageAfterAnalysisUpdatedModel );
                Assert.Equal( 2, resultMessageAfterAnalysisUpdatedModel.AnalysisCount );
                Assert.Equal( analysisUpdateRequest.AnalysisResults[0].LabelId,
                    resultMessageAfterAnalysisUpdatedModel.Analysis[0].Results[0].LabelId );
                Assert.Equal( analysisUpdateRequest.AnalysisResults[1].LabelId,
                    resultMessageAfterAnalysisUpdatedModel.Analysis[0].Results[1].LabelId );
                Assert.Equal( analysisUpdateRequest.AnalysisResults[2].LabelId,
                    resultMessageAfterAnalysisUpdatedModel.Analysis[0].Results[2].LabelId );
                Assert.Equal( analysisUpdateRequest.AnalysisResults[3].LabelId,
                    resultMessageAfterAnalysisUpdatedModel.Analysis[0].Results[3].LabelId );
            }
        }
    }
}

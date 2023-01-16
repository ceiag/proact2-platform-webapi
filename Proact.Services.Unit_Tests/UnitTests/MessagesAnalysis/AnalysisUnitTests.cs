using Microsoft.Extensions.Configuration;
using Moq;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.MessageAnalysis {
    public class AnalysisUnitTests {
        [Fact]
        public void AnalysisCreationConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user_patient = mockHelper.CreateDummyUser();
                var user_medic = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var message = mockHelper.CreateDummyNewTopicMessage( user_patient, medicalTeam );

                var analysisCreationRequest = LexiconCreatorHelper
                    .GetDummyAnalysisCreationRequest( mockHelper, message );

                var analysisCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMessageAnalysisQueriesService>()
                    .Create( medic.UserId, analysisCreationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var analysisRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<IMessageAnalysisQueriesService>()
                    .Get( analysisCreated.Id );

                Assert.NotNull( analysisRetrieved );
                Assert.Equal( 4, analysisRetrieved.AnalysisResults.Count );
                Assert.Equal( analysisCreationRequest.AnalysisResults[0].LabelId,
                    analysisRetrieved.AnalysisResults[0].LexiconLabelId );
                Assert.Equal( analysisCreationRequest.AnalysisResults[1].LabelId,
                    analysisRetrieved.AnalysisResults[1].LexiconLabelId );
                Assert.Equal( analysisCreationRequest.AnalysisResults[2].LabelId,
                    analysisRetrieved.AnalysisResults[2].LexiconLabelId );
            }
        }

        [Fact]
        public void AnalysisModelCreationConsinstecyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user_patient = mockHelper.CreateDummyUser();
                var user_medic = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var message = mockHelper.CreateDummyNewTopicMessage( user_patient, medicalTeam );

                var analysisCreationRequest = LexiconCreatorHelper
                    .GetDummyAnalysisCreationRequest( mockHelper, message );

                var analysisCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMessageAnalysisQueriesService>()
                    .Create( medic.UserId, analysisCreationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var analysisRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<IMessageAnalysisQueriesService>()
                    .Get( analysisCreated.Id );

                var analysisRetrievedModel = AnalysisEntityMapper.Map( analysisRetrieved, user_patient.Id );

                Assert.NotNull( analysisRetrievedModel );
                Assert.Equal( 4, analysisRetrievedModel.Results.Count );

                for ( int i = 0; i < analysisRetrievedModel.Results.Count; ++i ) {
                    Assert.Equal( analysisRetrieved.AnalysisResults[i].LexiconLabel.Label,
                        analysisRetrievedModel.Results[i].ResultLabel );
                    Assert.Equal( analysisRetrieved.AnalysisResults[i].LexiconLabel.LexiconCategory.Name,
                        analysisRetrievedModel.Results[i].CategoryName );
                }
            }
        }
    }
}

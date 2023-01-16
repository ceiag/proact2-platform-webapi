using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.SurveysAssignments {
    public class SurveyAssignementToUserUnitTests {
        [Fact]
        public void AssignSurveyToPatientCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var assignSurveyToPatientRequest = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey.Id,
                    UserIds = new List<Guid>() { user.Id },
                    StartTime = DateTime.UtcNow.AddDays( 1 ),
                    ExpireTime = DateTime.UtcNow.AddYears( 1 )
                };

                var surveyAssigned = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest );

                mockHelper.ServicesProvider.SaveChanges();

                Assert.NotNull( surveyAssigned );
                Assert.Equal( survey.Id, surveyAssigned[0].SurveyId );
                Assert.Equal( user.Id, surveyAssigned[0].UserId );
                Assert.Equal( assignSurveyToPatientRequest.StartTime.Day, surveyAssigned[0].StartTime.Day );
                Assert.Equal( assignSurveyToPatientRequest.StartTime.Month, surveyAssigned[0].StartTime.Month );
                Assert.Equal( assignSurveyToPatientRequest.StartTime.Year, surveyAssigned[0].StartTime.Year );
                Assert.Equal( assignSurveyToPatientRequest.ExpireTime.Day, surveyAssigned[0].ExpireTime.Day );
                Assert.Equal( assignSurveyToPatientRequest.ExpireTime.Month, surveyAssigned[0].ExpireTime.Month );
                Assert.Equal( assignSurveyToPatientRequest.ExpireTime.Year, surveyAssigned[0].ExpireTime.Year );
            }
        }

        [Fact]
        public void GetAssignedSurveysCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var assignSurveyToPatientRequest_0 = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey_0.Id,
                    UserIds = new List<Guid>() { user.Id },
                    StartTime = DateTime.UtcNow.AddDays( 1 ),
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                };

                var assignSurveyToPatientRequest_1 = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey_1.Id,
                    UserIds = new List<Guid>() { user.Id },
                    StartTime = DateTime.UtcNow.AddDays( 1 ),
                    ExpireTime = DateTime.UtcNow.AddYears( 1 )
                };

                var surveyAssigned_0 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest_0 );

                var surveyAssigned_1 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest_1 );

                mockHelper.ServicesProvider.SaveChanges();

                var assignedSurveys = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .GetByUserId( user.Id );

                Assert.Equal( 2, assignedSurveys.Count );
                Assert.Equal( survey_0.Id, surveyAssigned_0[0].SurveyId );
                Assert.Equal( user.Id, surveyAssigned_0[0].UserId );
                Assert.Equal( survey_1.Id, surveyAssigned_1[0].SurveyId );
                Assert.Equal( user.Id, surveyAssigned_1[0].UserId );
            }
        }

        [Fact]
        public void GetAvailableSurveysForUserCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var assignSurveyToPatientRequest_0 = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey_0.Id,
                    UserIds = new List<Guid>() { user.Id },
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                };

                var assignSurveyToPatientRequest_1 = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey_1.Id,
                    UserIds = new List<Guid>() { user.Id },
                    StartTime = DateTime.UtcNow.AddDays( 1 ),
                    ExpireTime = DateTime.UtcNow.AddYears( 1 )
                };

                var surveyAssigned_0 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest_0 );

                var surveyAssigned_1 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest_1 );

                mockHelper.ServicesProvider.SaveChanges();

                var assignedSurveys = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .GetAvailableSurveysForUser( user.Id );

                Assert.Single( assignedSurveys );
                Assert.Equal( survey_0.Id, surveyAssigned_0[0].SurveyId );
                Assert.Equal( user.Id, surveyAssigned_0[0].UserId );
                Assert.Equal( survey_1.Id, surveyAssigned_1[0].SurveyId );
                Assert.Equal( user.Id, surveyAssigned_1[0].UserId );
            }
        }

        [Fact]
        public void GetPatientsAssignedToSurveyCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var patient_0 = mockHelper.CreateDummyPatient( user_0 );
                var user_1 = mockHelper.CreateDummyUser();
                var patient_1 = mockHelper.CreateDummyPatient( user_1 );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                
                mockHelper.ServicesProvider.SaveChanges();

                var assignSurveyToPatientsRequest = new AssignSurveyToPatientsRequest() {
                    SurveyId = survey.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id },
                    StartTime = DateTime.UtcNow.AddDays( 1 ),
                    ExpireTime = DateTime.UtcNow.AddYears( 1 )
                };

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientsRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var patientsAssignedToSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .GetFromSurveyId( survey.Id );

                Assert.Equal( 2, patientsAssignedToSurvey.Count );
            }
        }
    }  
}

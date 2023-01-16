using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Surveys {
    public class DbSurveysValidityCheckerUnitTests {
        [Fact]
        public void IfSurveyQuestionsSetIsValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                SurveyQuestionsSet questionsSetRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsValid( questionsSet.Id, out questionsSetRetrieved )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( questionsSetRetrieved );
            }
        }

        [Fact]
        public void IfSurveyQuestionsSetIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                SurveyQuestionsSet questionsSetRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsValid( Guid.NewGuid(), out questionsSetRetrieved )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( questionsSetRetrieved );
            }
        }

        [Fact]
        public void IfSurveyQuestionsSetIsPublishedMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .SetState( questionsSet.Id, QuestionsSetsState.PUBLISHED );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsPublished( questionsSet.Id )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionsSetIsPublishedMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsPublished( questionsSet.Id )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionsSetIsEditableMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsEditable( questionsSet )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionsSetIsEditableMustReturnBadRequest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .SetState( questionsSet.Id, QuestionsSetsState.PUBLISHED );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionsSetIsEditable( questionsSet )
                    .Then( () => {
                        return new OkObjectResult( questionsSet );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionIsValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var surveyQuestion = SurveyCreatorHelper.CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );
                
                mockHelper.ServicesProvider.SaveChanges();

                SurveyQuestion surveyQuestionRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionIsValid( surveyQuestion.Id, out surveyQuestionRetrieved )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( surveyQuestionRetrieved );
            }
        }

        [Fact]
        public void IfSurveyQuestionIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var surveyQuestion = SurveyCreatorHelper.CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                SurveyQuestion surveyQuestionRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionIsValid( Guid.NewGuid(), out surveyQuestionRetrieved )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( surveyQuestionRetrieved );
            }
        }

        [Fact]
        public void IfSurveyIsValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                
                Survey surveyRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsValid( survey.Id, out surveyRetrieved )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( surveyRetrieved );
            }
        }

        [Fact]
        public void IfSurveyIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                Survey surveyRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsValid( Guid.NewGuid(), out surveyRetrieved )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( surveyRetrieved );
            }
        }

        [Fact]
        public void IfSurveyIsEditableMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsEditable( survey.Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyIsEditableMustReturnBadRequest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.GetQueriesService<ISurveyQueriesService>()
                    .SetState( survey.Id, SurveyState.PUBLISHED );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsEditable( survey.Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionNotAlreadyAddedMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var question_0 = SurveyCreatorHelper.CreateDummyBoolQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyMoodQuestion( mockHelper, questionsSet );

                SurveyCreatorHelper.AddQuestionToSurvey(
                    mockHelper, survey.Id, new List<Guid>() { question_0.Id } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionNotAlreadyAdded( survey, question_1.Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyQuestionNotAlreadyAddedMustReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var question_0 = SurveyCreatorHelper.CreateDummyBoolQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyMoodQuestion( mockHelper, questionsSet );

                SurveyCreatorHelper.AddQuestionToSurvey(
                    mockHelper, survey.Id, new List<Guid>() { question_0.Id } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyQuestionNotAlreadyAdded( survey, question_0.Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfSurveyAssignationIsValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var assignationRequest = new AssignSurveyToPatientsRequest() {
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                    SurveyId = survey.Id,
                    UserIds = new List<Guid> { user.Id }
                };

                var assignationResult = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                SurveysAssignationRelation assignmentRelation = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyAssignationIsValid( assignationResult[0].Id, out assignmentRelation )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyAssignationIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var assignationRequest = new AssignSurveyToPatientsRequest() {
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                    SurveyId = survey.Id,
                    UserIds = new List<Guid> { user.Id }
                };

                var assignationResult = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                SurveysAssignationRelation assignmentRelation = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyAssignationIsValid( Guid.NewGuid(), out assignmentRelation )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
            }
        }

        [Fact]
        public void IfSurveyIsAssignateToUserMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var assignationRequest = new AssignSurveyToPatientsRequest() {
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                    SurveyId = survey.Id,
                    UserIds = new List<Guid> { user.Id }
                };

                var assignationResult = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsAssignateToUser( assignationResult[0].Id, user.Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfSurveyIsAssignateToUserMustReturnBadRequest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var assignationRequest = new AssignSurveyToPatientsRequest() {
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                    SurveyId = survey.Id,
                    UserIds = new List<Guid> { user.Id }
                };

                var assignationResult = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfSurveyIsAssignateToUser( Guid.NewGuid(), assignationResult[0].Id )
                    .Then( () => {
                        return new OkObjectResult( survey );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }
    }
}

using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests.MessageAnalysis;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class ProjectPropertiesCreationUnitTests {
        private ProjectPropertiesCreateRequest _projectPropsCreateRequest
            = new ProjectPropertiesCreateRequest() {
                MedicsCanSeeOtherAnalisys = true,
                MessageCanBeAnalizedAfterMinutes = 10,
                MessageCanNotBeDeletedAfterMinutes = 15,
                MessageCanBeRepliedAfterMinutes = 8,
                IsSurveysSystemActive = true,
                IsAnalystConsoleActive = true,
            };

        private ProjectPropertiesUpdateRequest _projectPropsUpdateRequest
            = new ProjectPropertiesUpdateRequest() {
                MedicsCanSeeOtherAnalisys = false,
                MessageCanBeAnalizedAfterMinutes = 18,
                MessageCanNotBeDeletedAfterMinutes = 10,
                MessageCanBeRepliedAfterMinutes = 9,
                IsAnalystConsoleActive = false,
                IsSurveysSystemActive = false,
            };

        private Project CreateProjectProperties( MockDatabaseUnitTestHelper mockHelper ) {
            var project = mockHelper.CreateDummyProject();
            var lexicon = LexiconCreatorHelper.CreateDummyLexicon( mockHelper );

            mockHelper.ServicesProvider
                .GetQueriesService<IProjectPropertiesQueriesService>()
                .Create( project.Id, _projectPropsCreateRequest );

            mockHelper.ServicesProvider.SaveChanges();

            return project;
        }

        [Fact]
        public void CreateProjectProperties_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = CreateProjectProperties( mockHelper );

                var projectPropsRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectPropertiesQueriesService>()
                    .GetByProjectId( project.Id );

                Assert.NotNull( projectPropsRetrieved );
                Assert.Equal( _projectPropsCreateRequest.MedicsCanSeeOtherAnalisys, 
                    projectPropsRetrieved.MedicsCanSeeOtherAnalisys );
                Assert.Equal( _projectPropsCreateRequest.MessageCanBeAnalizedAfterMinutes, 
                    projectPropsRetrieved.MessageCanBeAnalizedAfterMinutes );
                Assert.Equal( _projectPropsCreateRequest.MessageCanNotBeDeletedAfterMinutes, 
                    projectPropsRetrieved.MessageCanNotBeDeletedAfterMinutes );
                Assert.Equal( _projectPropsCreateRequest.MessageCanBeRepliedAfterMinutes,
                   projectPropsRetrieved.MessageCanBeRepliedAfterMinutes );
                Assert.Equal( _projectPropsCreateRequest.MessageCanBeRepliedAfterMinutes,
                   projectPropsRetrieved.MessageCanBeRepliedAfterMinutes );
                Assert.Equal( _projectPropsCreateRequest.IsAnalystConsoleActive,
                   projectPropsRetrieved.IsAnalystConsoleActive );
                Assert.Equal( _projectPropsCreateRequest.IsSurveysSystemActive,
                   projectPropsRetrieved.IsSurveysSystemActive );
            }
        }

        [Fact]
        public void UpdateProjectProperties_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = CreateProjectProperties( mockHelper );

                mockHelper.ServicesProvider
                    .GetQueriesService<IProjectPropertiesQueriesService>()
                    .Update( project.Id, _projectPropsUpdateRequest );

                var updatedProjectPropsRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectPropertiesQueriesService>()
                    .GetByProjectId( project.Id );

                Assert.NotNull( updatedProjectPropsRetrieved );
                Assert.Equal( _projectPropsUpdateRequest.MedicsCanSeeOtherAnalisys,
                    updatedProjectPropsRetrieved.MedicsCanSeeOtherAnalisys );
                Assert.Equal( _projectPropsUpdateRequest.MessageCanBeAnalizedAfterMinutes,
                    updatedProjectPropsRetrieved.MessageCanBeAnalizedAfterMinutes );
                Assert.Equal( _projectPropsUpdateRequest.MessageCanNotBeDeletedAfterMinutes,
                    updatedProjectPropsRetrieved.MessageCanNotBeDeletedAfterMinutes );
                Assert.Equal( _projectPropsUpdateRequest.MessageCanBeRepliedAfterMinutes,
                   updatedProjectPropsRetrieved.MessageCanBeRepliedAfterMinutes );
                Assert.Equal( _projectPropsUpdateRequest.IsAnalystConsoleActive,
                   updatedProjectPropsRetrieved.IsAnalystConsoleActive );
                Assert.Equal( _projectPropsUpdateRequest.IsSurveysSystemActive,
                   updatedProjectPropsRetrieved.IsSurveysSystemActive );
            }
        }
    }
}

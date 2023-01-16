using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Linq;

namespace Proact.Services {
    public static class DbProjectsValidityChecker  {
        public static ConsistencyRulesHelper IfProjectIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid projectId, out Project project ) {

            Project projectResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    projectResult = rulesHelper.GetQueriesService<IProjectQueriesService>().Get( projectId );

                    return projectResult != null;
                },
                () => {
                    return new OkObjectResult( projectResult );
                },
                () => {
                    return new NotFoundObjectResult( $"project with id {projectId} not found" );
                } );

            project = projectResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectIsOpen(
            this ConsistencyRulesHelper rulesHelper, Guid projectId ) {

            Project projectResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IProjectQueriesService>().IsOpened( projectId );
                },
                () => {
                    return new OkObjectResult( projectResult );
                },
                () => {
                    return new NotFoundObjectResult( 
                        string.Format( "The Project {0} is closed.", projectId ) );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectNameAvailable(
            this ConsistencyRulesHelper rulesHelper, string name ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IProjectQueriesService>().IsProjectNameAvailable( name );
                },
                () => {
                    return new OkObjectResult( name );
                },
                () => {
                    return new ConflictObjectResult( $"this name is already taken" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectNameAvailableForExistingProject(
            this ConsistencyRulesHelper rulesHelper, string name, Guid projectId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var project = rulesHelper
                        .GetQueriesService<IProjectQueriesService>().Get( projectId );

                    return  project.Name == name || rulesHelper
                        .GetQueriesService<IProjectQueriesService>().IsProjectNameAvailable( name ); ;
                },
                () => {
                    return new OkObjectResult( name );
                },
                () => {
                    return new ConflictObjectResult( "this name is already taken" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectHasProjectProperties(
            this ConsistencyRulesHelper rulesHelper, Guid projectId, out ProjectProperties projectProperties ) {

            ProjectProperties projectPropertiesResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    projectPropertiesResult = rulesHelper
                        .GetQueriesService<IProjectPropertiesQueriesService>().GetByProjectId( projectId );

                    return projectPropertiesResult != null;
                },
                () => {
                    return new OkObjectResult( projectPropertiesResult );
                },
                () => {
                    return new NotFoundObjectResult( $"This project has not properties!" );
                } );

            projectProperties = projectPropertiesResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectHasNotProjectProperties(
            this ConsistencyRulesHelper rulesHelper, Guid projectId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var projectPropertiesResult = rulesHelper
                        .GetQueriesService<IProjectPropertiesQueriesService>().GetByProjectId( projectId );

                    return projectPropertiesResult == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"This project already has properties!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectHasNotLexiconAssignedYet(
            this ConsistencyRulesHelper rulesHelper, Guid projectId ) {

            ProjectProperties projectPropertiesResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IProjectPropertiesQueriesService>()
                        .GetByProjectId( projectId ).LexiconId == null;
                },
                () => {
                    return new OkObjectResult( projectPropertiesResult );
                },
                () => {
                    return new BadRequestObjectResult( $"This project has already lexicon assigned!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectHasAnalystConsoleActive(
            this ConsistencyRulesHelper rulesHelper, Guid projectId ) {

            ProjectProperties projectPropertiesResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IProjectPropertiesQueriesService>()
                        .GetByProjectId( projectId ).IsAnalystConsoleActive;
                },
                () => {
                    return new OkObjectResult( projectPropertiesResult );
                },
                () => {
                    return new BadRequestObjectResult( $"You must active the Analyst Console first" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, Project project ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return project.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"project {project.Id} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProjectHtmlContentIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid projectId, ProjectHtmlType type,
            out ProjectHtmlContent projectContacts ) {

            ProjectHtmlContent projContactsResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    projContactsResult = rulesHelper
                        .GetQueriesService<IProjectHtmlContentsQueriesService>()
                        .GetByProjectId( projectId, type );

                    return projContactsResult != null;
                },
                () => {
                    return new OkObjectResult( projContactsResult );
                },
                () => {
                    return new NotFoundObjectResult( $"project with id {projectId} not found!" );
                } );

            projectContacts = projContactsResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserIsInProject(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid? projectId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var projects = rulesHelper
                       .GetQueriesService<IProjectQueriesService>()
                       .GetProjectsWhereUserIsAssociated( userId );

                    return projects.Any( x => x.Id == projectId );
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"user {userId} is not into project {projectId}" );
                } );

            return validityChecker;
        }
    }
}

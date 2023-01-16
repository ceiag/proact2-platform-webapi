using Proact.Services.Entities;
using System;

namespace Proact.Services.QueriesServices {
    public class ProjectStateEditorService : IProjectStateEditorService {
        private readonly IProjectQueriesService _projectQueriesService;

        public ProjectStateEditorService( IProjectQueriesService projectQueriesService ) {
            _projectQueriesService = projectQueriesService;
        }

        public void CloseProject( Guid projectId ) {
            var project = _projectQueriesService.Get( projectId );
            var medicalTeams = project.MedicalTeams;

            project.State = ProjectState.Closed;

            foreach ( var medicalTeam in medicalTeams ) {
                medicalTeam.State = MedicalTeamState.ClosedByProject;
            }
        }

        public void OpenProject( Guid projectId ) {
            var project = _projectQueriesService.Get( projectId );
            var medicalTeams = project.MedicalTeams;

            project.State = ProjectState.Open;

            foreach ( var medicalTeam in medicalTeams ) {
                if ( medicalTeam.State == MedicalTeamState.ClosedByProject ) {
                    medicalTeam.State = MedicalTeamState.Open;
                }
            }
        }
    }
}

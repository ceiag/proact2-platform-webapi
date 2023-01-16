using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class ProjectQueriesService : IProjectQueriesService {
        private ProactDatabaseContext _database;

        public ProjectQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Project Create( Guid instituteId, ProjectCreateRequest projectCreateRequest ) {
            var project = new Project();

            project.InstituteId = instituteId;
            project.Name = projectCreateRequest.Name;
            project.Description = projectCreateRequest.Description;
            project.SponsorName = projectCreateRequest.SponsorName;
            project.State = ProjectState.Open;

            _database.Projects.Add( project );

            return project;
        }

        public Project Update( Guid projectId, ProjectUpdateRequest projectUpdateRequest ) {
            var project = Get( projectId );

            project.Name = projectUpdateRequest.Name;
            project.SponsorName = projectUpdateRequest.SponsorName;
            project.State = projectUpdateRequest.Status;
            project.Description = projectUpdateRequest.Description;

            return project;
        }

        public void AssignAdmin( Guid projectId, Medic medic ) {
            var project = Get( projectId );
            project.Admin = medic.User;
            project.AdminUserId = medic.UserId;
        }

        public void Remove( Guid projectId ) {
            _database.Projects.Remove( Get( projectId ) );
        }

        public Project Get( Guid projectId ) {
            return _database.Projects
                .Include( x => x.Admin )
                .Include( x => x.ProjectProperties )
                .FirstOrDefault( x => x.Id == projectId );
        }

        public bool IsProjectNameAvailable( string name ) {
            return !_database.Projects.Any( x => x.Name == name );
        }

        public bool IsOpened( Guid projectId ) {
            return Get( projectId ).State == ProjectState.Open;
        }

        public List<Project> GetProjectsWhereUserIsAssociated( Guid userId ) {
            return _database.Projects
                .Where( x => x.MedicalTeams.Any( x => x.Patients.Any( x => x.UserId == userId ) )
                    || x.MedicalTeams
                        .Any( x => x.MedicsRelation.Any( x => x.Medic.UserId == userId ) )
                    || x.MedicalTeams
                        .Any( x => x.NursesRelation.Any( x => x.Nurse.UserId == userId ) )
                    || x.MedicalTeams
                        .Any( x => x.ResearcherRelation.Any( x => x.Researcher.UserId == userId ) )
                    || x.MedicalTeams
                        .Any( x => x.DataManagersRelation.Any( x => x.DataManager.UserId == userId ) ) )
                .ToList();
        }

        public List<Project> GetsAll( Guid InstituteId ) {
            return _database.Projects
                .Where( x => x.InstituteId == InstituteId )
                .ToList();
        }
    }
}

using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class ProjectEntityMapper {
        public static List<ProjectModel> Map( List<Project> projects ) {
            List<ProjectModel> projectModels = new List<ProjectModel>();

            foreach ( var project in projects ) {
                projectModels.Add( Map( project ) );
            }

            return projectModels;
        }

        public static ProjectModel Map( Project project ) {
            if ( project == null )
                return null;

            return new ProjectModel() {
                ProjectId = project.Id,
                InstituteId = (Guid)project.InstituteId,
                Description = project.Description,
                Name = project.Name,
                SponsorName = project.SponsorName,
                Status = project.State,
                Properties = ProjectPropertiesEntityMapper.Map( project.ProjectProperties )
            };
        }
    }
}

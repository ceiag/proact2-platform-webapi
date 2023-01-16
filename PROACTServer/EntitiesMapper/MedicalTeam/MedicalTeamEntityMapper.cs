using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services {
    public static class MedicalTeamEntityMapper {
        private static string GetStringEmptyIfNull( string value ) {
            if ( string.IsNullOrEmpty( value ) ) {
                return string.Empty;
            }

            return value;
        }

        public static MedicalTeamModel Map( MedicalTeam medicalTeam ) {
            if ( medicalTeam == null ) 
                return null;

            return new MedicalTeamModel() {
                MedicalTeamId = medicalTeam.Id,
                AddressLine1 = GetStringEmptyIfNull( medicalTeam.AddressLine1 ),
                AddressLine2 = GetStringEmptyIfNull( medicalTeam.AddressLine2 ),
                City = GetStringEmptyIfNull( medicalTeam.City ),
                Country = GetStringEmptyIfNull( medicalTeam.Country ),
                Name = GetStringEmptyIfNull( medicalTeam.Name ),
                Phone = GetStringEmptyIfNull( medicalTeam.Phone ),
                PostalCode = GetStringEmptyIfNull( medicalTeam.PostalCode ),
                RegionCode = GetStringEmptyIfNull( medicalTeam.RegionCode ),
                StateOrProvince = GetStringEmptyIfNull( medicalTeam.StateOrProvince ),
                TimeZone = GetStringEmptyIfNull( medicalTeam.TimeZone ),
                State = medicalTeam.State,
                Project = ProjectEntityMapper.Map( medicalTeam.Project )
            };
        }

        public static List<MedicalTeamModel> Map( List<MedicalTeam> medicalTeams ) {
            var medicalTeamsModel = new List<MedicalTeamModel>();

            foreach ( var medicalTeam in medicalTeams ) {
                medicalTeamsModel.Add( Map( medicalTeam ) );
            }

            return medicalTeamsModel;
        }
    }
}

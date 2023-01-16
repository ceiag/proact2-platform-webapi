using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class MedicEntityMapper {
        public static MedicModel Map( Medic medic ) {
            return new MedicModel() {
                MedicalTeams = MedicalTeamEntityMapper.Map( medic.MedicalTeams ),
                UserId = medic.User.Id,
                InstituteId = (Guid)medic.User.InstituteId,
                AccountId = medic.User.AccountId,
                AvatarUrl = medic.User.AvatarUrl,
                Name = medic.User.Name,
                Title = medic.User.Title
            };
        }

        public static MedicModel Map( MedicAdmin medicAdmin ) {
            return new MedicModel() {
                MedicalTeams = new List<MedicalTeamModel>() {
                    MedicalTeamEntityMapper.Map( medicAdmin.MedicalTeam )
                },
                UserId = medicAdmin.User.Id,
                AccountId = medicAdmin.User.AccountId,
                AvatarUrl = medicAdmin.User.AvatarUrl,
                Name = medicAdmin.User.Name,
                Title = medicAdmin.User.Title
            };
        }

        public static List<MedicModel> Map( List<Medic> medics ) {
            var medicModels = new List<MedicModel>();

            foreach ( var medic in medics ) {
                medicModels.Add( Map( medic ) );
            }

            return medicModels;
        }

        public static List<MedicModel> Map( List<MedicAdmin> medics ) {
            var medicModels = new List<MedicModel>();

            foreach ( var medic in medics ) {
                medicModels.Add( Map( medic ) );
            }

            return medicModels;
        }
    }
}

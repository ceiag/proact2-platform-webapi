using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.EntitiesMapper {
    public class NurseEntityMapper {
        public static NurseModel Map( Nurse nurse ) {
            return new NurseModel() {
                MedicalTeams = MedicalTeamEntityMapper.Map( nurse.MedicalTeams ),
                UserId = nurse.User.Id,
                AccountId = nurse.User.AccountId,
                InstituteId = (Guid)nurse.User.InstituteId,
                AvatarUrl = nurse.User.AvatarUrl,
                Name = nurse.User.Name,
                Title = nurse.User.Title
            };
        }

        public static List<NurseModel> Map( List<Nurse> nurses ) {
            var nurseModels = new List<NurseModel>();

            foreach ( Nurse nurse in nurses ) {
                nurseModels.Add( Map( nurse ) );
            }

            return nurseModels;
        }
    }
}

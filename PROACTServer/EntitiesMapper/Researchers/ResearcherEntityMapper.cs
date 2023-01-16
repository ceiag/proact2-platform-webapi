using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.EntitiesMapper {
    public static class ResearcherEntityMapper {
        public static ResearcherModel Map( Researcher researcher ) {
            return new ResearcherModel() {
                MedicalTeams = MedicalTeamEntityMapper.Map( researcher.MedicalTeams ),
                UserId = researcher.User.Id,
                InstituteId = (Guid)researcher.User.InstituteId,
                AccountId = researcher.User.AccountId,
                AvatarUrl = researcher.User.AvatarUrl,
                Name = researcher.User.Name,
                Title = researcher.User.Title
            };
        }

        public static List<ResearcherModel> Map( List<Researcher> researchers ) {
            var researchersModel = new List<ResearcherModel>();

            foreach ( var researcher in researchers ) {
                researchersModel.Add( Map( researcher ) );
            }

            return researchersModel;
        }
    }
}

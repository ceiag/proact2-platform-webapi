using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.EntitiesMapper {
    public static class InstituteEntityMapper {
        public static InstituteModel Map( Institute institute ) {
            return new InstituteModel() {
                Id = institute.Id,
                Name = institute.Name,
                State = institute.State,
                Admins = UserEntityMapper.Map(
                    institute.Admins.Select( x => x.User ).ToList() ),
                Properties = new InstitutePropertiesModel() {
                    TermsAndConditions = GetTermsAndConditions( institute.Documents ),
                    PrivacyPolicy = GetPrivacyPolicy( institute.Documents )
                }
            };
        }

        public static List<InstituteModel> Map( List<Institute> institutes ) {
            var instituteModels = new List<InstituteModel>();

            foreach ( var institute in institutes ) {
                instituteModels.Add( Map( institute ) );
            }

            return instituteModels;
        }

        private static DocumentModel GetTermsAndConditions( List<Document> documents ) {
            return DocumentEntityMapper.Map( 
                documents.FirstOrDefault( x => x.Type == DocumentType.TermsAndConditions ) );
        }

        private static DocumentModel GetPrivacyPolicy( List<Document> documents ) {
            return DocumentEntityMapper.Map(
                documents.FirstOrDefault( x => x.Type == DocumentType.Privacy ) );
        }
    }
}

using System;
using System.Collections.Generic;

namespace Proact.Services.AuthorizationPolicies {
    public static class Roles {
        public const string SystemAdmin = "SystemAdmin";
        public const string InstituteAdmin = "InstituteAdmin";
        public const string ProjectAdmin = "ProjectAdmin";
        public const string MedicalTeamAdmin = "MedicalTeamAdmin";
        public const string MedicalProfessional = "MedicalProfessional";
        public const string Nurse = "Nurse";
        public const string Patient = "Patient";
        public const string Sponsor = "Sponsor";
        public const string Researcher = "Researcher";
        public const string MedicalTeamDataManager = "MedicalTeamDataManager";

        public const string ClaimTypeRoles = "Roles";

        public static List<String> AllRoles {   
            get {
                return new List<string>() {
                    SystemAdmin,
                    InstituteAdmin,
                    ProjectAdmin,
                    MedicalTeamAdmin,
                    MedicalProfessional,
                    Nurse,
                    Patient,
                    Sponsor,
                    Researcher,
                    MedicalTeamDataManager
                };
            }
        }
    }
}

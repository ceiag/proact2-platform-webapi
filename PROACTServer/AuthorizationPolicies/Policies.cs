using System;
using System.Collections.Generic;

namespace Proact.Services.AuthorizationPolicies {
    public static class Policies {

        public const string MessagesReadWrite = "Messages.ReadWrite";
        public const string MessagesPatientRead = "MessagesPatient.Read";
        public const string MessagesMineRead = "MessagesMineRead.Read";
        public const string MessagesBroadcastWrite = "MessagesBroadcast.Write";
        public const string MessagesNewTopicWrite = "Messages.NewTopicWrite";
        public const string MessagesAnalysisReadWrite = "MessageAnalysis.ReadWrite";
        public const string MessagesAnalysisFromBrowserReadWrite = "MessagesAnalysisConsoleReadWrite.ReadWrite";
        public const string UsersReadWrite = "Users.ReadWrite";
        public const string UsersMeRead = "Users.Me.Read";
        public const string PatientsRead = "PatientsRead.Read";
        public const string MedicalTeamReadWrite = "MedicalTeam.ReadWrite";
        public const string MedicalTeamRead = "MedicalTeam.Read";
        public const string InstitutesReadWrite = "Institutes.ReadWrite";
        public const string InstitutesRead = "Institutes.Read";
        public const string InstitutesMyReadWrite = "InstitutesMy.ReadWrite";
        public const string ProjectsReadWrite = "Projects.ReadWrite";
        public const string ProjectsRead = "Projects.Read";
        public const string SurveysReadWrite = "Surveys.ReadWrite";
        public const string SurveysRead = "Surveys.Read";
        public const string SurveysWrite = "Surveys.Write";
        public const string SurveysAnswerReadWrite = "SurveysAnswer.ReadWrite";
        public const string DataExporterRead = "DataExporter.Read";
        public const string LexiconReadWrite = "Lexicon.ReadWrite";
        public const string LexiconRead = "Lexicon.Read";
        public const string AppSettingsWrite = "AppSettings.Write";

        public static Dictionary<string, string[]> RolesAssociatedToPolicies
            = new Dictionary<string, string[]>() {
                {
                    AppSettingsWrite, new string[] {
                        Roles.SystemAdmin
                }   },
                {
                    UsersReadWrite, new string[] {
                        Roles.SystemAdmin,
                        Roles.MedicalTeamAdmin,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    UsersMeRead, new string[] {
                        Roles.Patient,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.Researcher,
                        Roles.InstituteAdmin,
                        Roles.SystemAdmin,
                        Roles.Nurse,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    MessagesReadWrite, new string[] {
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.Nurse, 
                        Roles.Patient
                }   },
                {
                    MessagesPatientRead, new string[] {
                        Roles.MedicalProfessional,
                        Roles.Nurse,
                        Roles.Researcher,
                        Roles.MedicalTeamAdmin,
                        Roles.Patient
                }   },
                {
                    MessagesMineRead, new string[] {
                        Roles.MedicalProfessional,
                        Roles.Nurse,
                        Roles.MedicalTeamAdmin,
                        Roles.Patient,
                        Roles.Researcher
                }   },
                {
                    MessagesAnalysisReadWrite, new string[] {
                        Roles.MedicalProfessional,
                        Roles.Nurse,
                        Roles.MedicalTeamAdmin,
                        Roles.Researcher
                }   },
                {
                    MedicalTeamReadWrite, new string[] {
                        Roles.InstituteAdmin,
                        Roles.MedicalTeamAdmin,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    MedicalTeamRead, new string[] {
                        Roles.Patient,
                        Roles.Researcher,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.InstituteAdmin,
                        Roles.Nurse,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    PatientsRead, new string[] {
                        Roles.Researcher,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.InstituteAdmin,
                        Roles.Nurse,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    ProjectsReadWrite, new string[] {
                        Roles.InstituteAdmin
                }   },
                {
                    MessagesBroadcastWrite, new string[] {
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.Nurse
                }   },
                {
                    MessagesNewTopicWrite, new string[] {
                        Roles.Patient
                }   },
                {
                    SurveysRead, new string[] {
                        Roles.InstituteAdmin,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.Nurse,
                        Roles.Researcher,
                        Roles.Patient,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    SurveysWrite, new string[] {
                        Roles.InstituteAdmin,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    SurveysReadWrite, new string[] {
                        Roles.InstituteAdmin,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    ProjectsRead, new string[] {
                        Roles.Patient,
                        Roles.Researcher,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.MedicalTeamDataManager,
                        Roles.InstituteAdmin,
                        Roles.Nurse
                }   },
                {
                    LexiconReadWrite, new string[] {
                        Roles.InstituteAdmin,
                }   },
                {
                    LexiconRead, new string[] {
                        Roles.Patient,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin,
                        Roles.InstituteAdmin,
                        Roles.Nurse,
                        Roles.Researcher,
                        Roles.MedicalTeamDataManager
                }   },
                {
                    InstitutesReadWrite, new string[] {
                        Roles.SystemAdmin
                }   },
                {
                    InstitutesRead, new string[] {
                        Roles.SystemAdmin,
                }   },
                {
                    InstitutesMyReadWrite, new string[] {
                        Roles.InstituteAdmin,
                }   },
                {
                    SurveysAnswerReadWrite, new string[] {
                        Roles.Patient
                }   },
                {
                    MessagesAnalysisFromBrowserReadWrite, new string[] {
                        Roles.Researcher,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin
                }   },
                {
                    DataExporterRead, new string[] {
                        Roles.Researcher,
                        Roles.MedicalProfessional,
                        Roles.MedicalTeamAdmin
                }   }
            };
    }
}

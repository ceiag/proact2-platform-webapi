using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.MessagesAttachment {
    public class IfUserCanDecryptMedia {
        private UserRoles _patientRoles = new UserRoles( new List<string>() { Roles.Patient } );
        private UserRoles _medicRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );
        private UserRoles _nurseRoles = new UserRoles( new List<string>() { Roles.Nurse } );

        [Fact]
        public void PatientOpenReplyMediaFromMedic_ReturnOk() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam;
            Patient patient;
            Medic medic;
            MessageModel originalMessage;
            MessageModel replyToMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage )
                .AddReplyFromMedicWithRandomValues( medic, originalMessage.MessageId, out replyToMessage );

            Message reply = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( replyToMessage.MessageId, out reply )
                    .IfUserCanDecryptMedia( reply, _patientRoles, patient.UserId )
                    .Then( () => {
                        return new OkObjectResult( reply );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkObjectResult );
        }

        [Fact]
        public void PatientOpenReplyMediaFromNurse_ReturnOk() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam;
            Patient patient;
            Nurse nurse;
            MessageModel originalMessage;
            MessageModel replyToMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddNurseWithRandomValues( medicalTeam, out nurse )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage )
                .AddReplyFromNurseWithRandomValues( nurse, originalMessage.MessageId, out replyToMessage );

            Message reply = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( replyToMessage.MessageId, out reply )
                    .IfUserCanDecryptMedia( reply, _patientRoles, patient.UserId )
                    .Then( () => {
                        return new OkObjectResult( reply );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkObjectResult );
        }

        [Fact]
        public void NurseOpenMediaFromPatient_ReturnOk() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam;
            Patient patient;
            Nurse nurse;
            MessageModel originalMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddNurseWithRandomValues( medicalTeam, out nurse )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage );

            Message message = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( originalMessage.MessageId, out message )
                    .IfUserCanDecryptMedia( message, _nurseRoles, nurse.UserId )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkObjectResult );
        }

        [Fact]
        public void MedicOpenMediaFromPatient_ReturnOk() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam;
            Patient patient;
            Medic medic;
            MessageModel originalMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage );

            Message message = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( originalMessage.MessageId, out message )
                    .IfUserCanDecryptMedia( message, _medicRoles, medic.UserId )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkObjectResult );
        }

        [Fact]
        public void MedicOpenMediaFromOtherMedicalTeam_ReturnUnauthorized() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam_0;
            MedicalTeam medicalTeam_1;
            Patient patient;
            Medic medic;
            MessageModel originalMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient )
                .AddMedicWithRandomValues( medicalTeam_1, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage );

            Message message = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( originalMessage.MessageId, out message )
                    .IfUserCanDecryptMedia( message, _medicRoles, medic.UserId )
                    .Then( () => {
                        return new UnauthorizedObjectResult( message );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as UnauthorizedObjectResult );
        }

        [Fact]
        public void NurseOpenMediaFromOtherMedicalTeam_ReturnUnauthorized() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project;
            MedicalTeam medicalTeam_0;
            MedicalTeam medicalTeam_1;
            Patient patient;
            Nurse nurse;
            MessageModel originalMessage;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient )
                .AddNurseWithRandomValues( medicalTeam_1, out nurse )
                .AddMessageFromPatientWithRandomValues( patient, out originalMessage );

            Message message = null;
            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMessageIsValid( originalMessage.MessageId, out message )
                    .IfUserCanDecryptMedia( message, _nurseRoles, nurse.UserId )
                    .Then( () => {
                        return new UnauthorizedObjectResult( message );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as UnauthorizedObjectResult );
        }
    }
}

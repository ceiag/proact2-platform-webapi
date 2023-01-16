using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.QueriesServices.Stats;
using Proact.Services.Tests.Shared;
using Proact.Services.Tests.Shared.Configs;
using Xunit;

namespace Proact.Services.UnitTests.Stats {
    public class GetMessagesStatsForProject {
        [Fact]
        public void _CheckStatsCorrectness() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            Medic medic_0 = null;
            Medic medic_1 = null;
            Nurse nurse_0 = null;
            Nurse nurse_1 = null;
            MessageModel message = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam, out patient_1 )
                .AddMedicWithRandomValues( medicalTeam, out medic_0 )
                .AddMedicWithRandomValues( medicalTeam, out medic_1 )
                .AddNurseWithRandomValues( medicalTeam, out nurse_0 )
                .AddNurseWithRandomValues( medicalTeam, out nurse_1 )
                .AddMessageFromPatientWithRandomValues( patient_0, out message )
                .AddMessageFromPatientWithRandomValues( patient_0, 4 )
                .AddReplyFromPatientWithRandomValues( patient_0, message.MessageId, 3 )
                .AddReplyFromMedicWithRandomValues( medic_0, message.MessageId, 2 )
                .AddReplyFromNurseWithRandomValues( nurse_0, message.MessageId, 5 )
                .AddMessageFromPatientWithAttachmentWithRandomValues(
                    patient_0, AttachmentType.IMAGE, 6 )
                .AddMessageFromPatientWithAttachmentWithRandomValues(
                    patient_0, AttachmentType.VIDEO, 7 )
                .AddMessageFromPatientWithAttachmentWithRandomValues(
                    patient_0, AttachmentType.AUDIO, 8 )
                .AddReplyFromPatientWithAttachmentWithRandomValues(
                    patient_0, message.MessageId, AttachmentType.IMAGE, 9 )
                .AddReplyFromPatientWithAttachmentWithRandomValues(
                    patient_0, message.MessageId, AttachmentType.VIDEO, 10 )
                .AddReplyFromPatientWithAttachmentWithRandomValues(
                    patient_0, message.MessageId, AttachmentType.AUDIO, 11 )
                .AddReplyFromMedicWithAttachmentWithRandomValues(
                    medic_0, message.MessageId, AttachmentType.IMAGE, 12 )
                .AddReplyFromMedicWithAttachmentWithRandomValues(
                    medic_0, message.MessageId, AttachmentType.AUDIO, 13 )
                .AddReplyFromMedicWithAttachmentWithRandomValues(
                    medic_0, message.MessageId, AttachmentType.VIDEO, 14 )
                .AddReplyFromNurseWithAttachmentWithRandomValues(
                    nurse_0, message.MessageId, AttachmentType.IMAGE, 15 )
                .AddReplyFromNurseWithAttachmentWithRandomValues(
                    nurse_0, message.MessageId, AttachmentType.AUDIO, 16 )
                .AddReplyFromNurseWithAttachmentWithRandomValues(
                    nurse_0, message.MessageId, AttachmentType.VIDEO, 17 );

            var statsProvider = servicesProvider
                .GetEditorService<IMessagesStatsProviderService>()
                .GetMessagesStatsForProject( project.Id );

            //patients
            Assert.Equal( 5, statsProvider.PatientsMessagesStats.TopicsTextOnly );
            Assert.Equal( 7, statsProvider.PatientsMessagesStats.TopicsWithVideo );
            Assert.Equal( 8, statsProvider.PatientsMessagesStats.TopicsWithAudio );
            Assert.Equal( 6, statsProvider.PatientsMessagesStats.TopicsWithImage );
            Assert.Equal( 3, statsProvider.PatientsMessagesStats.RepliesTextOnly );
            Assert.Equal( 10, statsProvider.PatientsMessagesStats.RepliesWithVideo );
            Assert.Equal( 11, statsProvider.PatientsMessagesStats.RepliesWithAudio );
            Assert.Equal( 9, statsProvider.PatientsMessagesStats.RepliesWithImage );
            Assert.Equal( 4, statsProvider.PatientsMessagesStats.UnrepliedTextOnly );
            Assert.Equal( 7, statsProvider.PatientsMessagesStats.UnrepliedWithVideo );
            Assert.Equal( 8, statsProvider.PatientsMessagesStats.UnrepliedWithAudio );
            Assert.Equal( 6, statsProvider.PatientsMessagesStats.UnrepliedWithImage );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.PatientsMessagesStats.AvgTopicsWithVideoDuration );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.PatientsMessagesStats.AvgTopicsWithAudioDuration );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.PatientsMessagesStats.AvgRepliesWithVideoDuration );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.PatientsMessagesStats.AvgRepliesWithAudioDuration );
            Assert.Equal( DefaultConfigs.Message.Length,
                statsProvider.PatientsMessagesStats.AvgTopicsTextLength );
            Assert.Equal( DefaultConfigs.Message.Length,
                statsProvider.PatientsMessagesStats.AvgRepliesTextLength );
            Assert.Equal( 2.5f, statsProvider.PatientsMessagesStats.AvgTopicsTextOnly );
            Assert.Equal( 3.5f, statsProvider.PatientsMessagesStats.AvgTopicsWithVideo );
            Assert.Equal( 4.0f, statsProvider.PatientsMessagesStats.AvgTopicsWithAudio );
            Assert.Equal( 3.0f, statsProvider.PatientsMessagesStats.AvgTopicsWithImage );
            Assert.Equal( 1.5f, statsProvider.PatientsMessagesStats.AvgRepliesTextOnly );
            Assert.Equal( 5.0f, statsProvider.PatientsMessagesStats.AvgRepliesWithVideo );
            Assert.Equal( 5.5f, statsProvider.PatientsMessagesStats.AvgRepliesWithAudio );
            Assert.Equal( 4.5f, statsProvider.PatientsMessagesStats.AvgRepliesWithImage );
            Assert.Equal( 2.0f, statsProvider.PatientsMessagesStats.AvgUnrepliedTextOnly );
            Assert.Equal( 3.5f, statsProvider.PatientsMessagesStats.AvgUnrepliedWithVideo );
            Assert.Equal( 4.0f, statsProvider.PatientsMessagesStats.AvgUnrepliedWithAudio );
            Assert.Equal( 3.0f, statsProvider.PatientsMessagesStats.AvgUnrepliedWithImage );

            //medics
            Assert.Equal( 2, statsProvider.MedicsMessagesStats.RepliesTextOnly );
            Assert.Equal( 14, statsProvider.MedicsMessagesStats.RepliesWithVideo );
            Assert.Equal( 13, statsProvider.MedicsMessagesStats.RepliesWithAudio );
            Assert.Equal( 12, statsProvider.MedicsMessagesStats.RepliesWithImage );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.MedicsMessagesStats.AvgRepliesWithVideoDuration );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.MedicsMessagesStats.AvgRepliesWithAudioDuration );
            Assert.Equal( DefaultConfigs.Message.Length,
                statsProvider.MedicsMessagesStats.AvgRepliesTextLength );
            Assert.Equal( 1.0f, statsProvider.MedicsMessagesStats.AvgRepliesTextOnly );
            Assert.Equal( 7.0f, statsProvider.MedicsMessagesStats.AvgRepliesWithVideo );
            Assert.Equal( 6.5f, statsProvider.MedicsMessagesStats.AvgRepliesWithAudio );
            Assert.Equal( 6.0f, statsProvider.MedicsMessagesStats.AvgRepliesWithImage );

            //nurses
            Assert.Equal( 5, statsProvider.NursesMessagesStats.RepliesTextOnly );
            Assert.Equal( 17, statsProvider.NursesMessagesStats.RepliesWithVideo );
            Assert.Equal( 16, statsProvider.NursesMessagesStats.RepliesWithAudio );
            Assert.Equal( 15, statsProvider.NursesMessagesStats.RepliesWithImage );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.NursesMessagesStats.AvgRepliesWithVideoDuration );
            Assert.Equal( DefaultConfigs.MediaDuration,
                statsProvider.NursesMessagesStats.AvgRepliesWithAudioDuration );
            Assert.Equal( DefaultConfigs.Message.Length,
                statsProvider.NursesMessagesStats.AvgRepliesTextLength );
            Assert.Equal( 2.5f, statsProvider.NursesMessagesStats.AvgRepliesTextOnly );
            Assert.Equal( 8.5f, statsProvider.NursesMessagesStats.AvgRepliesWithVideo );
            Assert.Equal( 8.0f, statsProvider.NursesMessagesStats.AvgRepliesWithAudio );
            Assert.Equal( 7.5f, statsProvider.NursesMessagesStats.AvgRepliesWithImage );
        }
    }
}

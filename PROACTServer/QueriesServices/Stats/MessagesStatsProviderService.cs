using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Stats;
using Proact.Services.QueriesServices.Stats.StatsQueries;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices.Stats {
    public class MessagesStatsProviderService : IMessagesStatsProviderService {
        private readonly ProactDatabaseContext _database;

        public MessagesStatsProviderService( ProactDatabaseContext database ) {
            _database = database;
        }

        private IQueryable<Message> GetMessages() {
            return _database.Messages;
        }

        private IQueryable<Message> GetTopicsTextOnly() {
            return GetMessages().Topics().TextOnly();
        }

        private IQueryable<Message> GetTopicsWithVideo() {
            return GetMessages().Topics().WithVideo();
        }

        private IQueryable<Message> GetTopicsWithAudio() {
            return GetMessages().Topics().WithAudio();
        }

        private IQueryable<Message> GetTopicsWithImage() {
            return GetMessages().Topics().WithImage();
        }

        private IQueryable<Message> GetRepliesTextOnly() {
            return GetMessages().Replies().TextOnly();
        }

        private IQueryable<Message> GetRepliesWithVideo() {
            return GetMessages().Replies().WithVideo();
        }

        private IQueryable<Message> GetRepliesWithAudio() {
            return GetMessages().Replies().WithAudio();
        }

        private IQueryable<Message> GetRepliesWithImage() {
            return GetMessages().Replies().WithImage();
        }

        public MessagesStatsModel GetMessagesStats() {
            int numberOfPatients = _database.Patients.Count();
            int numberOfMedics = _database.Medics.Count();
            int numberOfNurses = _database.Nurses.Count();

            return new MessagesStatsModel() {
                PatientsMessagesStats = new PatientMessagesStatsModel( numberOfPatients ) {
                    TopicsTextOnly = GetTopicsTextOnly().FromPatients().Count(),
                    RepliesTextOnly = GetRepliesTextOnly().FromPatients().Count(),
                    TopicsWithVideo = GetTopicsWithVideo().FromPatients().Count(),
                    TopicsWithAudio = GetTopicsWithAudio().FromPatients().Count(),
                    TopicsWithImage = GetTopicsWithImage().FromPatients().Count(),
                    RepliesWithVideo = GetRepliesWithVideo().FromPatients().Count(),
                    RepliesWithAudio = GetRepliesWithAudio().FromPatients().Count(),
                    RepliesWithImage = GetRepliesWithImage().FromPatients().Count(),
                    AvgTopicsWithVideoDuration = GetTopicsWithVideo().FromPatients().AvgDuration(),
                    AvgTopicsWithAudioDuration = GetTopicsWithAudio().FromPatients().AvgDuration(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo().FromPatients().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio().FromPatients().AvgDuration(),
                    AvgTopicsTextLength = GetTopicsTextOnly().FromPatients().AvgTextLength(),
                    AvgRepliesTextLength = GetRepliesTextOnly().FromPatients().AvgTextLength(),
                    UnrepliedTextOnly = GetTopicsTextOnly().FromPatients().Unreplied().Count(),
                    UnrepliedWithVideo = GetTopicsWithVideo().FromPatients().Unreplied().Count(),
                    UnrepliedWithAudio = GetTopicsWithAudio().FromPatients().Unreplied().Count(),
                    UnrepliedWithImage = GetTopicsWithImage().FromPatients().Unreplied().Count(),
                    ScopeNotSpecified = GetMessages().Topics().FromPatients()
                        .WithMessageScope( MessageScope.None ).Count(),
                    InfoScope = GetMessages().Topics().FromPatients()
                        .WithMessageScope( MessageScope.Info ).Count(),
                    HealthScope = GetMessages().Topics().FromPatients()
                        .WithMessageScope( MessageScope.Health ).Count(),
                    MoodVeryBad = GetMessages().Topics().FromPatients()
                        .WithPatientMood( PatientMood.VeryBad ).Count(),
                    MoodBad = GetMessages().Topics().FromPatients()
                        .WithPatientMood( PatientMood.Bad ).Count(),
                    MoodGood = GetMessages().Topics().FromPatients()
                        .WithPatientMood( PatientMood.Good ).Count(),
                    MoodVeryGood = GetMessages().Topics().FromPatients()
                        .WithPatientMood( PatientMood.VeryGood ).Count(),
                    MoodNotSpecified = GetMessages().Topics().FromPatients()
                        .WithPatientMood( PatientMood.None ).Count(),
                },
                MedicsMessagesStats = new MedicMessagesStatsModel( numberOfMedics ) {
                    RepliesTextOnly = GetRepliesTextOnly().FromMedics().Count(),
                    RepliesWithVideo = GetRepliesWithVideo().FromMedics().Count(),
                    RepliesWithAudio = GetRepliesWithAudio().FromMedics().Count(),
                    RepliesWithImage = GetRepliesWithImage().FromMedics().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo().FromMedics().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio().FromMedics().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly().FromMedics().AvgTextLength()
                },
                NursesMessagesStats = new NurseMessagesStatsModel( numberOfNurses ) {
                    RepliesTextOnly = GetRepliesTextOnly().FromNurses().Count(),
                    RepliesWithVideo = GetRepliesWithVideo().FromNurses().Count(),
                    RepliesWithAudio = GetRepliesWithAudio().FromNurses().Count(),
                    RepliesWithImage = GetRepliesWithImage().FromNurses().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo().FromNurses().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio().FromNurses().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly().FromNurses().AvgTextLength(),
                }
            };
        }

        public MessagesStatsModel GetMessagesStatsForInstitute( Guid instituteId ) {
            int numberOfPatients = _database.Patients.InsideInstitute( instituteId ).Count();
            int numberOfMedics = _database.Medics.InsideInstitute( instituteId ).Count();
            int numberOfNurses = _database.Nurses.InsideInstitute( instituteId ).Count();

            return new MessagesStatsModel() {
                PatientsMessagesStats = new PatientMessagesStatsModel( numberOfPatients ) {
                    TopicsTextOnly = GetTopicsTextOnly()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    TopicsWithVideo = GetTopicsWithVideo()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    TopicsWithAudio = GetTopicsWithAudio()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    TopicsWithImage = GetTopicsWithImage()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromInstitute( instituteId ).FromPatients().Count(),
                    AvgTopicsWithVideoDuration = GetTopicsWithVideo()
                        .FromInstitute( instituteId ).FromPatients().AvgDuration(),
                    AvgTopicsWithAudioDuration = GetTopicsWithAudio()
                        .FromInstitute( instituteId ).FromPatients().AvgDuration(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromPatients().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromPatients().AvgDuration(),
                    AvgTopicsTextLength = GetTopicsTextOnly()
                        .FromInstitute( instituteId ).FromPatients().AvgTextLength(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromPatients().AvgTextLength(),
                    UnrepliedTextOnly = GetTopicsTextOnly()
                        .FromInstitute( instituteId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithVideo = GetTopicsWithVideo()
                        .FromInstitute( instituteId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithAudio = GetTopicsWithAudio()
                        .FromInstitute( instituteId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithImage = GetTopicsWithImage()
                        .FromInstitute( instituteId ).FromPatients().Unreplied().Count(),
                },
                MedicsMessagesStats = new MedicMessagesStatsModel( numberOfMedics ) {
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromMedics().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromMedics().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromMedics().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromInstitute( instituteId ).FromMedics().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromMedics().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromMedics().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromMedics().AvgTextLength(),
                },
                NursesMessagesStats = new NurseMessagesStatsModel( numberOfNurses ) {
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromNurses().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromNurses().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromNurses().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromInstitute( instituteId ).FromNurses().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromInstitute( instituteId ).FromNurses().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromInstitute( instituteId ).FromNurses().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromInstitute( instituteId ).FromNurses().AvgTextLength()
                }
            };
        }

        public MessagesStatsModel GetMessagesStatsForProject( Guid projectId ) {
            int numberOfPatients = _database.Patients.InsideProject( projectId ).Count();
            int numberOfMedics = _database.Medics.InsideProject( projectId ).Count();
            int numberOfNurses = _database.Nurses.InsideProject( projectId ).Count();

            return new MessagesStatsModel() {
                PatientsMessagesStats = new PatientMessagesStatsModel( numberOfPatients ) {
                    TopicsTextOnly = GetTopicsTextOnly()
                        .FromProject( projectId ).FromPatients().Count(),
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromProject( projectId ).FromPatients().Count(),
                    TopicsWithVideo = GetTopicsWithVideo()
                        .FromProject( projectId ).FromPatients().Count(),
                    TopicsWithAudio = GetTopicsWithAudio()
                        .FromProject( projectId ).FromPatients().Count(),
                    TopicsWithImage = GetTopicsWithImage()
                        .FromProject( projectId ).FromPatients().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromProject( projectId ).FromPatients().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromProject( projectId ).FromPatients().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromProject( projectId ).FromPatients().Count(),
                    AvgTopicsWithVideoDuration = GetTopicsWithVideo()
                        .FromProject( projectId ).FromPatients().AvgDuration(),
                    AvgTopicsWithAudioDuration = GetTopicsWithAudio()
                        .FromProject( projectId ).FromPatients().AvgDuration(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromProject( projectId ).FromPatients().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromProject( projectId ).FromPatients().AvgDuration(),
                    AvgTopicsTextLength = GetTopicsTextOnly()
                        .FromProject( projectId ).FromPatients().AvgTextLength(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromProject( projectId ).FromPatients().AvgTextLength(),
                    UnrepliedTextOnly = GetTopicsTextOnly()
                        .FromProject( projectId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithVideo = GetTopicsWithVideo()
                        .FromProject( projectId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithAudio = GetTopicsWithAudio()
                        .FromProject( projectId ).FromPatients().Unreplied().Count(),
                    UnrepliedWithImage = GetTopicsWithImage()
                        .FromProject( projectId ).FromPatients().Unreplied().Count(),
                },
                MedicsMessagesStats = new MedicMessagesStatsModel( numberOfMedics ) {
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromProject( projectId ).FromMedics().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromProject( projectId ).FromMedics().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromProject( projectId ).FromMedics().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromProject( projectId ).FromMedics().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromProject( projectId ).FromMedics().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromProject( projectId ).FromMedics().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromProject( projectId ).FromMedics().AvgTextLength(),
                },
                NursesMessagesStats = new NurseMessagesStatsModel( numberOfNurses ) {
                    RepliesTextOnly = GetRepliesTextOnly()
                        .FromProject( projectId ).FromNurses().Count(),
                    RepliesWithVideo = GetRepliesWithVideo()
                        .FromProject( projectId ).FromNurses().Count(),
                    RepliesWithAudio = GetRepliesWithAudio()
                        .FromProject( projectId ).FromNurses().Count(),
                    RepliesWithImage = GetRepliesWithImage()
                        .FromProject( projectId ).FromNurses().Count(),
                    AvgRepliesWithVideoDuration = GetRepliesWithVideo()
                        .FromProject( projectId ).FromNurses().AvgDuration(),
                    AvgRepliesWithAudioDuration = GetRepliesWithAudio()
                        .FromProject( projectId ).FromNurses().AvgDuration(),
                    AvgRepliesTextLength = GetRepliesTextOnly()
                        .FromProject( projectId ).FromNurses().AvgTextLength(),
                }
            };
        }
    }
}

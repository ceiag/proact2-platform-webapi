using Moq;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;

namespace Proact.Services.UnitTests {
    public class MockDatabaseUnitTestHelper : IDisposable {
        private readonly ServicesProvider _serviceProvider = new ServicesProvider();
        private ConsistencyRulesHelper _consistencyRulesHelper;

        public ServicesProvider ServicesProvider {
            get { return _serviceProvider; }
        }

        public ConsistencyRulesHelper ConsistencyRulesHelper {
            get { return _consistencyRulesHelper; }
        }

        public MockDatabaseUnitTestHelper() {
            InitServiceProvider();
        }

        private void InitServiceProvider() {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup( x => x.GetService( typeof( IUserQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IUserQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IMedicQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IMedicQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( INurseQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<INurseQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IPatientQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IPatientQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IMedicalTeamQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IMedicalTeamQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IProjectQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IProjectQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IProjectPropertiesQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IProjectPropertiesQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IMessagesQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IMessagesQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyQuestionsQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyQuestionsQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyAssignationQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyAssignationQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyQuestionsSetQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyQuestionsSetQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IUserNotificationSettingsQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IUserNotificationSettingsQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswersQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyAnswersQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswersBlockQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyAnswersBlockQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswerToQuestionQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveyAnswerToQuestionQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ISurveySchedulerQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ISurveySchedulerQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IDeviceQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IDeviceQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( ILexiconQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<ILexiconQueriesService>() );
            serviceProvider
                .Setup( x => x.GetService( typeof( IMessageAnalysisQueriesService ) ) )
                .Returns( _serviceProvider.GetQueriesService<IMessageAnalysisQueriesService>() );

            _consistencyRulesHelper = new ConsistencyRulesHelper(
                serviceProvider.Object, new Mock<IErrorsExplainerService>().Object );
        }

        public string GenerateRandomName() {
            return Guid.NewGuid().ToString();
        }

        public Project CreateDummyProject() {
            var projectCreationRequest = new ProjectCreateRequest() {
                Description = GenerateRandomName(),
                Name = GenerateRandomName(),
                SponsorName = GenerateRandomName()
            };

            var project = ServicesProvider
                .GetQueriesService<IProjectQueriesService>().Create( projectCreationRequest );
            ServicesProvider.SaveChanges();

            return project;
        }

        public ProjectProperties CreateDummyProjectProperties( 
            Project project, 
            bool medicsCanSeeOtherAnalisys, 
            int messageCanBeAnalizedAfterMinutes, 
            int messageCanNotBeDeletedAfterMinutes, 
            int messageCanBeRepliedAfterMinutes ) {
            var request = new ProjectPropertiesCreateRequest() {
                MedicsCanSeeOtherAnalisys = medicsCanSeeOtherAnalisys,
                MessageCanBeAnalizedAfterMinutes = messageCanBeAnalizedAfterMinutes,
                MessageCanNotBeDeletedAfterMinutes = messageCanNotBeDeletedAfterMinutes,
                MessageCanBeRepliedAfterMinutes = messageCanBeRepliedAfterMinutes,
                IsAnalystConsoleActive = true,
                IsSurveysSystemActive = true
            };

            var projectProps = ServicesProvider
                .GetQueriesService<IProjectPropertiesQueriesService>().Create( project.Id, request );

            ServicesProvider.SaveChanges();

            return projectProps;
        }

        public User CreateDummyUser() {
            var user = new User() {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            ServicesProvider.GetQueriesService<IUserQueriesService>().Create( user );
            ServicesProvider.SaveChanges();

            return user;
        }

        public Patient CreateDummyPatient( User user ) {
            var patientCreationRequest = new PatientCreateRequest() {
                BirthYear = new Random().Next( 1900, 2000 ),
                Gender = "M",
                TreatmentStartDate = DateTime.Now,
            };

            var patient = ServicesProvider
                .GetQueriesService<IPatientQueriesService>().Create( user, patientCreationRequest );

            ServicesProvider.SaveChanges();

            return patient;
        }

        public Nurse CreateDummyNurse( User user ) {
            var nurse = ServicesProvider.GetQueriesService<INurseQueriesService>().Create( user.Id );
            ServicesProvider.SaveChanges();

            return nurse;
        }

        public MedicalTeam CreateDummyMedicalTeam( Project project ) {
            var medicalTeamCreationRequest = new MedicalTeamCreateRequest() {
                AddressLine1 = GenerateRandomName(),
                AddressLine2 = GenerateRandomName(),
                City = GenerateRandomName(),
                Country = GenerateRandomName(),
                Name = GenerateRandomName(),
                Phone = GenerateRandomName(),
                PostalCode = GenerateRandomName(),
                RegionCode = "IT-GE",
                StateOrProvince = "IT",
                TimeZone = "GMT+1"
            };

            var medicalTeam = ServicesProvider
                .GetQueriesService<IMedicalTeamQueriesService>()
                .Create( project.Id, medicalTeamCreationRequest );

            ServicesProvider.SaveChanges();

            medicalTeam.Patients = new List<Patient>();

            return medicalTeam;
        }

        public Medic CreateDummyMedic( User user ) {
            var medic = ServicesProvider.GetQueriesService<IMedicQueriesService>().Create( user.Id );
            ServicesProvider.SaveChanges();

            return medic;
        }

        public Device AddDummyDeviceToUser( Guid userId ) {
            return ServicesProvider
                .GetEditorsService<IUserNotificationsSettingsEditorService>()
                .AddDevice( userId, Guid.NewGuid() );
        }

        public MessageModel CreateNewTopicMessage( User user, MedicalTeam medicalTeam, string body ) {
            var messageCreationParams = GetDummyMessageCreationParams( user, medicalTeam, Roles.Patient );
            messageCreationParams.MessageRequestData.Body = body;

            var createdMessage = ServicesProvider.GetEditorsService<IMessageEditorService>()
                .CreateNewTopicMessage( messageCreationParams );

            return createdMessage;
        }

        public MessageModel CreateDummyNewTopicMessage( User user, MedicalTeam medicalTeam ) {
            var messageCreationParams = GetDummyMessageCreationParams( user, medicalTeam, Roles.Patient );

            var createdMessage = ServicesProvider.GetEditorsService<IMessageEditorService>()
                .CreateNewTopicMessage( messageCreationParams );

            ServicesProvider.SaveChanges();

            return createdMessage;
        }

        public MessageModel CreateDummyBroadcastMessage( User user, MedicalTeam medicalTeam ) {
            var messageCreationParams = GetDummyMessageCreationParams(
                user, medicalTeam, Roles.MedicalProfessional );

            var createdMessage = ServicesProvider.GetEditorsService<IMessageEditorService>()
                .CreateBroadcastMessage( messageCreationParams );

            ServicesProvider.SaveChanges();

            return createdMessage;
        }

        private MessageCreationParams GetDummyMessageCreationParams(
            User user, MedicalTeam medicalTeam, string role ) {
            return new MessageCreationParams() {
                MedicalTeam = medicalTeam,
                User = user,
                UserRoles = new UserRoles( new List<string>() { role } ),
                ShowAfterCreation = true,
                MessageRequestData = new MessageRequestData() {
                    Body = GenerateRandomName(),
                    Title = GenerateRandomName(),
                    HasAttachment = false,
                    Emotion = (PatientMood)new Random().Next( 0, 4 )
                }
            };
        }

        public MessageModel CreateDummyReplyToMessage(
            User user, MedicalTeam medicalTeam, MessageModel originalMessage ) {

            var messageCreationParams = GetDummyMessageCreationParams( user, medicalTeam, Roles.Patient );

            return ServicesProvider.GetEditorsService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessage.MessageId );
        }

        public MessageModel CreateDummyReplyToMessage(
            Patient patient, MedicalTeam medicalTeam, MessageModel originalMessage ) {

            var messageCreationParams = GetDummyMessageCreationParams(
                patient.User, medicalTeam, Roles.Patient );

            return ServicesProvider.GetEditorsService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessage.MessageId );
        }

        public MessageModel CreateDummyReplyToMessage(
            Medic medic, MedicalTeam medicalTeam, MessageModel originalMessage ) {

            var messageCreationParams = GetDummyMessageCreationParams(
                medic.User, medicalTeam, Roles.MedicalProfessional );

            return ServicesProvider.GetEditorsService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessage.MessageId );
        }

        public MessageModel CreateReplyToMessage(
            User user, MedicalTeam medicalTeam, MessageModel originalMessage ) {

            var messageCreationParams = GetDummyMessageCreationParams( user, medicalTeam, Roles.Patient );

            return ServicesProvider.GetEditorsService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessage.MessageId );
        }

        public void Dispose() {
        }
    }
}

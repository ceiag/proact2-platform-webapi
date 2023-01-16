using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using Proact.Services.Messages;
using Proact.Services.Models;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.DataManagers;
using Proact.Services.QueriesServices.Stats;
using Proact.Services.QueriesServices.Surveys.Scheduler;
using Proact.Services.QueriesServices.Surveys.Stats;
using Proact.Services.Services;
using System;

namespace Proact.Services.Tests.Shared {
    public class ProactServicesProvider {
        private ProactDatabaseContext _database;
        private IServiceProvider _serviceProvider;
        private IChangesTrackingService _changesTrackingService;
        private IUserIdentityService _userIdentityService;
        private IUsersCreatorQueriesService _usersCreatorQueriesService;
        private IGroupService _groupService;
        private ConsistencyRulesHelper _consistencyRulesHelper;

        public ProactDatabaseContext Database {
            get { return _database; }
        }

        public ConsistencyRulesHelper ConsistencyRulesHelper { 
            get { return _consistencyRulesHelper; }
        }

        public IServiceProvider ServiceProvider {
            get { return _serviceProvider; }
        }

        public IChangesTrackingService ChangesTrackingService { 
            get { return _changesTrackingService; }
        }

        public IUserIdentityService UserIdentityService {
            get { return _userIdentityService; }
        }

        public IUsersCreatorQueriesService UsersCreatorQueriesService {
            get { return _usersCreatorQueriesService; }
        }

        public IGroupService GroupService {
            get { return _groupService; }
        }

        public T GetQueriesService<T>() where T : IQueriesService {
            return (T)_serviceProvider.GetService( typeof( T ) );
        }

        public T GetEditorService<T>() where T : IDataEditorService {
            return (T)_serviceProvider.GetService( typeof( T ) );
        }

        public ProactServicesProvider() {
            CreateDatabase();

            var serviceProviderMock = new Mock<IServiceProvider>();
            InitQueriesServices( serviceProviderMock );

            _serviceProvider = serviceProviderMock.Object;
            InitEditorServices( serviceProviderMock );

            InitChangeTrackingService();
            InitCommonServices();
            InitConsistencyRules();
            InitUserIdentityService();
            InitGroupService();
            InitUsersCreatorService();

            AvatarConfiguration.Init( new Mock<IConfiguration>().Object );
        }

        private void CreateDatabase() {
            _database = new DatabaseProvider().CreateDatabase();
        }

        private void InitQueriesServices( Mock<IServiceProvider> mockServiceProvider ) {
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IUserQueriesService ) ) )
                .Returns( new UserQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IProjectQueriesService ) ) )
                .Returns( new ProjectQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IInstitutesQueriesService ) ) )
                .Returns( new InstitutesQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IProjectPropertiesQueriesService ) ) )
                .Returns( new ProjectPropertiesQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMedicalTeamQueriesService ) ) )
                .Returns( new MedicalTeamQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IPatientQueriesService ) ) )
                .Returns( new PatientQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMedicQueriesService ) ) )
                .Returns( new MedicQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( INurseQueriesService ) ) )
                .Returns( new NurseQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IResearcherQueriesService ) ) )
                .Returns( new ResearcherQueriesService( _database ) );
            mockServiceProvider
               .Setup( x => x.GetService( typeof( IDataManagerQueriesService ) ) )
               .Returns( new DataManagerQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMessagesQueriesService ) ) )
                .Returns( new MessagesQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ILexiconQueriesService ) ) )
                .Returns( new LexiconQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMessageAnalysisQueriesService ) ) )
                .Returns( new MessageAnalysisQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ILexiconCategoriesQueriesService ) ) )
                .Returns( new LexiconCategoriesQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ILexiconLabelQueriesService ) ) )
                .Returns( new LexiconLabelQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyQuestionsQueriesService ) ) )
                .Returns( new SurveyQuestionsQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyQueriesService ) ) )
                .Returns( new SurveyQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswersQueriesService ) ) )
                .Returns( new SurveyAnswersQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswersBlockQueriesService ) ) )
                .Returns( new SurveyAnswersBlockQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyQuestionsSetQueriesService ) ) )
                .Returns( new SurveyQuestionsSetQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswerToQuestionQueriesService ) ) )
                .Returns( new SurveyAnswerToQuestionQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyAssignationQueriesService ) ) )
                .Returns( new SurveyAssignationQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveySchedulerQueriesService ) ) )
                .Returns( new SurveySchedulerQueriesService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IUserNotificationSettingsQueriesService ) ) )
                .Returns( new NotificationSettingsQueriesService( _database ) );
        }

        private void InitEditorServices( Mock<IServiceProvider> mockServiceProvider ) {
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMessageEditorService ) ) )
                .Returns( new MessageEditorService( _database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IOrganizedMessagesProvider ) ) )
                .Returns( new OrganizedMessagesProvider( MockStringLocalizer() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMessageFormatterService ) ) )
                .Returns( new MessageFormatterService( 
                    GetQueriesService<IMessagesQueriesService>(),
                    GetEditorService<IOrganizedMessagesProvider>(),
                    GetQueriesService<IPatientQueriesService>() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyQuestionsEditorService ) ) )
                .Returns( new SurveyQuestionsEditorService(
                    GetQueriesService<ISurveyQuestionsQueriesService>(),
                    GetQueriesService<ISurveyAnswersQueriesService>(),
                    GetQueriesService<ISurveyQuestionsSetQueriesService>(),
                    GetQueriesService<ISurveyAnswersBlockQueriesService>() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyAnswerToQuestionEditorService ) ) )
                .Returns( new SurveyAnswerToQuestionEditorService(
                    GetQueriesService<ISurveyAnswerToQuestionQueriesService>(),
                    GetQueriesService<ISurveyAnswersQueriesService>(),
                    GetQueriesService<ISurveyAssignationQueriesService>(),
                    GetQueriesService<ISurveyQuestionsQueriesService>() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( IMessagesStatsProviderService ) ) )
                .Returns( new MessagesStatsProviderService( Database ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveysStatsQueriesService ) ) )
                .Returns( new SurveysStatsQueriesService( 
                    GetQueriesService<ISurveyAssignationQueriesService>(),
                    GetQueriesService<ISurveyQueriesService>(),
                    GetQueriesService<IPatientQueriesService>() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveySchedulerDispatcherService ) ) )
                .Returns( new SurveySchedulerDispatcherService(
                    GetQueriesService<ISurveySchedulerQueriesService>(),
                    GetQueriesService<ISurveyAssignationQueriesService>(),
                    new Mock<INotificationProviderService>().Object,
                    GetQueriesService<IUserNotificationSettingsQueriesService>() ) );
            mockServiceProvider
                .Setup( x => x.GetService( typeof( ISurveyStatsOverTimeQueriesService ) ) )
                .Returns( new SurveyStatsOverTimeQueriesService(
                    GetQueriesService<ISurveyAssignationQueriesService>(),
                    GetQueriesService<ISurveyQueriesService>() ) );
        }

        private void InitConsistencyRules() {
            _consistencyRulesHelper = new ConsistencyRulesHelper( _serviceProvider, MockStringLocalizer() );
        }

        private void InitChangeTrackingService() {
            _changesTrackingService = new ChangesTrackingService( 
                _database, GetQueriesService<IUserQueriesService>() );
        }

        private void InitGroupService() {
            _groupService = new Mock<IGroupService>().Object;
        }

        private void InitUserIdentityService() {
            var userIdentityServiceMock = new Mock<IUserIdentityService>();
            userIdentityServiceMock
                .Setup( x => x.Create( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) )
                .ReturnsAsync( new UserModel() { UserId = Guid.NewGuid() } );

            _userIdentityService = userIdentityServiceMock.Object;
        }

        private void InitUsersCreatorService() {
            _usersCreatorQueriesService = new UsersCreatorQueriesService(
                UserIdentityService, GroupService,
                GetQueriesService<IUserQueriesService>(),
                GetQueriesService<IPatientQueriesService>(),
                GetQueriesService<IMedicQueriesService>(),
                GetQueriesService<INurseQueriesService>(),
                GetQueriesService<IResearcherQueriesService>(),
                GetQueriesService<IDataManagerQueriesService>(),
                Database );
        }

        private void InitCommonServices() {
        }

        public IStringLocalizer<Resource> MockStringLocalizer() {
            var stringLocalizer = new Mock<IStringLocalizer<Resource>>();
            string key = "medical_team";
            var localizedString = new LocalizedString( key, "Team Medico" );
            stringLocalizer.Setup( _ => _[key] ).Returns( localizedString );

            return stringLocalizer.Object;
        }
    }
}

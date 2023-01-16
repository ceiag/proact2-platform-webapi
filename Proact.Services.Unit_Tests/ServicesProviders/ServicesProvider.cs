using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using Proact.Services.Messages;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Proact.Services.ServicesProviders {
    public class ServicesProvider {
        private readonly Dictionary<Type, object> _queriesServices = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> _editorsServices = new Dictionary<Type, object>();
        private ProactDatabaseContext _database;

        public ProactDatabaseContext Database {
            get { return _database; }
        }

        public ServicesProvider() {
            CreateDatabase();
            InitQueriesServices();
            InitEditorsServices();

            AvatarConfiguration.Init( new Mock<IConfiguration>().Object );
        }

        public T GetQueriesService<T>() where T : IQueriesService {
            return (T)_queriesServices[typeof( T )];
        }

        public T GetEditorsService<T>() where T : IDataEditorService {
            return (T)_editorsServices[typeof(T)];
        }

        public void SaveChanges() {
            _database.SaveChanges();
        }

        private void InitQueriesServices() {
            _queriesServices[typeof( IUserQueriesService )]
                = new UserQueriesService( _database );
            _queriesServices[typeof( IMedicQueriesService )]
                = new MedicQueriesService( _database );
            _queriesServices[typeof( INurseQueriesService )]
                = new NurseQueriesService( _database );
            _queriesServices[typeof( IPatientQueriesService )]
                = new PatientQueriesService( _database );
            _queriesServices[typeof( IMedicalTeamQueriesService )]
                = new MedicalTeamQueriesService( _database );
            _queriesServices[typeof( IProjectQueriesService )]
                = new ProjectQueriesService( _database );
            _queriesServices[typeof( IProjectPropertiesQueriesService )]
                = new ProjectPropertiesQueriesService( _database );
            _queriesServices[typeof( IMessagesQueriesService )]
                = new MessagesQueriesService( _database );
            _queriesServices[typeof( ISurveyQuestionsQueriesService )]
                = new SurveyQuestionsQueriesService( _database );
            _queriesServices[typeof( ISurveyQueriesService )]
                = new SurveyQueriesService( _database );
            _queriesServices[typeof( ISurveyAssignationQueriesService )]
                = new SurveyAssignationQueriesService( _database );
            _queriesServices[typeof( ISurveyQuestionsSetQueriesService )]
                = new SurveyQuestionsSetQueriesService( _database );
            _queriesServices[typeof( IUserNotificationSettingsQueriesService )]
                = new NotificationSettingsQueriesService( _database );
            _queriesServices[typeof( IDeviceQueriesService )]
                = new DeviceQueriesService( _database );
            _queriesServices[typeof( ISurveyAnswersQueriesService )]
                = new SurveyAnswersQueriesService( _database );
            _queriesServices[typeof( ISurveyAnswersBlockQueriesService )]
                = new SurveyAnswersBlockQueriesService( _database );
            _queriesServices[typeof( ISurveyAnswerToQuestionQueriesService )]
                = new SurveyAnswerToQuestionQueriesService( _database );
            _queriesServices[typeof( ISurveySchedulerQueriesService )]
                = new SurveySchedulerQueriesService( _database );
            _editorsServices[typeof( IMessageEditorService )]
               = new MessageEditorService( _database );
            _queriesServices[typeof( ILexiconQueriesService )]
                = new LexiconQueriesService( _database );
            _queriesServices[typeof( IMessageAnalysisQueriesService )]
                = new MessageAnalysisQueriesService( _database );
        }

        private void InitEditorsServices() {
            var notificationProviderMock = new Mock<INotificationProviderService>();
            notificationProviderMock.Setup( 
                x => x.SendNewSurveyToCompileNotificationToUser(
                       new List<Guid>(), "" ) )
                            .Returns( Task.FromResult( new HttpResponseMessage() {
                                StatusCode = HttpStatusCode.OK,
                            } ) );

            _editorsServices[typeof( ISurveyQuestionsEditorService )] 
                = new SurveyQuestionsEditorService( 
                    GetQueriesService<ISurveyQuestionsQueriesService>(),
                    GetQueriesService<ISurveyAnswersQueriesService>(),
                    GetQueriesService<ISurveyQuestionsSetQueriesService>(),
                    GetQueriesService<ISurveyAnswersBlockQueriesService>() );

            _editorsServices[typeof( ISurveyAnswerToQuestionEditorService )]
                = new SurveyAnswerToQuestionEditorService(
                    GetQueriesService<ISurveyAnswerToQuestionQueriesService>(),
                    GetQueriesService<ISurveyAnswersQueriesService>(),
                    GetQueriesService<ISurveyAssignationQueriesService>(),
                    GetQueriesService<ISurveyQuestionsQueriesService>() );

            _editorsServices[typeof( ISurveySchedulerEditorService )]
               = new SurveySchedulerEditorService(
                   GetQueriesService<ISurveySchedulerQueriesService>(),
                   notificationProviderMock.Object,
                   GetQueriesService<ISurveyAssignationQueriesService>(),
                   GetQueriesService<IUserNotificationSettingsQueriesService>() );

            _editorsServices[typeof( IUserNotificationsSettingsEditorService )]
               = new UserNotificationsSettingsEditorService(
                   GetQueriesService<IUserNotificationSettingsQueriesService>(),
                   GetQueriesService<IDeviceQueriesService>() );

            _editorsServices[typeof( IProjectStateEditorService )]
               = new ProjectStateEditorService(
                   GetQueriesService<IProjectQueriesService>() );

            _editorsServices[typeof( IUsersCreatorQueriesService )]
               = new UsersCreatorQueriesService(
                   new Mock<IUserIdentityService>().Object,
                   new Mock<IGroupService>().Object,
                   GetQueriesService<IUserQueriesService>(),
                   GetQueriesService<IPatientQueriesService>(),
                   GetQueriesService<IMedicQueriesService>(),
                   GetQueriesService<INurseQueriesService>(),
                   _database );

            _editorsServices[typeof( IMessageFormatterService )]
               = new MessageFormatterService( 
                  GetQueriesService<IMessagesQueriesService>(),
                  new OrganizedMessagesProvider( new Mock<IStringLocalizer<Resource>>().Object ) );

            _editorsServices[typeof( IMessageEditorService )]
               = new MessageEditorService( _database );
        }

        private void CreateDatabase() {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ProactDatabaseContext>()
                .UseInMemoryDatabase( databaseName: "mockdb" )
                .UseInternalServiceProvider( serviceProvider );

            _database = new ProactDatabaseContext( builder.Options );

            _database.SaveChanges();
        }
    }
}

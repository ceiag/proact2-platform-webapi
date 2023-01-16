using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Proact.Services.Configurations;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Linq;
using Proact.Services.Messages;
using Proact.Services.AzureMediaServices;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Proact.Services.Services.EmailSender;
using Proact.Services.QueriesServices.Stats;
using Proact.Services.QueriesServices.Surveys.Stats;
using Proact.Services.QueriesServices.Surveys.Scheduler;
using Proact.Services.QueriesServices.DataManagers;
using Proact.Services.Exporters;

namespace Proact.Services {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void InitProactConfigurations() {
            AvatarConfiguration.Init( Configuration );
            AzureMediaServicesConfiguration.Init( Configuration );
            MediaFilesUploaderSettings.Init(Configuration );
            OneSignalConfiguration.Init( Configuration );
            EmailSenderConfiguration.Init( Configuration );
        }

        public void ConfigureServices( IServiceCollection services ) {
            AddAuhentication( services );
            AddAuthorization( services );
            AddLocalization( services );
            services.AddControllers();

            services.AddDbContext<ProactDatabaseContext>(
                options => options.UseSqlServer(
                Configuration.GetConnectionString( "DefaultConnection" ) )
                .UseLazyLoadingProxies() );

            services.AddScoped<IMessageFormatterService, MessageFormatterService>();
            services.AddScoped<IOrganizedMessagesProvider, OrganizedMessagesProvider>();
            services.AddScoped<IImageUploaderService, AzureImageUploaderService>();
            services.AddScoped<IAzureMediaEncryptionService, AzureMediaEncryptionService>();
            services.AddScoped<IContentKeyPolicyCreatorService, AES128ContentKeyPolicyCreatorService>();
            services.AddScoped<IMessageMediaFileCreatorService, MessageMediaFileCreatorService>();
            services.AddScoped<IFilesStorageService, AzureMediaStorageService>();
            services.AddScoped<IMedicQueriesService, MedicQueriesService>();
            services.AddScoped<IUserQueriesService, UserQueriesService>();
            services.AddScoped<INurseQueriesService, NurseQueriesService>();
            services.AddScoped<IDataManagerQueriesService, DataManagerQueriesService>();
            services.AddScoped<IResearcherQueriesService, ResearcherQueriesService>();
            services.AddScoped<IMessagesQueriesService, MessagesQueriesService>();
            services.AddScoped<IMedicalTeamQueriesService, MedicalTeamQueriesService>();
            services.AddScoped<IPatientQueriesService, PatientQueriesService>();
            services.AddScoped<IProjectQueriesService, ProjectQueriesService>();
            services.AddScoped<IProjectPropertiesQueriesService, ProjectPropertiesQueriesService>();
            services.AddScoped<IInstitutesQueriesService, InstitutesQueriesService>();
            services.AddScoped<IAvatarProviderService, AvatarProviderService>();
            services.AddScoped<IDeviceQueriesService, DeviceQueriesService>();
            services.AddScoped<INotificationProviderService, OneSignalProviderService>();
            services.AddScoped<INotificationTextProviderService, NotificationTextContentsProviderService>();
            services.AddScoped<IMessageNotifierService, MessageNotifierService>();
            services.AddScoped<IUserNotificationSettingsQueriesService, NotificationSettingsQueriesService>();
            services.AddScoped<ISurveyQuestionsQueriesService, SurveyQuestionsQueriesService>();
            services.AddScoped<ISurveyAnswersQueriesService, SurveyAnswersQueriesService>();
            services.AddScoped<ISurveyQueriesService, SurveyQueriesService>();
            services.AddScoped<ISurveyAssignationQueriesService, SurveyAssignationQueriesService>();
            services.AddScoped<ISurveyAnswerToQuestionQueriesService, SurveyAnswerToQuestionQueriesService>();
            services.AddScoped<ISurveysStatsQueriesService, SurveysStatsQueriesService>();
            services.AddScoped<IChangesTrackingService, ChangesTrackingService>();
            services.AddScoped<ISurveyQuestionsSetQueriesService, SurveyQuestionsSetQueriesService>();
            services.AddScoped<ISurveyAnswersBlockQueriesService, SurveyAnswersBlockQueriesService>();
            services.AddScoped<ISurveySchedulerQueriesService, SurveySchedulerQueriesService>();
            services.AddScoped<IUsersCreatorQueriesService, UsersCreatorQueriesService>();
            services.AddScoped<ILexiconQueriesService, LexiconQueriesService>();
            services.AddScoped<IMessageAnalysisQueriesService, MessageAnalysisQueriesService>();
            services.AddScoped<ILexiconCategoriesQueriesService, LexiconCategoriesQueriesService>();
            services.AddScoped<ILexiconLabelQueriesService, LexiconLabelQueriesService>();
            services.AddScoped<IDocumentsQueriesService, DocumentsQueriesService>();
            services.AddScoped<IDocumentsStorageService, DocumentsStorageService>();
            services.AddScoped<IProtocolStorageService, ProtocolStorageService>();
            services.AddScoped<IProtocolQueriesService, ProtocolQueriesService>();
            services.AddScoped<IProjectHtmlContentsQueriesService, ProjectHtmlContentsQueriesService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddScoped<IProjectStateEditorService, ProjectStateEditorService>();
            services.AddScoped<IMessageEditorService, MessageEditorService>();
            services.AddScoped<ISurveyQuestionsEditorService, SurveyQuestionsEditorService>();
            services.AddScoped<ISurveyAnswerToQuestionEditorService, SurveyAnswerToQuestionEditorService>();
            services.AddScoped<IUserNotificationsSettingsEditorService, UserNotificationsSettingsEditorService>();
            services.AddScoped<IProactSystemInitializerService, ProactSystemInitializerService>();
            //services.AddScoped<IMediaFilesUploaderService, AzureMediaBlobContainerUploaderService>();
            services.AddScoped<IMediaFilesUploaderService, AzureMediaServiceUploaderService>();
            services.AddScoped<IMessageAttachmentManagerService, MessageAttachmentManagerService>();
            services.AddScoped<IMobileAppsInfoQueriesService, MobileAppsInfoQueriesService>();
            services.AddScoped<IMessagesStatsProviderService, MessagesStatsProviderService>();
            services.AddScoped<ISurveyAssignationQueriesService, SurveyAssignationQueriesService>();
            services.AddScoped<ISurveySchedulerDispatcherService, SurveySchedulerDispatcherService>();
            services.AddScoped<ISurveyStatsOverTimeQueriesService, SurveyStatsOverTimeQueriesService>();
            services.AddScoped<IProactDataExporterService, ProactDataExporterService>();

            services.AddScoped<ConsistencyRulesHelper, ConsistencyRulesHelper>();

            AddSwagger( services );

            services.AddHangfire( configuration => configuration
             .SetDataCompatibilityLevel( CompatibilityLevel.Version_170 )
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseConsole()
             .UseSqlServerStorage(
                    Configuration.GetConnectionString( "DefaultConnection" ),
                    new SqlServerStorageOptions {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes( 5 ),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes( 5 ),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    } ) );

            services.AddHangfireServer();

            services.Configure<AzureB2CSettingsModel>(
                Configuration.GetSection( "AzureB2CUserGraph" ) );

            services.AddCors( options => {
                options.AddPolicy(
                "CorsPolicy",
                 builder => builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader() );
            } );

            services.AddScoped<IUserIdentityService, AzureB2CUserIdentityService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IAuthorizationHandler, RolesHandler>();
            services.AddAzureClients( builder => {
                builder.AddBlobServiceClient( Configuration["ProactServicesLogsConnectionString:blob"], preferMsi: true );
                builder.AddQueueServiceClient( Configuration["ProactServicesLogsConnectionString:queue"], preferMsi: true );
            } );
        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {
            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }

            UseSwagger( app );
            UserLocalization( app );

            app.UseCors( x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed( origin => true )
                .AllowCredentials() );

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors();

            app.UseHangfireDashboard( "/hangfire", new DashboardOptions {
                Authorization = new[] { new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new [] {
                            new BasicAuthAuthorizationUser {
                                Login = "admin",
                                PasswordClear =  "ProactPassword1"
                            }
                        }
                    }) 
                }
            } );

            app.UseEndpoints( endpoints => {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute( 
                    "default", 
                    "api/{culture:culture}/{controller=Home}/{action=Index}/{id?}" );
            } );

            InitProactConfigurations();
        }

        private void AddAuhentication( IServiceCollection services ) {
            services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
              .AddMicrosoftIdentityWebApi( Configuration.GetSection( "AzureAdB2C" ) );
        }

        private void AddAuthorization( IServiceCollection services ) {
            services.AddAuthorization( options => {
                foreach ( string policyKey in Policies.RolesAssociatedToPolicies.Keys ) {

                    options.AddPolicy( policyKey,
                      policy => policy.Requirements
                      .Add( new RolesRequirement( Policies.RolesAssociatedToPolicies[policyKey] ) ) );
                }
            } );
        }

        private void AddSwagger( IServiceCollection services ) {
            services.AddSwaggerGen( c => {
                c.SwaggerDoc( "v1", new OpenApiInfo {
                    Title = Configuration["Swagger:ApiName"],
                    Version = Configuration["Swagger:ApiVer"]
                } );

                c.UseOneOfForPolymorphism();

                var filePath = Path.Combine( AppContext.BaseDirectory, "Proact.Services.xml" );
                c.IncludeXmlComments( filePath );

                var authority = string.Format(
                    Configuration["Swagger:AuthUrl"],
                    Configuration["Swagger:SignUpSignInPolicyId"] );

                c.AddSecurityDefinition( "oauth2", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows {
                        Implicit = new OpenApiOAuthFlow {

                            AuthorizationUrl = new Uri( authority, UriKind.Absolute ),
                            Scopes = new Dictionary<string, string>{
                              {  Configuration["Swagger:Scope"],"access_as_user" }
                            }
                        }
                    }
                } );

                c.AddSecurityRequirement( new OpenApiSecurityRequirement(){ {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "oauth2",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                } );
            } );
        }

        private void UseSwagger( IApplicationBuilder app ) {
            app.UseSwagger();
            app.UseSwaggerUI(
                c => {
                    c.SwaggerEndpoint( "/swagger/v1/swagger.json",
                        Configuration["Swagger:ApiName"]
                        + Configuration["Swagger:ApiVer"] );

                    c.OAuthClientId( Configuration["Swagger:ClientId"] );
                }
            );
        }

        private void AddLocalization( IServiceCollection services ) {

            var routeParameter = Configuration["Localization:RouteParameter"];
            var defaultCulture = Configuration["Localization:DefaultCulture"];
            var supportedCultures = Configuration
                .GetSection( "Localization" )
                .GetSection( "SupportedCultures" )
                .GetChildren()
                .Select( x => x.Value )
                .ToArray();

            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(
                options => {
                    var supportedCulturesInfo = new List<CultureInfo>();
                    foreach ( var culture in supportedCultures) {
                        supportedCulturesInfo.Add( new CultureInfo( culture ) );
                    }

                    options.DefaultRequestCulture
                        = new RequestCulture( defaultCulture, defaultCulture );

                    options.SupportedCultures = supportedCulturesInfo;
                    options.SupportedUICultures = supportedCulturesInfo;
                    options.RequestCultureProviders = new[] {
                        new RouteDataRequestCultureProvider {
                            IndexOfCulture = 2,
                            IndexofUICulture = 2
                        }
                    };
                } );

            services.Configure<RouteOptions>( options => {
                options.ConstraintMap.Add( routeParameter, typeof( LanguageRouteConstraint ) );
            } );
        }

        private void UserLocalization( IApplicationBuilder app ) {
            var localizeOptions
                = app.ApplicationServices
                .GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization( localizeOptions.Value );
        }
    }
    internal static class StartupExtensions {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient( this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi ) {
            if ( preferMsi && Uri.TryCreate( serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri ) ) {
                return builder.AddBlobServiceClient( serviceUri );
            }
            else {
                return builder.AddBlobServiceClient( serviceUriOrConnectionString );
            }
        }
        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient( this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi ) {
            if ( preferMsi && Uri.TryCreate( serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri ) ) {
                return builder.AddQueueServiceClient( serviceUri );
            }
            else {
                return builder.AddQueueServiceClient( serviceUriOrConnectionString );
            }
        }
    }
}

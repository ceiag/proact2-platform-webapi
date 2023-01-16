using Microsoft.Extensions.Localization;
using Proact.Services.Configurations;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proact.Services.Services.EmailSender {
    public class EmailSenderService : IEmailSenderService {
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private readonly IProjectHtmlContentsQueriesService _projectHtmlContentsQueriesService;
        private readonly IUserIdentityService _userIdentityService;

        public EmailSenderService( 
            IStringLocalizer<Resource> stringLocalizer,
            IProjectHtmlContentsQueriesService projectHtmlContentsQueriesService,
            IUserIdentityService userIdentityService ) {
            _stringLocalizer = stringLocalizer;
            _projectHtmlContentsQueriesService = projectHtmlContentsQueriesService;
            _userIdentityService = userIdentityService;
        }

        public async Task SendEmail( string to, string subject, string body ) {
            var client = new HttpClient();
            var jsonData = JsonSerializer.Serialize( new {
                email = to,
                subject = subject,
                body = body
            } );

            await client.PostAsync( EmailSenderConfiguration.RequestUrl, 
               new StringContent( jsonData, Encoding.UTF8, "application/json" ) );
        }

        public async Task SendWelcomeEmailTo( Guid projectId, Patient patient ) {
            var subject = _stringLocalizer["email_subject_user_welcome"].Value;
            var body = _projectHtmlContentsQueriesService
                .GetByProjectId( projectId, ProjectHtmlType.UserWelcomeEmail );

            var userEmail = await _userIdentityService.GetUserEmail( patient.User.AccountId );
            if ( !string.IsNullOrEmpty( userEmail ) && body is not null ) {
                await SendEmail( userEmail, subject, body.HtmlContent );
            }
        }

        public async Task SendSuspendedEmailTo( Guid projectId, Patient patient ) {
            var subject = _stringLocalizer["email_subject_user_suspended"].Value;
            var body = _projectHtmlContentsQueriesService
                .GetByProjectId( projectId, ProjectHtmlType.UserSuspendedEmail );

            var userEmail = await _userIdentityService.GetUserEmail( patient.User.AccountId );
            await SendEmail( userEmail, subject, body.HtmlContent );
        }

        public async Task SendDeactivatedEmailTo( Guid projectId, Patient patient ) {
            var subject = _stringLocalizer["email_subject_user_deactivated"].Value;
            var body = _projectHtmlContentsQueriesService
                .GetByProjectId( projectId, ProjectHtmlType.UserSuspendedEmail );

            var userEmail = await _userIdentityService.GetUserEmail( patient.User.AccountId );
            await SendEmail( userEmail, subject, body.HtmlContent );
        }
    }
}

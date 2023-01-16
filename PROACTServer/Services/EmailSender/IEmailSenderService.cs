using Proact.Services.Entities;
using System;
using System.Threading.Tasks;

namespace Proact.Services.Services.EmailSender {
    public interface IEmailSenderService {
        public Task SendEmail( string to, string subject, string body );
        public Task SendWelcomeEmailTo( Guid projectId, Patient patient );
        public Task SendSuspendedEmailTo( Guid projectId, Patient patient );
        public Task SendDeactivatedEmailTo( Guid projectId, Patient patient );
    }
}

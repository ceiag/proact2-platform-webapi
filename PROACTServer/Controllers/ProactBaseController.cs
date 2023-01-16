using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Linq;
using System.Security.Claims;

namespace Proact.Services.Controllers {
    public abstract class ProactBaseController : Controller {
        private readonly ConsistencyRulesHelper _consistencyRulesHelper;
        private readonly IChangesTrackingService _changesTrackingService;

        public ProactBaseController( 
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper ) {
            _changesTrackingService = changesTrackingService;
            _consistencyRulesHelper = consistencyRulesHelper;
        }

        protected ConsistencyRulesHelper RulesHelper {
            get { return _consistencyRulesHelper; }
        }

        protected User GetCurrentUser() {
            var accountId = HttpContext.User.FindFirst( ClaimTypes.NameIdentifier ).Value;

            return _consistencyRulesHelper
                .GetQueriesService<IUserQueriesService>().GetByAccountId( accountId );
        }

        protected Institute GetCurrentInstitute() {
            return GetCurrentUser().Institute;
        }

        protected Guid GetInstituteId() {
            return GetCurrentInstitute().Id;
        }

        protected bool HasRoleOf( string role ) {
            return HttpContext.User.Claims.FirstOrDefault( x => x.Value == role ) != null;
        }

        protected UserRoles GetCurrentUserRoles() { 
            return new UserRoles( HttpContext.User.Claims.Select( x => x.Value ).ToList() );
        }

        protected void SaveChanges() {
            _changesTrackingService.SaveChanges( GetCurrentUser().AccountId.ToString() );
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Proact.UnitTests.Helpers {
    public static class HttpContextMocker {
        public static void MockHttpContext( Controller controller, User user, string role ) {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var claims = new List<Claim>() {
                    new Claim( ClaimTypes.NameIdentifier, user.AccountId ),
                    new Claim( Roles.ClaimTypeRoles, role )
                };

            var identity = new ClaimsIdentity( claims, "TestAuthType" );
            var claimsPrincipal = new ClaimsPrincipal( identity );

            controller.ControllerContext.HttpContext.User
                = new ClaimsPrincipal( claimsPrincipal ) { };
        }
    }
}

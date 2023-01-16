using System;
namespace Proact.Services {
    public class ProactRouteConfiguration {

#if DEBUG
        public const string DefaultRoute = "api/en-US/[controller]";

#else
        public const string DefaultRoute = "api/{culture:culture}/[controller]";
#endif

    }
}

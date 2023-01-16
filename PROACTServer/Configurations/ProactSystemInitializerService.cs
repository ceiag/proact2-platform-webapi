using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.Services.Configurations {
    public class ProactSystemInitializerService : IProactSystemInitializerService {
        private readonly ProactDatabaseContext _database;
        private readonly IGroupService _groupService;
        private readonly IUserIdentityService _identityUserService;
        private readonly IUserQueriesService _usersQueriesService;

        public ProactSystemInitializerService( 
            ProactDatabaseContext database, IGroupService groupService,
            IUserIdentityService identityUserService, IUserQueriesService usersQueriesService ) {
            _database = database;
            _groupService = groupService;
            _identityUserService = identityUserService;
            _usersQueriesService = usersQueriesService;
        }

        public async Task Initialize( SystemInitializationRequest request ) {
            if ( !SystemAlreadyInitialized() ) {
                await CreateRoles();
                await CreateSystemAdmin( request );
                CreateSystemInfoRow();
                SetAsInitialized();
            }
        }

        private async Task CreateRoles() {
            foreach ( var roleName in Roles.AllRoles ) {
                await _groupService.CreateSecurityGroup( roleName );
            }
        }

        private async Task CreateSystemAdmin( SystemInitializationRequest request ) {
            var user = await _identityUserService.Create( 
                request.FirstName, request.LastName, request.Email );
            var systemAdminGroupId = await _groupService.GetGroupIdByName( Roles.SystemAdmin );

            User userAdmin = new User() {
                Id = Guid.NewGuid(),
                AccountId = user.AccountId,
                Name = request.FirstName + " " + request.LastName,
                State = UserSubscriptionState.Active,
                AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl
            };

            _usersQueriesService.Create( userAdmin );
            _database.SaveChanges();

            await _groupService.AddMember( systemAdminGroupId, user.AccountId );
        }

        public bool SystemAlreadyInitialized() {
            if ( _database.ProactSystemInfo.Count() == 0 ) {
                return false;
            }

            return _database.ProactSystemInfo.First().SystemInitialized;
        }

        public void SetAsInitialized() {
            _database.ProactSystemInfo.First().SystemInitialized = true;
            _database.SaveChanges();
        }

        private void CreateSystemInfoRow() {
            if ( _database.ProactSystemInfo.Count() == 0 ) {
                var proactSystemInfo = new ProactSystemInfo {
                    SystemInitialized = false
                };

                _database.ProactSystemInfo.Add( proactSystemInfo );
                _database.SaveChanges();
            }
        }
    }
}

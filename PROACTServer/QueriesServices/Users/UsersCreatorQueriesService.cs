using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.EntitiesMapper.DataManagers;
using Proact.Services.Models;
using Proact.Services.Models.DataManagers;
using Proact.Services.QueriesServices.DataManagers;
using Proact.Services.Services;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices
{
    public class UsersCreatorQueriesService : IUsersCreatorQueriesService {
        private readonly IUserIdentityService _userIdentityService;
        private readonly IGroupService _groupService;
        private readonly IUserQueriesService _usersQueriesService;
        private readonly IPatientQueriesService _patientQueriesService;
        private readonly IMedicQueriesService _medicQueriesService;
        private readonly INurseQueriesService _nurseQueriesService;
        private readonly IResearcherQueriesService _researcherQueriesService;
        private readonly IDataManagerQueriesService _datamanagerQueriesService;
        private readonly ProactDatabaseContext _database;

        public UsersCreatorQueriesService( 
            IUserIdentityService userIdentityService, 
            IGroupService groupService, 
            IUserQueriesService userQueriesService, 
            IPatientQueriesService patientQueriesService,
            IMedicQueriesService medicQueriesService, 
            INurseQueriesService nurseQueriesService,
            IResearcherQueriesService researcherQueriesService,
            IDataManagerQueriesService datamanagerQueriesService,
            ProactDatabaseContext database ) {
            _userIdentityService = userIdentityService;
            _groupService = groupService;
            _usersQueriesService = userQueriesService;
            _patientQueriesService = patientQueriesService;
            _medicQueriesService = medicQueriesService;
            _nurseQueriesService = nurseQueriesService;
            _researcherQueriesService = researcherQueriesService;
            _datamanagerQueriesService = datamanagerQueriesService;
            _database = database;
        }

        public async Task<PatientModel> CreatePatient( Guid instituteId, PatientCreateRequest request ) {
            var basicUserModel = await CreateBasicUser( 
                instituteId, request.FirstName, request.Lastname, request.Email );

            try {
                var user = _usersQueriesService.Get( basicUserModel.UserId );
                user.AvatarUrl = AvatarConfiguration.PatientAvatarDefaultUrl;

                _database.SaveChanges();

                var patient = _patientQueriesService.Create( user, request );
                await AddUserToGroupRoles( Roles.Patient, user.AccountId );

                _database.SaveChanges();

                return PatientEntityMapper.Map( patient, false );
            }
            catch ( Exception ex ) {
                await _userIdentityService.Delete( basicUserModel.AccountId );
                throw new Exception( ex.Message );
            }
        }

        public async Task<MedicModel> CreateMedic( Guid instituteId, CreateMedicRequest request ) {
            var basicUserModel = await CreateBasicUser( 
                instituteId, request.FirstName, request.Lastname, request.Email );

            try {
                var user = _usersQueriesService.Get( basicUserModel.UserId );
                user.AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl;

                _database.SaveChanges();

                var medic = _medicQueriesService.Create( user.Id );
                await AddUserToGroupRoles( Roles.MedicalProfessional, user.AccountId );

                _database.SaveChanges();

                return MedicEntityMapper.Map( medic );
            }
            catch ( Exception ex ) {
                await _userIdentityService.Delete( basicUserModel.AccountId );
                throw new Exception( ex.Message );
            }
        }

        public async Task<NurseModel> CreateNurse( Guid instituteId, CreateNurseRequest request ) {
            var basicUserModel = await CreateBasicUser( 
                instituteId, request.FirstName, request.Lastname, request.Email );

            try {
                var user = _usersQueriesService.Get( basicUserModel.UserId );
                user.AvatarUrl = AvatarConfiguration.NurseAvatarDefaultUrl;

                _database.SaveChanges();

                var nurse = _nurseQueriesService.Create( user.Id );
                await AddUserToGroupRoles( Roles.Nurse, user.AccountId );

                _database.SaveChanges();
                return NurseEntityMapper.Map( nurse );
            }
            catch ( Exception ex ) {
                await _userIdentityService.Delete( basicUserModel.AccountId );
                throw new Exception( ex.Message );
            }
        }

        public async Task<ResearcherModel> CreateResearcher( 
            Guid instituteId, CreateResearcherRequest request ) {
            var basicUserModel = await CreateBasicUser( 
                instituteId, request.FirstName, request.Lastname, request.Email );

            try {
                var user = _usersQueriesService.Get( basicUserModel.UserId );
                user.AvatarUrl = AvatarConfiguration.ResearcherAvatarDefaultUrl;

                _database.SaveChanges();

                var reseacher = _researcherQueriesService.Create( user.Id );
                await AddUserToGroupRoles( Roles.Researcher, user.AccountId );

                _database.SaveChanges();
                return ResearcherEntityMapper.Map( reseacher );
            }
            catch ( Exception ex ) {
                await _userIdentityService.Delete( basicUserModel.AccountId );
                throw new Exception( ex.Message );
            }
        }

        public async Task<DataManagerModel> CreateDataManager(
            Guid instituteId, CreateDataManagerRequest request ) {
            var basicUserModel = await CreateBasicUser(
                instituteId, request.FirstName, request.Lastname, request.Email );

            try {
                var user = _usersQueriesService.Get( basicUserModel.UserId );
                user.AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl;

                _database.SaveChanges();

                var dataManager = _datamanagerQueriesService.Create( user.Id );
                await AddUserToGroupRoles( Roles.MedicalTeamDataManager, user.AccountId );

                _database.SaveChanges();
                return DataManagerEntityMapper.Map( dataManager );
            }
            catch ( Exception ex ) {
                await _userIdentityService.Delete( basicUserModel.AccountId );
                throw new Exception( ex.Message );
            }
        }

        private async Task AddUserToGroupRoles( string role, string accountId ) {
            var groupId = await _groupService.GetGroupIdByName( role );
            await _groupService.AddMember( groupId, accountId );
        }

        private async Task<bool> EmailAlreadyExist( string email ) {
            return await _userIdentityService.GetByEmail( email ) != null;
        }

        private bool AccountIdAlreadyExist( string accountId ) {
            return _usersQueriesService.GetByAccountId( accountId ) != null;
        }

        private async Task<UserModel> CreateBasicUserOnAzureAD(
            string firstName, string lastName, string email ) {
            var newUser = new UserModel();

            if ( !await EmailAlreadyExist( email ) ) {
                newUser = await _userIdentityService.Create( firstName, lastName, email );
            }
            else {
                newUser = await _userIdentityService.GetByEmail( email );
            }

            if ( AccountIdAlreadyExist( newUser.AccountId ) ) {
                throw new Exception( $"User with AccountId {newUser.AccountId} already exists" );
            }

            return newUser;
        }

        public async Task<UserModel> CreateBasicUser( 
            Guid instituteId, string firstName, string lastName, string email ) {
            try {
                var createdUser = await CreateBasicUserOnAzureAD( firstName, lastName, email );

                User user = new User() {
                    Id = Guid.NewGuid(),
                    InstituteId = instituteId,
                    AccountId = createdUser.AccountId,
                    Name = firstName + " " + lastName,
                    State = UserSubscriptionState.Active,
                    AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl
                };

                var userCreated = _usersQueriesService.Create( user );

                _database.SaveChanges();
                return UserEntityMapper.Map( userCreated );
            }
            catch ( Exception ex ) {
                throw new Exception( ex.Message );
            }
        }
    }
}

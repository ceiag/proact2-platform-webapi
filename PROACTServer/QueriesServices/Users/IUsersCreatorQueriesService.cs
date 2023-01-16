using Proact.Services.Models;
using Proact.Services.Models.DataManagers;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public interface IUsersCreatorQueriesService : IDataEditorService {
        public Task<PatientModel> CreatePatient( Guid instituteId, PatientCreateRequest request );
        public Task<MedicModel> CreateMedic( Guid instituteId, CreateMedicRequest request );
        public Task<NurseModel> CreateNurse( Guid instituteId, CreateNurseRequest request );
        public Task<ResearcherModel> CreateResearcher( Guid instituteId, CreateResearcherRequest request );
        public Task<DataManagerModel> CreateDataManager( Guid instituteId, CreateDataManagerRequest request );
        public Task<UserModel> CreateBasicUser(
            Guid instituteId, string firstName, string lastName, string email );
    }
}

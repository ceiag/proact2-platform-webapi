using System.Threading.Tasks;
using Proact.Services.Models;

namespace Proact.Services {
    public interface IUserIdentityService {
        public Task<UserModel> Create( string firstName, string lastName, string email );
        public Task<UserModel> GetByAccountId( string accountId );
        public Task<UserModel> GetByEmail( string email );
        public Task<bool> Delete( string accountId );
        public Task<bool> Suspend( string accountId );
        public Task<bool> Activate( string accountId );
        public Task<string> GetUserEmail( string accountId );
    }
}

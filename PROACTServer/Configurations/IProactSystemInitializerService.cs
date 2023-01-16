using Proact.Services.Models;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IProactSystemInitializerService {
        public Task Initialize( SystemInitializationRequest request );
        public bool SystemAlreadyInitialized();
    }
}

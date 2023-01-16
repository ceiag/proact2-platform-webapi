using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services {
    public class MedicModel : UserModel {
        public List<MedicalTeamModel> MedicalTeams { get; set; }
    }
}

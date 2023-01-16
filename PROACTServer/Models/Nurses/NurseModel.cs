using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services {
    public class NurseModel : UserModel {
        public List<MedicalTeamModel> MedicalTeams { get; set; }
    }
}

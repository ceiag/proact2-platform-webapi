using System.Collections.Generic;

namespace Proact.Services.Models {
    public class ResearcherModel : UserModel {
        public List<MedicalTeamModel> MedicalTeams { get; set; }
    }
}

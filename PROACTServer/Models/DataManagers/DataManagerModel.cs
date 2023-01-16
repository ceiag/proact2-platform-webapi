using System.Collections.Generic;

namespace Proact.Services.Models.DataManagers;

public class DataManagerModel : UserModel {
    public List<MedicalTeamModel> MedicalTeams { get; set; }
}

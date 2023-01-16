using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class PatientModel : UserModel {
        public PatientModel() {
            MedicalTeam = new List<MedicalTeamModel>();
        }

        public new string Name { 
            get => string.IsNullOrEmpty( base.Name ) ? Code : base.Name;
            set => base.Name = value;
        }

        public string Code { get; set; }
        public int BirthYear { get; set; }
        public string Gender { get; set; }
        public DateTime? TreatmentStartDate { get; set; }
        public DateTime? TreatmentEndDate { get; set; }
        public List<MedicalTeamModel> MedicalTeam { get; set; }
    }
}

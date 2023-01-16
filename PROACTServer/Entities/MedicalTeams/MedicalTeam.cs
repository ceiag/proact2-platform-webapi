using Proact.Services.Entities.MedicalTeams;
using Proact.Services.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Proact.Services.Entities {
    public class MedicalTeam : TrackableEntity, IEntity {
        public MedicalTeam() {
            MedicsRelation = new List<MedicsMedicalTeamRelation>();
            NursesRelation = new List<NursesMedicalTeamRelation>();
            DataManagersRelation = new List<DataManagersMedicalTeamRelation>();
        }

        public string Name { get; set; }
        public virtual ICollection<MedicAdmin> Admins { get; set; }
        public bool Enabled { get; set; } = true;
        public string Phone { get; set; } = String.Empty;
        public string AddressLine1 { get; set; } = String.Empty;
        public string AddressLine2 { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public string StateOrProvince { get; set; } = String.Empty;
        public string RegionCode { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public string TimeZone { get; set; } = String.Empty;
        public MedicalTeamState State { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<MedicsMedicalTeamRelation> MedicsRelation { get; set; }
        public virtual ICollection<NursesMedicalTeamRelation> NursesRelation { get; set; }
        public virtual ICollection<ResearchersMedicalTeamRelation> ResearcherRelation { get; set; }
        public virtual ICollection<DataManagersMedicalTeamRelation> DataManagersRelation { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        
        [NotMapped]
        public List<Medic> Medics {
            get {
                return MedicsRelation.Select( x => x.Medic ).ToList();
            }
        }

        [NotMapped]
        public List<Nurse> Nurses {
            get {
                return NursesRelation.Select( x => x.Nurse ).ToList();
            }
        }

        [NotMapped]
        public List<Researcher> Reseachers {
            get {
                return ResearcherRelation.Select( x => x.Researcher ).ToList();
            }
        }

        [NotMapped]
        public List<DataManager> DataManagers {
            get {
                return DataManagersRelation.Select( x => x.DataManager ).ToList();
            }
        }
    }
}

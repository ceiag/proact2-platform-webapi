using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class InstituteModel {
        public Guid Id { get; set; }
        public InstituteState State { get; set; }
        public string Name { get; set; }
        public List<UserModel> Admins { get; set; }
        public InstitutePropertiesModel Properties { get; set; }
    }
}

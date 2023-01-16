using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class UserModel {
        [Required]
        public Guid UserId { get; set; }
        public Guid InstituteId { get; set; }
        [Required]
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Title { get; set; }
        public string AccountId { get; set; }
        [Required]
        public UserSubscriptionState State { get; set; }
        public List<string> Roles { get; set; }
    }
}

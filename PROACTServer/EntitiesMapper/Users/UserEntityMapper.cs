using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class UserEntityMapper {
        private static Guid GetInstituteFromNullableId( User user ) {
            Guid instituteId = Guid.Empty;

            if ( user.InstituteId != null ) {
                instituteId = (Guid)user.InstituteId;
            }

            return instituteId;
        }

        public static UserModel Map( User user ) {
            return new UserModel() {
                UserId = user.Id,
                InstituteId = GetInstituteFromNullableId( user ),
                AccountId = user.AccountId,
                AvatarUrl = user.AvatarUrl,
                Name = user.Name,
                Title = user.Title
            };
        }

        public static List<UserModel> Map( List<User> users ) {
            var userModels = new List<UserModel>();

            foreach ( var user in users ) {
                userModels.Add( Map( user ) );
            }

            return userModels;
        }

        public static User Map( UserModel userModel ) {
            return new User() {
                Id = userModel.UserId,
                AccountId = userModel.AccountId,
                Name = userModel.Name,
                AvatarUrl = userModel.AvatarUrl,
                InstituteId = userModel.InstituteId,
                State = userModel.State,
            };
        }
    }
}

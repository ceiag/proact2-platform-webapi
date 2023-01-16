using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class UserQueriesService : IUserQueriesService {
        private ProactDatabaseContext _database;

        public UserQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public User Create( User user ) {
            return _database.Users.Add( user ).Entity;
        }

        public User Delete( Guid userId ) {
            return _database.Users.Remove( Get( userId ) ).Entity;
        }

        public User Get( Guid userId ) {
            return _database.Users
                .Include( x => x.NotificationSettings )
                .FirstOrDefault( x => x.Id == userId );
        }

        public User GetByAccountId( string accountId ) {
            return _database.Users
                .Include( x => x.NotificationSettings )
                .FirstOrDefault( x => x.AccountId == accountId );
        }

        public List<User> GetsAll() {
            return _database.Users.ToList();
        }

        public void SetAvatarUrl( Guid userId, string avatarUrl ) {
            Get( userId ).AvatarUrl = avatarUrl;
        }
    }
}

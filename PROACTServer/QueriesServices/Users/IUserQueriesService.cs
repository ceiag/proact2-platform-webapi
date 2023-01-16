using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IUserQueriesService : IQueriesService {
        public User Create( User user );
        public User Get( Guid userId );
        public User Delete( Guid userId );
        public List<User> GetsAll();
        public User GetByAccountId( string accountId );
        public void SetAvatarUrl( Guid userId, string avatarUrl );
    }
}

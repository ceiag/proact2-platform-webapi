using Proact.Services.Entities;
using System;

namespace Proact.Services.Users {
    public class UserNameFormatter {
        private User _user;

        public UserNameFormatter( User user ) {
            this._user = user;
        }

        public static string Format( string displayName, string firstName, string lastName ) {
            if ( !string.IsNullOrWhiteSpace( displayName ) ) {
                return displayName;
            }

            if ( !string.IsNullOrWhiteSpace( firstName )
                && !string.IsNullOrWhiteSpace( lastName ) ) {
                return firstName + " " + lastName;
            }

            return !string.IsNullOrWhiteSpace( lastName ) ? lastName : firstName;
        }

        public string Format() {
            var name = !string.IsNullOrWhiteSpace(_user.Name) 
                ? _user.Name : GetNormilizedAccountId();

            if ( !string.IsNullOrWhiteSpace( _user.Title ) ) {
                name += ", " + _user.Title;
            }

            return name;
        }

        private string GetNormilizedAccountId() {
            var accountId = _user.AccountId;

            var signAtIndex = accountId.IndexOf('@');

            return signAtIndex > 0 ? accountId.Substring( 0, signAtIndex ) : accountId;
        }
    }
}

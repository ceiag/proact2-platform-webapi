using Proact.Services.Entities;
using Proact.Services.Models;
using Xunit;

namespace Proact.Comparators {
    public static class PatientEqual {
        public static void AssertEqual( Patient expected, PatientModel current ) {
            if ( expected == null || current == null ) {
                Assert.Null( expected );
                Assert.Null( current );
            }
            else {
                Assert.Equal( expected.UserId, current.UserId );
                Assert.Equal( expected.User.AvatarUrl, current.AvatarUrl );
                Assert.Equal( expected.User.AccountId, current.AccountId );
                Assert.Equal( expected.Gender, current.Gender );
                Assert.Equal( expected.User.Name, current.Name );
                Assert.Equal( expected.User.State, current.State );
                Assert.Equal( expected.User.Title, current.Title );
                Assert.Equal( expected.TreatmentStartDate, current.TreatmentStartDate );
                
                MedicalTeamEqual.AssertEqual( expected.MedicalTeam, current.MedicalTeam[0] );
            }
        }
    }
}

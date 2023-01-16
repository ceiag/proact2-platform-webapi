using Proact.Services.Entities;
using Proact.Services.Models;
using Xunit;

namespace Proact.Comparators {
    public static class MedicalTeamEqual {
        public static void AssertEqual( MedicalTeam expected, MedicalTeamModel current ) {
            if ( expected == null || current == null ) {
                Assert.Null( expected );
                Assert.Null( current );
            }
            else {
                Assert.Equal( expected.Id, current.MedicalTeamId );
                Assert.Equal( expected.Name, current.Name );
                Assert.Equal( expected.AddressLine1, current.AddressLine1 );
                Assert.Equal( expected.AddressLine2, current.AddressLine2 );
                Assert.Equal( expected.City, current.City );
                Assert.Equal( expected.Country, current.Country );
                Assert.Equal( expected.Phone, current.Phone );
                Assert.Equal( expected.PostalCode, current.PostalCode );
                Assert.Equal( expected.RegionCode, current.RegionCode );
                Assert.Equal( expected.StateOrProvince, current.StateOrProvince );
                Assert.Equal( expected.TimeZone, current.TimeZone );

                ProjectEqual.AssertEqual( expected.Project, current.Project );
            }
        }
    }
}

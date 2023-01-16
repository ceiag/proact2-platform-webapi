using System;

namespace Proact.Services {
    public static class GuidExtension {
        public static string ToReadable( this Guid guid ) {
            return Convert.ToBase64String( guid.ToByteArray() ).Substring( 0, 5 ).ToUpper();
        }
    }
}

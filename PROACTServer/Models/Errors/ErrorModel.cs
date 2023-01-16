namespace Proact.Services.Models {
    public class ErrorModel {
        /// <summary>
        /// The error message in plain text
        /// </summary>
        /// <example>This is an error message!</example>
        public readonly string Message;

        /// <summary>
        /// The error code
        /// </summary>
        public readonly ErrorCode Code;

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorModel( ErrorCode code, string message ) {
            this.Message = message;
            this.Code = code;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models.Messages {
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
    public class RecordedTimeLessThanNowAttribute : ValidationAttribute {
        private static readonly TimeSpan UploadTimeTolerance = TimeSpan.FromMinutes(5);

        //private ILog logger = LogManager.GetLogger(typeof(RecordedTimeLessThanNowAttribute));

        public RecordedTimeLessThanNowAttribute() : base( "RecordedTime must not be in future" ) {
        }

        protected override ValidationResult IsValid( object value, ValidationContext validationContext ) {
            var recordedTime = value as DateTime?;

            var now = DateTime.UtcNow.Add(UploadTimeTolerance);
            if ( recordedTime != null && recordedTime > now ) {
                //logger.WarnFormat( "message is not valid. RecordedTime '{0}' > UploadedTime (+{1} tolerance) '{2}'", recordedTime, UploadTimeTolerance, now );
                return new ValidationResult( "RecordedTime must not be in future" );
            }

            return ValidationResult.Success;
        }
    }
}

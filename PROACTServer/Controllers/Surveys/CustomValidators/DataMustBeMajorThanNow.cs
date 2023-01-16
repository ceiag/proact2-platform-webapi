using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services {
    public class DataMustBeMajorThanNow : ValidationAttribute {
        public override bool IsValid( object date ) {
            var surveyDate = (DateTime)date;

            return surveyDate.AddHours( 1 ) >= DateTime.Today.ToUniversalTime();
        }
    }
}

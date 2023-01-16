using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proact.Services.Models {
    public class SurveyQuestionPropertiesJsonConverter : JsonConverter<ISurveyQuestionProperties> {
        public override ISurveyQuestionProperties Read( 
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            if ( typeToConvert == typeof( SurveyMinMaxQuestionProperties ) ) {
                return JsonSerializer.Deserialize<SurveyMinMaxQuestionProperties>( reader.GetString() );
            }

            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, ISurveyQuestionProperties value, JsonSerializerOptions options ) {

            if ( value is SurveyMinMaxQuestionProperties ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyMinMaxQuestionProperties, 
                    typeof( SurveyMinMaxQuestionProperties ), options );
            }
            else if ( value is SurveyOpenQuestionProperties ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyOpenQuestionProperties, 
                    typeof( SurveyOpenQuestionProperties ), options );
            }
            else if ( value is SurveySingleChoiceQuestionProperties ) {
                JsonSerializer.Serialize(
                    writer, value as SurveySingleChoiceQuestionProperties, 
                    typeof( SurveySingleChoiceQuestionProperties ), options );
            }
            else if ( value is SurveyNoQuestionProperties ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyNoQuestionProperties, 
                    typeof( SurveyNoQuestionProperties ), options );
            }
            else {
                throw new ArgumentOutOfRangeException(
                    nameof( value ), $"Unknown implementation of the interface " +
                        $"{nameof( ISurveyQuestionProperties )} for the parameter {nameof( value )}. " +
                        $"Unknown implementation: {value?.GetType().Name}" );
            }
        }
    }
}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Proact.Services.Models {
    public class SurveyQuestionAnswersJsonConverter : JsonConverter<ISurveyAnswersContainer> {
        public override ISurveyAnswersContainer Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, ISurveyAnswersContainer value, JsonSerializerOptions options ) {
            if ( value is SurveyOpenAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyOpenAnswerContainer, 
                    typeof( SurveyOpenAnswerContainer ), options );
            }
            else if ( value is SurveyRatingAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyRatingAnswerContainer, 
                    typeof( SurveyRatingAnswerContainer ), options );
            }
            else if ( value is SurveySingleChoiceAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as SurveySingleChoiceAnswerContainer, 
                    typeof( SurveySingleChoiceAnswerContainer ), options );
            }
            else if ( value is SurveyMultipleChoiceAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as SurveyMultipleChoiceAnswerContainer, 
                    typeof( SurveyMultipleChoiceAnswerContainer ), options );
            }
            else if ( value is BoolAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as BoolAnswerContainer, 
                    typeof( BoolAnswerContainer ), options );
            }
            else if ( value is MoodAnswerContainer ) {
                JsonSerializer.Serialize(
                    writer, value as MoodAnswerContainer,
                    typeof( MoodAnswerContainer ), options );
            }
            else {
                throw new ArgumentOutOfRangeException(
                    nameof( value ), $"Unknown implementation of the interface " +
                        $"{nameof( SurveyAnswerModel )} for the parameter {nameof( value )}. " +
                        $"Unknown implementation: {value?.GetType().Name}" );
            }
        }
    }
}

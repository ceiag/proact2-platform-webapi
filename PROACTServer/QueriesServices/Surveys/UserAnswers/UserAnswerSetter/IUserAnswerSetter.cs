using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.UserAnswersSetter {
    public interface IUserAnswerSetter {
        public SurveyUserAnswersQuestionModel SetUserAnswerToQuestionSurvey(
           SurveysAssignationRelation assignmentRelation, SurveyQuestionCompileRequest compiledQuestion );
        public void Validate( SurveyQuestion question, SurveyQuestionCompileRequest compiledQuestion );
    }
}

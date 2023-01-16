using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services {
    public static class SurveyUserAnswersEntityMapper {
        public static List<SurveyUserAnswersQuestionModel> Map( 
            List<SurveyUsersQuestionsAnswersRelation> surveyUsersQuestionsAnswersRelations ) {
            var surveyUserAnswer = new List<SurveyUserAnswersQuestionModel>();

            foreach ( var answerItem in surveyUsersQuestionsAnswersRelations ) {
                surveyUserAnswer.Add( Map( answerItem ) );
            }

            return surveyUserAnswer;
        }

        public static SurveyUserAnswersQuestionModel Map(
            SurveyUsersQuestionsAnswersRelation surveyUsersQuestionsAnswersRelation ) {
            return new SurveyUserAnswersQuestionModel() {
                QuestionId = surveyUsersQuestionsAnswersRelation.QuestionId,
                QuestionsSetId = surveyUsersQuestionsAnswersRelation.Question.QuestionsSetId,
                SurveyId = surveyUsersQuestionsAnswersRelation.QuestionId,
                Answers = Map( surveyUsersQuestionsAnswersRelation.Answers )
            };
        }

        public static List<SurveyUserAnswerItemModel> Map( List<SurveyUserQuestionAnswer> surveyUserQuestionAnswers ) {
            var surveyUserAnswerItemsModel = new List<SurveyUserAnswerItemModel>();

            foreach ( var answerItem in surveyUserQuestionAnswers ) {
                surveyUserAnswerItemsModel.Add( Map( answerItem ) );
            }

            return surveyUserAnswerItemsModel;
        }

        public static SurveyUserAnswerItemModel Map( SurveyUserQuestionAnswer surveyUserQuestionAnswer ) {
            return new SurveyUserAnswerItemModel() {
                AnswerId = surveyUserQuestionAnswer.AnswerId,
                Value = surveyUserQuestionAnswer.Value
            };
        }
    }
}

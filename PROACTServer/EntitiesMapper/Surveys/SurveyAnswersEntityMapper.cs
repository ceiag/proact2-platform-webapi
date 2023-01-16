using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class SurveyAnswersEntityMapper {
        public static SurveyAnswersBlockModel Map( SurveyAnswersBlock surveyAnswersBlock ) {
            return new SurveyAnswersBlockModel() {
                Id = surveyAnswersBlock.Id,
                ProjectId = (Guid)surveyAnswersBlock.ProjectId,
                Answers = Map( surveyAnswersBlock.Answers ),
            };
        }

        public static List<SurveyAnswersBlockModel> Map( List<SurveyAnswersBlock> surveyAnswersBlock ) {
            var answersBlocks = new List<SurveyAnswersBlockModel>();

            foreach ( var answerBlock in surveyAnswersBlock ) {
                answersBlocks.Add( Map( answerBlock ) );
            }

            return answersBlocks;
        }

        public static List<SurveyAnswerModel> Map( ICollection<SurveyAnswer> answers ) {
            var answersResult = new List<SurveyAnswerModel>();

            foreach ( var answer in answers ) {
                answersResult.Add( Map( answer ) );
            }

            return answersResult;
        }

        public static SurveyAnswerModel Map( SurveyAnswer surveyAnswer ) {
            return new SurveyAnswerModel() {
                Id = surveyAnswer.Id,
                Label = surveyAnswer.LabelId
            };
        }
    }
}

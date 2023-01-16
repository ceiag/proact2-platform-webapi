using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services {
    public static class SurveysEntityMapper {
        private static readonly SurveyQuestionComposerProvider _surveyQuestionComposerProvider 
            = new SurveyQuestionComposerProvider();

        public static SurveyQuestionsSetModel Map( SurveyQuestionsSet surveyQuestionsSet ) {
            return new SurveyQuestionsSetModel() {
                Id = surveyQuestionsSet.Id,
                ProjectId = (Guid)surveyQuestionsSet.ProjectId,
                Title = surveyQuestionsSet.Title,
                Description = surveyQuestionsSet.Description,
                Version = surveyQuestionsSet.Version,
                State = surveyQuestionsSet.State,
                Questions = Map( surveyQuestionsSet.Questions ).OrderBy( x => x.Order ).ToList()
            };
        }

        public static List<SurveyQuestionsSetModel> Map( List<SurveyQuestionsSet> surveyQuestionsSets ) {
            var mappedSurveyQuestionsSets = new List<SurveyQuestionsSetModel>();
            
            foreach ( var surveyQuestion in surveyQuestionsSets ) {
                mappedSurveyQuestionsSets.Add( Map( surveyQuestion ) );
            }

            return mappedSurveyQuestionsSets;
        }

        public static SurveyQuestionModel Map( SurveyQuestion question ) {
            if ( question == null ) {
                return null;
            }

            var questionModel = new SurveyQuestionModel() {
                Id = question.Id,
                Order = question.Order,
                Question = question.Question,
                QuestionsSetId = question.QuestionsSetId,
                Title = question.Title,
                Type = question.Type
            };

            return _surveyQuestionComposerProvider.Compose( question, questionModel );
        }

        public static List<SurveyQuestionModel> Map( List<SurveyQuestion> questions ) { 
            var questionsList = new List<SurveyQuestionModel>();

            if ( questions != null ) {
                foreach ( var question in questions ) {
                    questionsList.Add( Map( question ) );
                }
            }
            
            return questionsList;
        }

        public static List<SurveyQuestionModel> Map( List<SurveysQuestionsRelation> questionsRelations ) {
            var questionsList = new List<SurveyQuestionModel>();

            int index = 0;
            foreach ( var question in questionsRelations ) {
                var questionModel = Map( question.Question );
                questionModel.Order = index;
                ++index;

                questionsList.Add( questionModel );
            }

            return questionsList;
        }

        public static SurveyModel Map( Survey survey ) {
            return new SurveyModel() {
                Id = survey.Id,
                ProjectId = (Guid)survey.ProjectId,
                Title = survey.Title,
                Description = survey.Description,
                SurveyState = survey.SurveyState,
                Created = survey.Created,
                Version = survey.Version,
                Reccurence = survey.Reccurence,
                StartTime = survey.StartTime,
                ExpireTime = survey.ExpireTime,
                Questions = Map( survey.Questions ),
            };
        }

        public static List<SurveyModel> Map( List<Survey> surveys ) {
            var surveysModel = new List<SurveyModel>();

            foreach ( var survey in surveys ) {
                surveysModel.Add( Map( survey ) );
            }

            return surveysModel;
        }
    }
}

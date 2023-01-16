using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public static class DbSurveysValidityChecker {
        public static ConsistencyRulesHelper IfSurveyQuestionsSetIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid questionsSetId, out SurveyQuestionsSet questionsSet ) {
            SurveyQuestionsSet questionsSetResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    questionsSetResult = rulesHelper
                        .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                        .Get( questionsSetId );
                    
                    return questionsSetResult != null;
                },
                () => {
                    return new OkObjectResult( questionsSetResult );
                },
                () => {
                    return new NotFoundObjectResult( $"survey not found!" );
                } );

            questionsSet = questionsSetResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyQuestionsSetIsPublished(
            this ConsistencyRulesHelper rulesHelper, Guid questionsSetId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ISurveyQuestionsSetQueriesService>()
                        .Get( questionsSetId ).State == QuestionsSetsState.PUBLISHED;
                },
                () => {
                    return new OkObjectResult( questionsSetId );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"question set with id {questionsSetId} is not published yet!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyQuestionsSetIsEditable(
            this ConsistencyRulesHelper rulesHelper, SurveyQuestionsSet questionsSet ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return questionsSet.State == QuestionsSetsState.DRAW;
                },
                () => {
                    return new OkObjectResult( questionsSet );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"Questions Set {questionsSet.Title} can not be modify after convalidation" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyQuestionIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid questionId, out SurveyQuestion question ) {
            SurveyQuestion questionResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    questionResult = rulesHelper
                        .GetQueriesService<ISurveyQuestionsQueriesService>().Get( questionId );

                    return questionResult != null;
                },
                () => {
                    return new OkObjectResult( questionResult );
                },
                () => {
                    return new NotFoundObjectResult( $"survey question not found!" );
                } );

            question = questionResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid surveyId, out Survey survey ) {
            Survey surveyResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    surveyResult = rulesHelper
                        .GetQueriesService<ISurveyQueriesService>()
                        .Get( surveyId );

                    return surveyResult != null;
                },
                () => {
                    return new OkObjectResult( surveyResult );
                },
                () => {
                    return new NotFoundObjectResult( $"survey with id {surveyId} not found!" );
                } );

            survey = surveyResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyIsEditable(
            this ConsistencyRulesHelper rulesHelper, Guid surveyId ) {
            
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ISurveyQueriesService>()
                        .Get( surveyId ).SurveyState == SurveyState.DRAW;
                },
                () => {
                    return new OkObjectResult( surveyId );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"Survey {surveyId} can not be modify after convalidation" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyQuestionNotAlreadyAdded(
            this ConsistencyRulesHelper rulesHelper, Survey survey, Guid questionId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper.GetQueriesService<ISurveyQueriesService>()
                        .IsQuestionAlreadyAdded( survey, questionId );
                },
                () => {
                    return new OkObjectResult( questionId );
                },
                () => {
                    return new ConflictObjectResult(
                        $"Question with id {questionId} is already present!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyAssignationIsValid(
            this ConsistencyRulesHelper rulesHelper, 
            Guid assegnationId, out SurveysAssignationRelation assegnamentRelation ) {
            SurveysAssignationRelation assegnationResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    assegnationResult = rulesHelper
                        .GetQueriesService<ISurveyAssignationQueriesService>()
                        .GetById( assegnationId );

                    return assegnationResult != null;
                },
                () => {
                    return new OkObjectResult( assegnationResult );
                },
                () => {
                    return new NotFoundObjectResult(
                        $"Assegnation with id {assegnationId} not found!" );
                } );

            assegnamentRelation = assegnationResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfSurveyIsAssignedToUser(
            this ConsistencyRulesHelper rulesHelper, Guid assegnationId, Guid userId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var assegnation = rulesHelper
                        .GetQueriesService<ISurveyAssignationQueriesService>()
                        .GetById( assegnationId );

                    return assegnation != null && assegnation.UserId == userId;
                },
                () => {
                    return new OkObjectResult( assegnationId );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"You can not access to this compiled survey!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfAssignationIsOwnedByPatient(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid assignationId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<ISurveyAssignationQueriesService>()
                        .GetById( assignationId ).UserId == userId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"assignation {assignationId} is not assigned to user {userId}" );
                } );

            return validityChecker;
        }
    }
}

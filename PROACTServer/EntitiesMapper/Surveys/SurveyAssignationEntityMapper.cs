using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public static class SurveyAssignationEntityMapper {
        public static SurveyAssignationModel Map( SurveysAssignationRelation surveyAssignment ) {
            if ( surveyAssignment.CompletedDateTime == null ) {
                surveyAssignment.CompletedDateTime = DateTime.MinValue;
            }

            return new SurveyAssignationModel() {
                Id = surveyAssignment.Id,
                ExpireTime = surveyAssignment.ExpireTime,
                StartTime = surveyAssignment.StartTime,
                SurveyId = surveyAssignment.SurveyId,
                UserId = surveyAssignment.UserId,
                SurveyDescription = surveyAssignment.Survey.Description,
                SurveyTitle = surveyAssignment.Survey.Title,
                SurveyState = surveyAssignment.Survey.SurveyState,
                SurveyVersion = surveyAssignment.Survey.Version,
                Completed = surveyAssignment.Completed,
                CompletedDateTime = surveyAssignment.CompletedDateTime,
                Reccurence = surveyAssignment.Scheduler.Reccurence,
                User = UserEntityMapper.Map( surveyAssignment.User ),
                Scheduler = ScheduledSurveyEntityMapper.Map( surveyAssignment.Scheduler )
            };
        }

        public static List<SurveyAssignationModel> Map( List<SurveysAssignationRelation> surveysAssignments ) {
            var surveyAssignmentList = new List<SurveyAssignationModel>();

            foreach ( var surveyAssignment in surveysAssignments ) {
                surveyAssignmentList.Add( Map( surveyAssignment ) );
            }

            return surveyAssignmentList;
        }
    }
}

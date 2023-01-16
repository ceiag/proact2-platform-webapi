using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyAssignationQueriesService : IQueriesService {
        public List<SurveysAssignationRelation> AssignSurveyToPatients(
            AssignSurveyToPatientRequest request );
        public List<SurveysAssignationRelation> GetByUserId( Guid userId );
        public SurveysAssignationRelation GetById( Guid assegnationId );
        public List<SurveysAssignationRelation> GetFromSurveyId( Guid surveyId );
        public List<SurveysAssignationRelation> GetCompletedSurveysAssigned( Guid userId );
        public List<SurveysAssignationRelation> GetNotCompletedSurveysAssigned( Guid userId );
        public List<SurveysAssignationRelation> GetAvailableSurveysForUser( Guid userId );
        public List<SurveysAssignationRelation> GetExpiresWithinTwoDays();
        public List<SurveysAssignationRelation> GetCompletedFromPatients( Guid surveyId, Guid userId );
    }
}

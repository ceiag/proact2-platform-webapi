using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices;
public sealed class SurveyAssignationQueriesService : ISurveyAssignationQueriesService {
    private readonly ProactDatabaseContext _database;
    private readonly Func<SurveysAssignationRelation, bool> _isExpiringWithin48Hours
        = x => ( x.ExpireTime.Date - DateTime.UtcNow.Date ).TotalHours <= 48;
    private readonly Func<SurveysAssignationRelation, bool> _isNotExpired
        = x => x.StartTime.Date <= DateTime.UtcNow.Date && x.ExpireTime.Date >= DateTime.UtcNow.Date;

    public SurveyAssignationQueriesService( ProactDatabaseContext database ) {
        _database = database;
    }

    public List<SurveysAssignationRelation> AssignSurveyToPatients(
        AssignSurveyToPatientRequest request ) {
        var assigments = new List<SurveysAssignationRelation>();

        foreach ( var scheduler in request.Schedulers ) {
            var assignment = new SurveysAssignationRelation() {
                SurveyId = request.SurveyId,
                SchedulerId = scheduler.Id,
                UserId = scheduler.UserId,
                StartTime = DateTime.UtcNow.Date,
                ExpireTime = GetExpireDateFromScheduler( scheduler ),
            };

            assigments.Add( assignment );
        }

        _database.SurveysAssignationsRelations.AddRange( assigments );
        _database.SaveChanges();

        return assigments;
    }

    private DateTime GetExpireDateFromScheduler( SurveyScheduler scheduler ) {
        var expiringDate = scheduler.Reccurence switch {
            SurveyReccurence.Once => scheduler.ExpireTime,
            SurveyReccurence.Daily => DateTime.UtcNow.Date,
            SurveyReccurence.Weekly => DateTime.UtcNow.Date.AddDays( 7 ),
            SurveyReccurence.Monthly => DateTime.UtcNow.Date.AddMonths( 1 ),
            _ => throw new Exception( "SurveyReccurence not supported" )
        };

        return expiringDate;
    }

    public List<SurveysAssignationRelation> GetByUserId( Guid userId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .OrderBy( x => x.ExpireTime )
            .Where( x => x.UserId == userId ).ToList();
    }

    public List<SurveysAssignationRelation> GetCompletedSurveysAssigned( Guid userId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .Where( x => x.UserId == userId )
            .Where( x => x.Completed )
            .OrderByDescending( x => x.CompletedDateTime )
            .ToList();
    }

    public List<SurveysAssignationRelation> GetNotCompletedSurveysAssigned( Guid userId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .Where( x => x.UserId == userId )
            .Where( x => !x.Completed )
            .OrderBy( x => x.ExpireTime )
            .ToList();
    }

    public List<SurveysAssignationRelation> GetAvailableSurveysForUser( Guid userId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .Where( x => x.UserId == userId )
            .Where( x => !x.Completed )
            .Where( _isNotExpired )
            .OrderBy( x => x.ExpireTime )
            .ToList();
    }

    public SurveysAssignationRelation GetById( Guid assegnationId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .FirstOrDefault( x => x.Id == assegnationId );
    }

    public List<SurveysAssignationRelation> GetFromSurveyId( Guid surveyId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .Include( x => x.Scheduler )
            .Where( x => x.SurveyId == surveyId )
            .ToList();
    }

    public List<SurveysAssignationRelation> GetExpiresWithinTwoDays() {
        return _database.SurveysAssignationsRelations
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Include( x => x.Scheduler )
            .Where( x => !x.Completed )
            .Where( x => x.ExpireTime.Date >= DateTime.UtcNow.Date )
            .Where( x => x.StartTime.Date <= DateTime.UtcNow.Date )
            .Where( _isExpiringWithin48Hours )
            .ToList();
    }

    public List<SurveysAssignationRelation> GetCompletedFromPatients( Guid surveyId, Guid userId ) {
        return _database.SurveysAssignationsRelations
            .IncludeSurveyAssegnationCommonTables()
            .Where( x => x.SurveyId == surveyId )
            .Where( x => x.Completed )
            .Where( x => x.UserId == userId )
            .ToList();
    }
}

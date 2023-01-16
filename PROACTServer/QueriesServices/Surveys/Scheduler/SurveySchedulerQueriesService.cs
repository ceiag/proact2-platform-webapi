using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices;
public class SurveySchedulerQueriesService : ISurveySchedulerQueriesService {
    private ProactDatabaseContext _database;
    private readonly Func<SurveyScheduler, bool> _isStarted
        = x => x.StartTime < DateTime.UtcNow;
    private readonly Func<SurveyScheduler, bool> _isNotExpired
        = x => x.ExpireTime > DateTime.UtcNow;
    private readonly Func<SurveyScheduler, bool> _isNotExecutedAlmostOnce
        = x => x.LastSubmission < DateTime.MinValue.AddYears( 1 );
    private readonly Func<SurveyScheduler, bool> _isSameDayOfTheWeek
        = x => x.StartTime.ToUniversalTime().DayOfWeek == DateTime.UtcNow.DayOfWeek;
    private readonly Func<SurveyScheduler, bool> _isPassedExactlyAMonth
        = x => x.StartTime.ToUniversalTime().Day == DateTime.UtcNow.Day
                && x.LastSubmission.Month < DateTime.UtcNow.Month;

    public SurveySchedulerQueriesService( ProactDatabaseContext database ) {
        _database = database;
    }

    public void Create( List<SurveyScheduler> schedulers ) {
        _database.SurveyScheduler.AddRange( schedulers );
        _database.SaveChanges();
    }

    public List<SurveyScheduler> Create( CreateScheduledSurveyRequest request ) {
        var schedulers = new List<SurveyScheduler>();

        foreach ( var userId in request.UserIds ) {
            var scheduler = new SurveyScheduler() {
                Id = Guid.NewGuid(),
                UserId = userId,
                SurveyId = request.SurveyId,
                Reccurence = request.Reccurence,
                StartTime = request.StartTime.Date,
                ExpireTime = request.ExpireTime.Date.ToUniversalTime(),
                LastSubmission = DateTime.MinValue,
            };

            schedulers.Add( scheduler );
        }

        Create( schedulers );
        return schedulers;
    }

    public SurveyScheduler Create( SurveyScheduler scheduler ) {
        return _database.SurveyScheduler.Add( scheduler ).Entity;
    }

    public SurveyScheduler GetBySchedulerId( Guid schedulerId ) {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .FirstOrDefault( x => x.Id == schedulerId );
    }

    public List<SurveyScheduler> GetBySchedulerIds( List<Guid> schedulerIds ) {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( x => schedulerIds.Contains( x.Id ) )
            .ToList();
    }

    public List<SurveyScheduler> GetByUserId( Guid userId ) {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( x => x.UserId == userId )
            .ToList();
    }

    public List<SurveyScheduler> GetBySurveyId( Guid surveyId ) {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( x => x.SurveyId == surveyId )
            .ToList();
    }

    public List<SurveyScheduler> GetNotProcessedOnceSurveyScheduled() {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( _isStarted )
            .Where( _isNotExpired )
            .Where( x => x.Reccurence == SurveyReccurence.Once )
            .Where( _isNotExecutedAlmostOnce )
            .ToList();
    }

    public List<SurveyScheduler> GetNotProcessedDailySurveyScheduled() {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( _isStarted )
            .Where( _isNotExpired )
            .Where( x => x.Reccurence == SurveyReccurence.Daily )
            .Where( x => x.LastSubmission.Date.AddDays( 1 ) < DateTime.UtcNow )
            .ToList();
    }

    public List<SurveyScheduler> GetNotProcessedWeeklySurveyScheduled() {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( _isStarted )
            .Where( _isNotExpired )
            .Where( x => x.Reccurence == SurveyReccurence.Weekly )
            .Where( x => x.LastSubmission.AddDays( 1 ) < DateTime.UtcNow )
            .Where( _isSameDayOfTheWeek )
            .ToList();
    }

    public List<SurveyScheduler> GetNotProcessedMonthlySurveyScheduled() {
        return _database.SurveyScheduler
            .Include( x => x.Survey )
            .Include( x => x.User )
            .Where( _isStarted )
            .Where( _isNotExpired )
            .Where( x => x.Reccurence == SurveyReccurence.Monthly )
            .Where( _isPassedExactlyAMonth )
            .ToList();
    }

    public void SetSchedulersAsProcessed( List<SurveyScheduler> schedulers ) {
        foreach ( var scheduler in schedulers ) {
            scheduler.LastSubmission = DateTime.UtcNow;
        }

        _database.SurveyScheduler.UpdateRange( schedulers );
        _database.SaveChanges();
    }
}

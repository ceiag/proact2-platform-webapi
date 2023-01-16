using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Entities.MedicalTeams;
using Proact.Services.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices.DataManagers;

public class DataManagerQueriesService : IDataManagerQueriesService {
    private ProactDatabaseContext _database;

    public DataManagerQueriesService( ProactDatabaseContext proactDatabaseContext ) {
        _database = proactDatabaseContext;
    }

    public DataManager Get( Guid userId ) {
        return _database
            .DataManagers
            .FirstOrDefault( x => x.UserId == userId );
    }

    public DataManager Create( Guid userId ) {
        return _database.DataManagers.Add( new DataManager() { UserId = userId } ).Entity;
    }

    public void RemoveFromMedicalTeam( MedicalTeam medicalTeam, DataManager dataManager ) {
        medicalTeam.DataManagers.Remove( dataManager );
    }

    public DataManager Delete( Guid userId ) {
        return _database.DataManagers.Remove( Get( userId ) ).Entity;
    }

    public List<DataManager> GetAll( Guid instituteId ) {
        return _database.DataManagers.Where( x => x.User.InstituteId == instituteId ).ToList();
    }

    public void AddToMedicalTeam( Guid userId, Guid medicalTeamId ) {
        var dataManager = Get( userId );
        var dataManagerMedicalTeamRelation = new DataManagersMedicalTeamRelation() {
            Id = Guid.NewGuid(),
            MedicalTeamId = medicalTeamId,
            DataManagerId = dataManager.Id
        };

        _database.DataManagersMedicalTeamRelations.Add( dataManagerMedicalTeamRelation );
    }

    private DataManagersMedicalTeamRelation GetMedicalTeamRelation( Guid userId, Guid medicalTeamId ) {
        return _database.DataManagersMedicalTeamRelations.FirstOrDefault(
            x => x.MedicalTeamId == medicalTeamId && x.DataManager.UserId == userId );
    }

    public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam ) {
        var relationToRemove = GetMedicalTeamRelation( userId, medicalTeam.Id );

        _database.DataManagersMedicalTeamRelations.Remove( relationToRemove );
    }

    public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId ) {
        return Get( userId ).MedicalTeams.Any( x => x.Id == medicalTeamId );
    }
}

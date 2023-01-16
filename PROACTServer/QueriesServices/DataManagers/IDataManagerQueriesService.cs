using Proact.Services.Entities;
using System.Collections.Generic;
using System;
using Proact.Services.Entities.Users;

namespace Proact.Services.QueriesServices.DataManagers;

public interface IDataManagerQueriesService : IQueriesService {
    public DataManager Get( Guid userId );
    public List<DataManager> GetAll( Guid instituteId );
    public DataManager Create( Guid userId );
    public DataManager Delete( Guid userId );
    public void AddToMedicalTeam( Guid userId, Guid medicalTeamId );
    public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam );
    public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId );
}

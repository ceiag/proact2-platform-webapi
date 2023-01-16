using System.Collections.Generic;
using System;
using Proact.Services.Entities.Users;
using Proact.Services.Models.DataManagers;

namespace Proact.Services.EntitiesMapper.DataManagers;

public static class DataManagerEntityMapper {
    public static DataManagerModel Map( DataManager dataManager ) {
        return new DataManagerModel() {
            MedicalTeams = MedicalTeamEntityMapper.Map( dataManager.MedicalTeams ),
            UserId = dataManager.User.Id,
            InstituteId = (Guid)dataManager.User.InstituteId,
            AccountId = dataManager.User.AccountId,
            AvatarUrl = dataManager.User.AvatarUrl,
            Name = dataManager.User.Name,
            Title = dataManager.User.Title
        };
    }

    public static List<DataManagerModel> Map( List<DataManager> dataManagers ) {
        var dataManagerModels = new List<DataManagerModel>();

        foreach ( var dataManager in dataManagers ) {
            dataManagerModels.Add( Map( dataManager ) );
        }

        return dataManagerModels;
    }
}
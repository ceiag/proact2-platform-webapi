using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Entities.Users;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.DataManagers;
using System;

namespace Proact.Services.DatabaseValidityChecker;

public static class DbDataManagerValidityChecker {
    public static ConsistencyRulesHelper IfDataManagerIsValid(
        this ConsistencyRulesHelper rulesHelper, Guid userId, out DataManager dataManager ) {
        DataManager dataManagerResult = null;

        var validityChecker = rulesHelper.CheckIf(
            () => {
                dataManagerResult = rulesHelper
                    .GetQueriesService<IDataManagerQueriesService>()
                    .Get( userId );

                return dataManagerResult != null;
            },
            () => {
                return new OkObjectResult( userId );
            },
            () => {
                return new NotFoundObjectResult( $"DataManager with userId: {userId} not found!" );
            } );

        dataManager = dataManagerResult;
        return validityChecker;
    }

    public static ConsistencyRulesHelper IfDataManagerNotExist(
        this ConsistencyRulesHelper rulesHelper, Guid userId ) {

        var validityChecker = rulesHelper.CheckIf(
            () => {
                return rulesHelper
                    .GetQueriesService<IDataManagerQueriesService>()
                    .Get( userId ) == null;
            },
            () => {
                return new OkObjectResult( userId );
            },
            () => {
                return new ConflictObjectResult( $"DataManager with userId: {userId} already exist!" );
            } );

        return validityChecker;
    }
}
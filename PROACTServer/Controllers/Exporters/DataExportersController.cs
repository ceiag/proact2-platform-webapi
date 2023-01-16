using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Exporters;
using Proact.Services.Models.Exportations;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System;

namespace Proact.Services.Controllers.Exporters;
[ApiController]
[Route( ProactRouteConfiguration.DefaultRoute )]
public class DataExportersController : ProactBaseController {
    private readonly IProactDataExporterService _dataExporterService;

    public DataExportersController(
        IChangesTrackingService changesTrackingService,
        ConsistencyRulesHelper consistencyRulesHelper,
        IProactDataExporterService surveyAnswersExporterService ) 
        : base( changesTrackingService, consistencyRulesHelper ) {
        _dataExporterService = surveyAnswersExporterService;
    }

    /// <summary>
    /// Get the stats over the time about a patient in csv format
    /// </summary>
    /// <param name="surveyId">Id of the Surveys</param>
    /// <param name="userId">Id of the User</param>
    /// <param name="format">supported: csv</param>
    /// <returns>Stats of Survey</returns>
    [HttpGet]
    [Route( "survey/{surveyId:guid}/user/{userId:guid}/{format}" )]
    [Authorize( Policy = Policies.DataExporterRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyStatsExportResult ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetExportedSurveyStatsOverTimeForPatient(
        Guid surveyId, Guid userId, string format ) {
        Survey survey = null;

        return RulesHelper
            .IfSurveyIsValid( surveyId, out survey )
            .Then( () => {

                if ( format == "csv" ) {
                    var exportedCsv = _dataExporterService
                        .ExportPatientSurveyAnswers( surveyId, userId, new CsvFormatSurveyExporter() );

                    return Ok( exportedCsv );
                }

                return BadRequest( "format not found" );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Export Analysis of a patient in desired format
    /// </summary>
    /// <param name="userId">Id of the User</param>
    /// <param name="format">supported: csv</param>
    /// <returns>Stats of Survey</returns>
    [HttpGet]
    [Route( "analysis/user/{userId:guid}/{format}" )]
    [Authorize( Policy = Policies.DataExporterRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyStatsExportResult ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetExportedAnalysisForPatient( Guid userId, string format ) {
        Patient patient = null;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .Then( () => {

                if ( format == "csv" ) {
                    var exportedCsv = _dataExporterService
                        .ExportMessagesFromPatient( userId, new CsvFormatAnalysisExporter() );

                    return Ok( exportedCsv );
                }

                return BadRequest( "format not found" );
            } )
            .ReturnResult();
    }
}

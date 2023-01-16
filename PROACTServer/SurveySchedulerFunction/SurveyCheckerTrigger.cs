//using System;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//public static async Task Run( TimerInfo myTimer, ILogger log ) {
//    var url = "https://devproactservices.azurewebsites.net/api/it-IT/SurveysSchedulerCheck";

//    var data = new StringContent( "", Encoding.UTF8, "application/json" );
//    using var client = new HttpClient();
//    var response = await client.PostAsync( url, data );
//    var result = await response.Content.ReadAsStringAsync();

//    log.LogInformation( $"SurveyCheckerTrigger Executed at: {DateTime.Now} with result: {result}" );
//}
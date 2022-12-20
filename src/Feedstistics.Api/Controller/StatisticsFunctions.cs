namespace Feedstistics.Api.Controller
{
    using System.Net;
    using Feedstistics.Data;
    using Feedstistics.Lib;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
    using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;

    public class StatisticsFunctions
    {
        private ILogger Logger { get; }

        private IFeedstisticsService FeedDataService { get; }

        public StatisticsFunctions(
            ILoggerFactory loggerFactory,
            IFeedstisticsService feedDataService)
        {
            this.Logger = loggerFactory.CreateLogger<StatisticsFunctions>();
            this.FeedDataService = feedDataService;
        }

        /// <summary>
        /// Gets latest statistc for a given statistic name.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="statisticName"></param>
        /// <returns></returns>
        [Function("GetLatestStatistic")]
        [OpenApiOperation(operationId: "GetLatestStatistic", tags: new[] { "statistics" }, Summary = "Gets latest statistc for a given statistic name.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "statisticName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The name of the statistic to get statistics for.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic), Description = "The OK response with latest IStatistic for statistic name.")]
        public async Task<HttpResponseData> GetLatestStatisticForName(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistic/name/{statisticName}/latest")]
            HttpRequestData req,
            string statisticName)
        {
            this.Logger.LogInformation($"Function execution started: [{GetLatestStatisticForName}], StatisticName: [{statisticName}]");

            try
            {
                var statistic = await this.FeedDataService.GetLatestStatisticAsync(statisticName);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetLatestStatisticForName)}], StatisticName: [{statisticName}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets latest statistc for all statistic names.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="statisticName"></param>
        /// <returns></returns>
        [Function("GetLatestStatistics")]
        [OpenApiOperation(operationId: "GetLatestStatistics", tags: new[] { "statistics" }, Summary = "Gets latest statistc for all statistic names.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic[]), Description = "The OK response with latest IStatistics.")]
        public async Task<HttpResponseData> GetLatestStatistics(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/latest")]
            HttpRequestData req)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetLatestStatistics)}]");

            try
            {
                var statistic = await this.FeedDataService.GetLatestStatisticsAsync();

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetLatestStatistics)}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all statistcs for a given statistic name starting starting from rangeStart.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="statisticName"></param>
        /// <returns></returns>
        [Function("GetStatisticsForRangeStart")]
        [OpenApiOperation(operationId: "GetStatisticsForRangeStart", tags: new[] { "statistics" }, Summary = "Gets all statistcs for a given statistic name starting starting from rangeStart.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "statisticName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The name of the statistic to get statistics for.")]
        [OpenApiParameter(name: "rangeStart", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The start time to get statistic values for.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic[]), Description = "The OK response with collection IStatistics.")]
        public async Task<HttpResponseData> GetStatisticsForRangeStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/name/{statisticName}/range-from/{rangeStart}")]
            HttpRequestData req,
            string statisticName,
            DateTime rangeStart)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetStatisticsForRangeStart)}], StatisticName: [{statisticName}], RangeStart: [{rangeStart}]");

            try
            {
                var statistic = await this.FeedDataService.GetStatisticsAsync(statisticName, rangeStart);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetStatisticsForRangeStart)}], StatisticName: [{statisticName}], RangeStart: [{rangeStart}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all statistcs for a given statistic name starting from rangeStart to rangeEnd.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="statisticName">The name of the statistic to get statistics for.</param>
        /// <param name="rangeStart">The start time to get statistic values for.</param>
        /// <param name="rangeEnd">The start time to get statistic values for.</param>
        /// <returns></returns>
        [Function("GetStatisticsForRangeStartAndEnd")]
        [OpenApiOperation(operationId: "GetStatisticsForRangeStartAndEnd", tags: new[] { "statistics" }, Summary = "Gets all statistcs for a given statistic name starting starting from rangeStart to rangeEnd.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "statisticName", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The name of the statistic to get statistics for.")]
        [OpenApiParameter(name: "rangeStart", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The start time to get statistic values for.")]
        [OpenApiParameter(name: "rangeEnd", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The end time to get statistic values for.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic[]), Description = "The OK response with collection IStatistics.")]
        public async Task<HttpResponseData> GetStatisticsForRangeStartAndEnd(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/name/{statisticName}/range-from/{rangeStart}/range-end/{rangeEnd}")]
            HttpRequestData req,
            string statisticName,
            DateTime rangeStart,
            DateTime rangeEnd)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetLatestStatisticForName)}], StatisticName: [{statisticName}], RangeStart: [{rangeStart}], RangeEnd: [{rangeEnd}]");

            try
            {
                var statistic = await this.FeedDataService.GetStatisticsAsync(statisticName, rangeStart, rangeEnd);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetLatestStatisticForName)}], StatisticName: [{statisticName}], RangeStart: [{rangeStart}], RangeEnd: [{rangeEnd}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all statistcs starting from rangeStart.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="rangeStart">The start time to get statistic values for.</param>
        /// <returns></returns>
        [Function("GetAllStatisticsForRangeStart")]
        [OpenApiOperation(operationId: "GetAllStatisticsForRangeStart", tags: new[] { "statistics" }, Summary = "Gets all statistcs for a given statistic name starting starting from rangeStart.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "rangeStart", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The start time to get statistic values for.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic[]), Description = "The OK response with collection IStatistics.")]
        public async Task<HttpResponseData> GetAllStatisticsForRangeStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/all/range-from/{rangeStart}")]
            HttpRequestData req,
            DateTime rangeStart)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetAllStatisticsForRangeStart)}], RangeStart: [{rangeStart}]");

            try
            {
                var statistic = await this.FeedDataService.GetStatisticsAsync(rangeStart);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetAllStatisticsForRangeStart)}], RangeStart: [{rangeStart}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all statistcs starting from rangeStart to rangeEnd.
        /// </summary>
        /// <param name="req">HttpRequest.</param>
        /// <param name="rangeStart">The start time to get statistic values for.</param>
        /// <param name="rangeEnd">The start time to get statistic values for.</param>
        /// <returns></returns>
        [Function("GetAllStatisticsForRangeStartAndEnd")]
        [OpenApiOperation(operationId: "GetAllStatisticsForRangeStartAndEnd", tags: new[] { "statistics" }, Summary = "Gets all statistcs for a given statistic name starting starting from rangeStart to rangeEnd.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "rangeStart", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The start time to get statistic values for.")]
        [OpenApiParameter(name: "rangeEnd", In = ParameterLocation.Path, Required = true, Type = typeof(DateTime), Description = "The end time to get statistic values for.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IStatistic[]), Description = "The OK response with collection IStatistics.")]
        public async Task<HttpResponseData> GetAllStatisticsForRangeStartAndEnd(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/all/range-from/{rangeStart}/range-end/{rangeEnd}")]
            HttpRequestData req,
            DateTime rangeStart,
            DateTime rangeEnd)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetAllStatisticsForRangeStartAndEnd)}], RangeStart: [{rangeStart}], RangeEnd: [{rangeEnd}]");

            try
            {
                var statistic = await this.FeedDataService.GetStatisticsAsync(rangeStart, rangeEnd);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetAllStatisticsForRangeStartAndEnd)}], RangeStart: [{rangeStart}], RangeEnd: [{rangeEnd}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all statistcs names.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Function("GetStatisticsNames")]
        [OpenApiOperation(operationId: "GetStatisticsNames", tags: new[] { "statistics" }, Summary = "Gets all statistcs names.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "key", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string[]), Description = "The OK response with collection of statistic names.")]
        public async Task<HttpResponseData> GetStatisticsNames(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/name")]
            HttpRequestData req)
        {
            this.Logger.LogInformation($"Function execution started: [{nameof(GetStatisticsNames)}]");

            try
            {
                var statistic = this.FeedDataService.GetStatisticNames();

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(statistic);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception executing function: [{nameof(GetStatisticsNames)}]");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }
    }
}

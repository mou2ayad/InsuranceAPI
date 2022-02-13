using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Utilities.ErrorHandling;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;

namespace Insurance.Utilities.APIRestClient
{
    public class ApiRestClient<TY>
    {
        protected readonly HttpClient HttpClient;
        protected readonly ILogger<TY> Logger;

        public ApiRestClient(HttpClient httpClient, ILogger<TY> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        public async Task<T> SendGetRequest<T>(string endpoint)
        {
            var responseMessage= await Retry(async () =>
            {
                var response = await HttpClient.GetAsync($"/{endpoint}");
                
                if (response.StatusCode.IsConnectionIssueStatusCode())
                    throw new ConnectivityException(response.StatusCode);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Logger.LogWarning(await ExtractErrorMessage(response));
                    throw new ClientException("Invalid input");
                }
                    

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await ExtractErrorMessage(response));

                return response;
            }, (ex, retryCount) =>
                Logger.LogWarning("Retrying to call [{0}], method GET, attempt [{1}], ErrorMessage: [{2}] ",
                    endpoint, retryCount, ex.Message));

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }
        
        private async Task<string> ExtractErrorMessage(HttpResponseMessage response)
        {
            string errorMessage = $"Fail in http call with status [{response.StatusCode}]";
            if (response.Content != null)
            {
                ProblemDetails problemDetails =
                    JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());
                if (problemDetails != null)
                    errorMessage = $"{errorMessage}, traceId: [{problemDetails.TraceId}]";
            }

            return errorMessage;
        }

        private static Task<T> Retry<T>(Func<Task<T>> action, Action<Exception, int> onRetry = null) =>
            Policy.Handle<ConnectivityException>()
                .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(1.0), (exception, _, retryCount, __) =>
                {
                    Action<Exception, int> onRetryAction = onRetry;
                    if (onRetryAction == null)
                        return;
                    onRetryAction(exception, retryCount);
                }).ExecuteAsync(action);
    }
}


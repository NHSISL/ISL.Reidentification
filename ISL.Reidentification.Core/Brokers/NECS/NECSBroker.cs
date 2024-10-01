// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Brokers.NECS;
using ISL.ReIdentification.Core.Models.Brokers.NECS.Requests;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.NECS
{
    public class NECSBroker : INECSBroker
    {
        private readonly NECSConfiguration necsConfiguration;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;

        public NECSBroker(NECSConfiguration necsConfiguration)
        {
            this.necsConfiguration = necsConfiguration;
            this.httpClient = SetupHttpClient();
            this.apiClient = SetupApiClient();
        }

        public async ValueTask<NecsReIdentificationResponse> ReIdAsync(
            NecsReidentificationRequest necsReidentificationRequest)
        {
            var returnedAddress =
                await this.apiClient.PostContentAsync<NecsReidentificationRequest, NecsReIdentificationResponse>
                    ($"api/Reid/Process", necsReidentificationRequest);

            return returnedAddress;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: this.necsConfiguration.ApiUrl),
            };

            httpClient.DefaultRequestHeaders.Add("X-API-KEY", necsConfiguration.ApiKey);

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(this.httpClient);
    }
}

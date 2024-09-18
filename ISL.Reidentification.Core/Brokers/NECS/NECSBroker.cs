// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.NECS;
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

        public async ValueTask<List<string>> ReIdAsync(string pseudoNumber)
        {
            var returnedAddress =
                await this.apiClient.GetContentAsync<List<string>>($"api/reid?skid={pseudoNumber}");

            return returnedAddress;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: this.necsConfiguration.ApiUrl),
            };

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(this.httpClient);
    }
}

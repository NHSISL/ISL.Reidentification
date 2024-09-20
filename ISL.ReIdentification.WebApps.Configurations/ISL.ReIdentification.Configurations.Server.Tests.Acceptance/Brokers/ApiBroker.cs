// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Net.Http;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private readonly WebApplicationFactory<Program> webApplicationFactory;
        private readonly HttpClient httpClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        private readonly IOdsStorageBroker odsStorageBrokerMock;

        public ApiBroker()
        {
            this.webApplicationFactory = new AcceptanceTestApplicationFactory<Program>();
            this.httpClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.httpClient);
            this.odsStorageBrokerMock = this.webApplicationFactory.Services.GetService<IOdsStorageBroker>();
        }
    }
}
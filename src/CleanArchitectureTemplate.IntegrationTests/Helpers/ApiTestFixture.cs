﻿using System;
using System.Net.Http;
using CleanArchitectureTemplate.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.IntegrationTests.Helpers
{

    public class ApiTestFixture : DatabaseTestFixture, IDisposable
    {
        public readonly HttpClient HttpClient;

        public ApiTestFixture()
        {
            var server = new TestServer(new WebHostBuilder()
            .UseEnvironment(EnvironmentName.Development)
            .ConfigureTestServices((IServiceCollection serviceCollection) =>
            {
                // Use stubbed database for integration tests
                //serviceCollection.AddScoped<MyBeerAppContext>(x => dbContext);
                //serviceCollection.AddScoped<IRepository<Beer>, EfRepository<Beer>>();
            })
            .UseStartup<Startup>());

            HttpClient = server.CreateClient();
        }

        public void Dispose()
        {
        }
    }
}

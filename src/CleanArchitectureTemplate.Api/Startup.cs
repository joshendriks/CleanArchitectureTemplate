﻿using CleanArchitectureTemplate.Application.Shared;
using CleanArchitectureTemplate.Application.ToDoItems.UseCases;
using CleanArchitectureTemplate.Domain.Shared;
using CleanArchitectureTemplate.Domain.ToDoItems;
using CleanArchitectureTemplate.Infrastructure.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace CleanArchitectureTemplate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // TODO: read from appsettings.json
            var applicationSettings = ApplicationSettings.Builder
                .WithCacheDuration(TimeSpan.FromSeconds(10))
                .Build();

            services.AddSingleton<IApplicationSettings>(x => applicationSettings);

            services.AddScoped<AppDbContext>(x => new AppDbContextFactory().CreateDbContext(null));
            services.AddScoped<IReadOnlyRepository<ToDoItem>, CachedRepositoryDecorator<ToDoItem>>();
            services.AddScoped<IRepository<ToDoItem>, EfRepository<ToDoItem>>();
            services.AddMediatR(cfg => cfg.AsScoped(), typeof(ToDoItemsHandler).GetTypeInfo().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

﻿using Azure.Identity;
using dotnet_exemplar.Application.Common.Interfaces;
using dotnet_exemplar.Infrastructure.Data;
using dotnet_exemplar.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Generation.Processors.Security;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Get current logged in user
        builder.Services.AddScoped<IUser, CurrentUser>();

        // For accessing things like Headers from the handler
        builder.Services.AddHttpContextAccessor();

        // provides /health endpoint and checks if db is reachable
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Unnecessary because Angular is used for the frontend
        //builder.Services.AddRazorPages();

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            // Suppress model state invalid filter
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.DocumentName = "v1";
            configure.Title = "dotnet_exemplar API";

            configure.AddSecurity("ApiKey", new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "api-key",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Description = "Enter your API key"
            });

            configure.OperationProcessors.Add(
                new AspNetCoreOperationSecurityScopeProcessor("ApiKey")
            );
        });
    }

    public static void AddKeyVaultIfConfigured(this IHostApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }
    }
}

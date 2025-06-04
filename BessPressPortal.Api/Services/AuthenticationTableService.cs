namespace BessPressPortal.Api.Services
{

    using Azure;
    using Azure.Data.Tables;
    using BessPressPortal.Shared.Models;
    using global::BessPressPortal.Api.Entities;
    using global::BessPressPortal.Api.Helpers;
    using global::BessPressPortal.Client.Pages.Private;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AuthenticationTableService
    {
        private readonly TableClient _tableClient;
        private readonly string _tableName = "Logins"; // Your table name
        private readonly IPasswordHasher<LoginEntity> _hasher;

        public AuthenticationTableService(IConfiguration configuration, IPasswordHasher<LoginEntity> hasher)
        {
            string connectionString = configuration["AzureStorage:TableConnectionString"];
            _hasher = hasher;
            _tableClient = new TableClient(connectionString, _tableName);
            _tableClient.CreateIfNotExists();
        }
        public async Task<ApiResponse<bool>> AddUserAsync(CreateUserDto dto)
        {
            var existing = await _tableClient.QueryAsync<LoginEntity>(e => e.RowKey == dto.UserEmail).FirstOrDefaultAsync();

            if (existing != null)
            {
                return ApiResponse<bool>.Fail("Email already exists.");
            }

            var loginEntity = new LoginEntity
            {
                UserEmail = dto.UserEmail,
                CreatedDate = DateTime.UtcNow, // Server sets these dates
                RowKey = Guid.NewGuid().ToString() // Server generates RowKey
            };

            loginEntity.PasswordHash = _hasher.HashPassword(loginEntity, dto.Password);
            loginEntity.PartitionKey = EmailHelper.ExtractDomainFromEmail(dto.UserEmail);

            if (string.IsNullOrEmpty(loginEntity.RowKey))
            {
                loginEntity.RowKey = Guid.NewGuid().ToString(); // Ensure unique row key
            }

            loginEntity.CreatedDate = DateTime.UtcNow;

            await _tableClient.AddEntityAsync(loginEntity);
            return ApiResponse<bool>.Ok(true, "User created successfully.");

        }


        public async Task<bool> EmailExistsAsync(string email)
        {
            var lowerEmail = email.ToLowerInvariant(); // Normalize
            var queryResults = _tableClient.QueryAsync<LoginEntity>(u => u.UserEmail == lowerEmail);

            await foreach (var entity in queryResults)
            {
                return true; // At least one match found
            }

            return false; // No match
        }


    }
}
